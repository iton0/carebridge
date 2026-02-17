import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Patient } from '../models/patient';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private readonly _signalRUrl = 'http://localhost:5138/patientHub';
  private connection: signalR.HubConnection;

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(this._signalRUrl)
      .withAutomaticReconnect()
      .build();
  }

  // A method to start and return the listener
  onPatientUpdate(callback: (patients: Patient[]) => void) {
    this.connection.on('PatientUpdated', callback);
    this.connection.start().catch((err) => console.error(err));
  }
}
