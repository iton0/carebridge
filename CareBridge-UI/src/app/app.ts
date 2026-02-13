import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PatientService } from './services/patient';
import { Patient } from './models/patient';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './app.html',
  styleUrls: ['./app.css'],
})
export class AppRoot {
  public readonly patientService = inject(PatientService);

  addNewPatient() {
    const newPatient: Patient = {
      id: Math.floor(Math.random() * 10000),
      familyName: 'Old-Record',
      givenName: 'Test',
      lastScreeningDate: new Date('2021-01-01'),
      gender: 'other',
    };
    this.patientService.addPatient(newPatient);
  }
}
