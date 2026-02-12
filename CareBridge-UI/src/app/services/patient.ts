import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Patient } from '../models/patient';

@Injectable({
  providedIn: 'root'
})

export class PatientService {
  private apiUrl: string = 'http://localhost:5138/api/patient';

  constructor(private http: HttpClient) { }

  getOverduePatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(`${this.apiUrl}/overdue`);
  }
}
