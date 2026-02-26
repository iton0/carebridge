import { signal, computed, inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { Patient } from '../models/patient';

// Static utility kept outside the class for maximum portability
export const mergeAndFilter = (
  remote: Patient[],
  local: Patient[],
  resolved: Set<number>,
): Patient[] => {
  const seen = new Set<number>();
  const result: Patient[] = [];

  const process = (p: Patient) => {
    if (!seen.has(p.id) && !resolved.has(p.id)) {
      seen.add(p.id);
      result.push(p);
    }
  };

  local.forEach(process);
  remote.forEach(process);

  return result;
};

@Injectable()
export class PatientStore {
  private readonly http = inject(HttpClient);

  readonly remotePatients = signal<Patient[]>([]);
  readonly localUpdates = signal<Patient[]>([]);
  readonly resolvedIds = signal<Set<number>>(new Set());
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  // In PatientStore
  readonly overduePatients = computed(() => {
    const remote = this.remotePatients();
    const local = this.localUpdates();
    const resolved = this.resolvedIds();
    const threshold = new Date();
    threshold.setFullYear(threshold.getFullYear() - 1);

    return mergeAndFilter(remote, local, resolved).filter((p) => {
      const pDate = new Date(p.lastScreeningDate);
      return pDate < threshold;
    });
  });

  readonly overdueCount = computed(() => this.overduePatients().length);

  async load(apiUrl: string) {
    this.loading.set(true);
    try {
      const data = await firstValueFrom(this.http.get<Patient[]>(`${apiUrl}/overdue`));
      this.remotePatients.set(data);
      this.error.set(null);
    } catch {
      this.error.set('Failed to load patients.');
    } finally {
      this.loading.set(false);
    }
  }

  async addPatient(apiUrl: string, newPatient: Patient) {
    this.localUpdates.update((prev) => [...prev, newPatient]);
    try {
      const saved = await firstValueFrom(this.http.post<Patient>(apiUrl, newPatient));
      this.localUpdates.update((prev) => prev.map((p) => (p === newPatient ? saved : p)));
    } catch {
      this.localUpdates.update((prev) => prev.filter((p) => p !== newPatient));
    }
  }

  resolve(id: number) {
    this.resolvedIds.update((set) => {
      const next = new Set(set);
      next.add(id);
      return next;
    });

    this.localUpdates.update((prev) => prev.filter((p) => p.id !== id));
  }
}
