import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    children: [
        {
            path: '',
            loadChildren: () => import('./feature/user/user.routes'),
        },
        {
          path: 'contacts',
          loadChildren: () => import('./feature/contact/contact.routes'),
      },
    ],
},
];
