import { Component, computed, inject, linkedSignal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PatientService } from './services/patient';
import { Patient } from './models/patient';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatListModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    CommonModule,
  ],
  templateUrl: './app.html',
  styleUrls: ['./app.css'],
})
export class AppRoot {
  public readonly patientService = inject(PatientService);

  searchQuery = linkedSignal<number, string>({
    source: () => this.patientService.overdueCount(),
    computation: () => '',
  });

  filteredPatients = computed(() => {
    const query = this.searchQuery().toLowerCase().trim();
    const patients = this.patientService.overduePatients();

    if (!query) return patients;

    return patients.filter((p) => {
      const first = p.givenName?.toLowerCase() ?? '';
      const last = p.familyName?.toLowerCase() ?? '';
      return first.includes(query) || last.includes(query);
    });
  });

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

  updateSearch(event: Event) {
    const input = event.target as HTMLInputElement;
    this.searchQuery.set(input.value);
  }
}
