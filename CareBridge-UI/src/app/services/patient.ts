import { Injectable, computed, inject, signal, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import { Patient } from '../models/patient';

@Injectable({ providedIn: 'root' })
export class PatientService implements OnDestroy {
  private readonly _http = inject(HttpClient);
  private readonly _apiUrl = 'http://localhost:5138/api/patient';
  private readonly _signalRUrl = 'http://localhost:5138/patientHub';

  private readonly _overduePatients = signal<Patient[]>([]);
  readonly overduePatients = this._overduePatients.asReadonly();
  readonly overdueCount = computed(() => this.overduePatients().length);

  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  private _connection: signalR.HubConnection;

  constructor() {
    this._connection = new signalR.HubConnectionBuilder()
      .withUrl(this._signalRUrl)
      .withAutomaticReconnect()
      .build();

    // Listen for the SignalR broadcast from the .NET Controller
    this._connection.on('PatientUpdated', (updatedPatients: Patient[]) => {
      console.log('SignalR Broadcast Received!', updatedPatients);
      this._overduePatients.set(updatedPatients);
    });

    this._connection.start().catch((err) => console.error('SignalR Error:', err));
    this.loadOverduePatients();
  }

  async loadOverduePatients() {
    this.loading.set(true);
    try {
      const overdueUrl = this._apiUrl + '/overdue';
      const data = await firstValueFrom(this._http.get<Patient[]>(overdueUrl));
      this._overduePatients.set(data ?? []);
    } catch {
      this.error.set('Could not retrieve records.');
    } finally {
      this.loading.set(false);
    }
  }

  async addPatient(newPatient: Patient) {
    try {
      // This ensures the user sees the patient even if SignalR is slow
      this._overduePatients.update((prev) => [...prev, newPatient]);

      // Send to API
      await firstValueFrom(this._http.post<Patient>(this._apiUrl, newPatient));

      console.log('Patient added successfully to Server');
    } catch (err) {
      console.error('Failed to add patient to the API', err);

      // ROLLBACK: If the server fails, remove them to keep data accurate
      this.loadOverduePatients();
      this.error.set('Server rejected the new patient.');
    }
  }

  markResolved(id: number) {
    this._overduePatients.update((list) => list.filter((p) => p.id !== id));
  }

  ngOnDestroy() {
    this._connection.stop();
  }
}
