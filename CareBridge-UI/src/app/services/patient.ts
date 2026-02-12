import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { Patient } from '../models/patient';

@Injectable({
  providedIn: 'root',
})
export class PatientService {
  private readonly http: HttpClient = inject(HttpClient);
  private readonly apiUrl: string = 'http://localhost:5138/api/patient';

  /**
   * Fetches overdue patients.
   * @throws Error if the API is unreachable.
   */
  getOverduePatients(): Promise<Patient[]> {
    return firstValueFrom(this.http.get<Patient[]>(`${this.apiUrl}/overdue`));
  }
}
