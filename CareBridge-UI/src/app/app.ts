import { Component, computed, inject, signal, DestroyRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PatientStore } from './services/patient';
import { SignalRService } from './services/signalr';
import { AuthService } from './services/auth';
import { createPatient } from './models/patient';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatChipsModule } from '@angular/material/chips';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    MatChipsModule,
    MatFormFieldModule,
    MatToolbarModule,
    MatIconModule,
    MatListModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    CommonModule,
  ],
  templateUrl: './app.html',
  styleUrls: ['./app.css'],
  providers: [PatientStore],
})
export class AppRoot {
  private readonly _apiURL = 'http://localhost:5138/api/patient';

  public readonly store = inject(PatientStore);
  public readonly auth = inject(AuthService);

  readonly searchQuery = signal('');

  readonly filteredPatients = computed(() => {
    const query = this.searchQuery().toLowerCase().trim();
    const patients = this.store.overduePatients();
    if (!query) return patients;
    return patients.filter(
      (p) =>
        p.givenName?.toLowerCase().includes(query) || p.familyName?.toLowerCase().includes(query),
    );
  });

  constructor() {
    const signalR = inject(SignalRService);
    const destroyRef = inject(DestroyRef);

    // 1. Connect Transport to Store
    void signalR.start(this.store);

    // 2. Initial Data Load
    void this.store.load(this._apiURL);

    // 3. Automated Cleanup
    destroyRef.onDestroy(() => signalR.stop());
  }

  addNewPatient() {
    const newPatient = createPatient({
      familyName: 'Test',
      givenName: 'Patient',
    });
    this.store.addPatient(this._apiURL, newPatient);
  }
}
