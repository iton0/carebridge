import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { PatientService } from './services/patient';
import { Patient } from './models/patient';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class AppRoot implements OnInit {
  private patientService: PatientService = inject(PatientService);

  patients$: Observable<Patient[]> | undefined;

  ngOnInit() {
    this.patients$ = this.patientService.getOverduePatients();
  }
}
