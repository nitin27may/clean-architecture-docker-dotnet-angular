import { Routes } from '@angular/router';
import { redirectToHomeIfAuthenticated, redirectToLoginIfNotAuthenticated } from '@core/guards';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { ProfileComponent } from './profile/profile.component';
import { RegisterComponent } from './register/register.component';
import LayoutComponent from '@core/layout/layout.component';

export default [
    {
        path: '',
        component: LayoutComponent,
        canActivate: [redirectToLoginIfNotAuthenticated()],
        children: [
            {
                path: '',
                component: HomeComponent,
            },
            {
                path: 'profile',

                component: ProfileComponent,
            },
        ],
    },
    {
        path: 'login',
        component: LoginComponent,
        canActivate: [redirectToHomeIfAuthenticated()],
    },

    {
        path: 'signup',
        component: RegisterComponent,
    },
] as Routes;
