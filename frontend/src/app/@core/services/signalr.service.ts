import { Injectable, inject } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { AuthStateService } from './auth-state.service';
import { environment } from '@environments/environment';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection;
  private authState = inject(AuthStateService);
  private notificationService = inject(NotificationService);

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiEndpoint}/hubs/contact`, {
        accessTokenFactory: () => this.authState.getCurrentUser().token
      })
      .configureLogging(LogLevel.Information)
      .build();

    this.registerOnServerEvents();
    this.startConnection();
  }

  private startConnection(): void {
    this.hubConnection
      .start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.log('Error while starting SignalR connection: ' + err));
  }

  private registerOnServerEvents(): void {
    this.hubConnection.on('ContactAdded', (data) => {
      this.notificationService.success('Contact added: ' + data.firstName + ' ' + data.lastName);
    });

    this.hubConnection.on('ContactUpdated', (data) => {
      this.notificationService.info('Contact updated: ' + data.firstName + ' ' + data.lastName);
    });

    this.hubConnection.on('ContactDeleted', (id) => {
      this.notificationService.warning('Contact deleted with ID: ' + id);
    });

    this.hubConnection.on('ReceiveMessage', (message) => {
      this.notificationService.info('New message: ' + message);
    });

    this.hubConnection.on('ReceiveNotification', (notification) => {
      this.notificationService.info('New notification: ' + notification.message);
    });
  }

  public sendMessage(message: string): void {
    this.hubConnection.invoke('SendMessage', message)
      .catch(err => console.error(err));
  }
}
