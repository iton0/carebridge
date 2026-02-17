import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { Patient } from '../models/patient';
import { SignalRService } from './signalr';

@Injectable({ providedIn: 'root' })
export class PatientService {
  private readonly _http = inject(HttpClient);
  private readonly _signalR = inject(SignalRService);

  private readonly _apiUrl = 'http://localhost:5138/api/patient';

  private readonly _overduePatients = signal<Patient[]>([]);
  readonly overduePatients = this._overduePatients.asReadonly();
  readonly overdueCount = computed(() => this.overduePatients().length);

  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  constructor() {
    // Listen for SignalR updates
    this._signalR.onPatientUpdate((updatedPatients) => {
      this._overduePatients.set(updatedPatients);
    });

    this.loadOverduePatients();
  }

  async loadOverduePatients() {
    this.loading.set(true);
    try {
      const overdueUrl = `${this._apiUrl}/overdue`;
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
      // Update UI before server responds
      this._overduePatients.update((prev) => [...prev, newPatient]);

      await firstValueFrom(this._http.post<Patient>(this._apiUrl, newPatient));
      console.log('Patient added successfully to Server');
    } catch (err) {
      console.error('Failed to add patient to the API', err);
      // Rollback on failure
      this.loadOverduePatients();
      this.error.set('Server rejected the new patient.');
    }
  }

  markResolved(id: number) {
    this._overduePatients.update((list) => list.filter((p) => p.id !== id));
  }
}
