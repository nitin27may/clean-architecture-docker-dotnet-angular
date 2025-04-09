import { Component, OnInit } from '@angular/core';
import { NotificationService } from '@core/services/notification.service';

@Component({
  selector: 'app-notification-center',
  templateUrl: './notification-center.component.html',
  styleUrls: ['./notification-center.component.scss']
})
export class NotificationCenterComponent implements OnInit {
  notifications: any[] = [];

  constructor(private notificationService: NotificationService) {}

  ngOnInit(): void {
    this.notificationService.getNotificationObservable().subscribe(notification => {
      this.notifications.push(notification);
    });
  }

  markAsRead(notification: any): void {
    notification.isRead = true;
    // Call backend service to mark notification as read
  }

  archiveNotification(notification: any): void {
    // Call backend service to archive notification
  }
}
