import { Injectable, computed, inject, signal, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import { Patient } from '../models/patient';

@Injectable({ providedIn: 'root' })
export class PatientService implements OnDestroy {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5138/api/patient';

  private readonly _overduePatients = signal<Patient[]>([]);
  readonly overduePatients = this._overduePatients.asReadonly();
  readonly overdueCount = computed(() => this.overduePatients().length);

  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  private connection: signalR.HubConnection;

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5138/patientHub')
      .withAutomaticReconnect()
      .build();

    // Listen for the SignalR broadcast from the .NET Controller
    this.connection.on('PatientUpdated', (updatedPatients: Patient[]) => {
      console.log('SignalR Broadcast Received!', updatedPatients);
      this._overduePatients.set(updatedPatients);
    });

    this.connection.start().catch((err) => console.error('SignalR Error:', err));
    this.loadOverduePatients();
  }

  async loadOverduePatients() {
    this.loading.set(true);
    try {
      const data = await firstValueFrom(this.http.get<Patient[]>(`${this.apiUrl}/overdue`));
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

      // 2. Send to API
      await firstValueFrom(this.http.post<Patient>(this.apiUrl, newPatient));

      console.log('Patient added successfully to Server');
    } catch (err) {
      console.error('Failed to add patient to the API', err);

      // 3. ROLLBACK: If the server fails, remove them to keep data accurate
      this.loadOverduePatients();
      this.error.set('Server rejected the new patient.');
    }
  }

  markResolved(id: number) {
    this._overduePatients.update((list) => list.filter((p) => p.id !== id));
  }

  ngOnDestroy() {
    this.connection.stop();
  }
}
