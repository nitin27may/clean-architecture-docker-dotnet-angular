import { RouterModule, Routes } from '@angular/router';
import { NgModule } from "@angular/core";


export const routes: Routes = [
    {
        path: '',
        children: [
            {
                path: '',
                loadChildren: () => import('app/feature/user/user.routes'),
            },
        ],
    },
    {
        path: 'contacts',
        children: [
            {
                path: '',
                loadChildren: () =>
                    import('app/feature/contact/contact.routes'),
            },
        ],
    },
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
