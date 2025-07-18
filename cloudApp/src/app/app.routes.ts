// app/app.routes.ts
import { Routes } from '@angular/router';
import { provideRouter } from '@angular/router';
import {LoginComponent} from './pages/login/login';
import {SignupComponent} from './pages/signup/signup';
import { MsalRedirectComponent } from '@azure/msal-angular';
import { DashboardComponent } from './pages/dashboard/dashboard';
import { MsalGuard } from '@azure/msal-angular';


export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent},//,canActivate: [MsalGuard]
  { path: 'signup', component: SignupComponent},
  { path: 'redirect', component: MsalRedirectComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];

export const appRoutes = provideRouter(routes);
