import { Component, inject, resource, ResourceRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PatientService } from './services/patient';
import { Patient } from './models/patient';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class AppRoot {
  private readonly patientService = inject(PatientService);

  patientsResource: ResourceRef<Patient[] | undefined> = resource({
    loader: () => this.patientService.getOverduePatients(),
  });
}
