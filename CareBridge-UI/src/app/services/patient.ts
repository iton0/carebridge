import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { toSignal } from '@angular/core/rxjs-interop';
import { Subject, switchMap, tap, catchError, of, startWith } from 'rxjs';
import { Patient } from '../models/patient';
import { SignalRService } from './signalr';

@Injectable({ providedIn: 'root' })
export class PatientService {
  private readonly _http = inject(HttpClient);
  private readonly _signalR = inject(SignalRService);
  private readonly _apiUrl = 'http://localhost:5138/api/patient';

  // --- State Signals ---
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  private readonly _resolvedIds = signal<Set<number>>(new Set());

  private readonly _localOverdueUpdates = signal<Patient[]>([]);

  private readonly _loadTrigger = new Subject<void>();

  private readonly _remoteOverduePatients = toSignal(
    this._loadTrigger.pipe(
      startWith(void 0),
      tap(() => this.loading.set(true)),
      switchMap(() =>
        this._http.get<Patient[]>(`${this._apiUrl}/overdue`).pipe(
          tap(() => {
            this._localOverdueUpdates.set([]); // Clear local on fresh sync
            this.error.set(null);
          }),
          catchError(() => {
            this.error.set('Could not retrieve records.');
            return of([]);
          }),
          tap(() => this.loading.set(false)),
        ),
      ),
    ),
    { initialValue: [] as Patient[] },
  );

  readonly overduePatients = computed(() => {
    const remote = this._remoteOverduePatients();
    const local = this._localOverdueUpdates();
    const resolved = this._resolvedIds();

    const combined = [...remote, ...local];

    // Filter out duplicates AND anything the user marked as resolved
    const uniqueMap = new Map<number, Patient>();
    for (const p of combined) {
      if (!resolved.has(p.id)) {
        uniqueMap.set(p.id, p);
      }
    }

    return Array.from(uniqueMap.values());
  });

  readonly overdueCount = computed(() => this.overduePatients().length);

  constructor() {
    this._signalR.onPatientUpdate((updatedPatients) => {
      this._localOverdueUpdates.set(updatedPatients);
    });
  }

  loadOverduePatients() {
    this._loadTrigger.next();
  }

  async addPatient(newPatient: Patient) {
    this._localOverdueUpdates.update((prev) => [...prev, newPatient]);

    try {
      await fetch(this._apiUrl, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(newPatient),
      });
    } catch (err) {
      // Rollback on failure
      this._localOverdueUpdates.update((prev) => prev.filter((p) => p !== newPatient));
      this.error.set('Server rejected the new patient.');
      console.error(err);
    }
  }

  markResolved(id: number) {
    this._resolvedIds.update((set) => {
      const newSet = new Set(set);
      newSet.add(id);
      return newSet;
    });
  }
}
