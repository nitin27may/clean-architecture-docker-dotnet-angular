import { Routes } from '@angular/router';
import { redirectToLoginIfNotAuthenticated } from '@core/guards';
import LayoutComponent from '@core/layout/layout.component';
import { PermissionGuard } from '@core/guards/permission.guard';
import { ContactDetailsComponent } from './contact-details/contact-details.component';
import { ContactFormComponent } from './contact-form/contact-form.component';
import { ContactListComponent } from './contact-list/contact-list.component';
import { ContactDetailsResolver } from './contact.resolver';

export default [
  {
    path: '',
    component: LayoutComponent,
    canActivate: [redirectToLoginIfNotAuthenticated()],
    children: [
      {
        path: '',
        component: ContactListComponent,
        canActivate: [PermissionGuard('Contacts', 'Read')]
      },
      {
        path: 'create',
        component: ContactFormComponent,
        canActivate: [PermissionGuard('Contacts', 'Create')]
      },
      {
        path: 'edit/:contactId',
        component: ContactFormComponent,
        canActivate: [PermissionGuard('Contacts', 'Update')],
        resolve: { contactDetails: ContactDetailsResolver }
      },
      {
        path: 'details/:contactId',
        component: ContactDetailsComponent,
        canActivate: [PermissionGuard('Contacts', 'Read')],
        resolve: { contactDetails: ContactDetailsResolver }
      }
    ]
  }
] as Routes;
