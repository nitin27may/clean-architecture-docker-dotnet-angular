import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NotificationService } from '@core/services/notification.service';
import { SignalRService } from '@core/services/signalr.service';

@Component({
  selector: 'app-admin-messages',
  templateUrl: './admin-messages.component.html',
  styleUrls: ['./admin-messages.component.scss']
})
export class AdminMessagesComponent {
  messageForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private notificationService: NotificationService,
    private signalRService: SignalRService
  ) {
    this.messageForm = this.fb.group({
      message: ['', Validators.required],
      recipient: ['']
    });
  }

  sendMessage(): void {
    if (this.messageForm.valid) {
      const message = this.messageForm.value.message;
      const recipient = this.messageForm.value.recipient;

      if (recipient) {
        // Send one-to-one message
        this.signalRService.sendMessageToUser(recipient, message);
        this.notificationService.success('Message sent to user: ' + recipient);
      } else {
        // Broadcast message to all users
        this.signalRService.broadcastMessage(message);
        this.notificationService.success('Message broadcasted to all users');
      }

      this.messageForm.reset();
    } else {
      this.notificationService.error('Please enter a message');
    }
  }
}
