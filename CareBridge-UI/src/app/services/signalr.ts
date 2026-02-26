import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Patient } from '../models/patient';
import { PatientStore } from './patient';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private readonly _url = 'http://localhost:5138/patientHub';
  private _connection: signalR.HubConnection;

  constructor() {
    this._connection = new signalR.HubConnectionBuilder()
      .withUrl(this._url)
      .withAutomaticReconnect()
      .build();
  }

  async start(store: PatientStore) {
    if (this._connection.state !== signalR.HubConnectionState.Disconnected) return;

    this._connection.on('PatientUpdated', (data: Patient[]) => {
      store.localUpdates.set(data);
    });

    await this._connection.start();
  }

  async stop() {
    this._connection.off('PatientUpdated');
    if (this._connection.state !== signalR.HubConnectionState.Disconnected) {
      await this._connection.stop();
    }
  }
}
