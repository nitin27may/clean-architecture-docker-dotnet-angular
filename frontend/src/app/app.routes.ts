import { Routes } from '@angular/router';
export const routes: Routes = [
  {
    path: 'contacts',
    loadChildren: () => import('@features/contact/contact.routes'),
  },
  {
    path: 'admin',
    loadChildren: () => import('@features/admin/admin.routes'),
  },
  {
    path: '',
    loadChildren: () => import('@features/user/user.routes'),
  },
];
