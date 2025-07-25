import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { routes } from './app.routes';
import { MSAL_INSTANCE, MsalService, MsalBroadcastService, MsalGuard } from '@azure/msal-angular';
import {msalInstance} from './auth/msal-config';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    provideRouter(routes),
    {
      provide: MSAL_INSTANCE,
      useValue: msalInstance,
    },
    MsalService,
    MsalBroadcastService,
    MsalGuard,
  ],
};


/*
MSAL_INSTANCE provides the configured MSAL instance to Angular.
MsalService is used for login and token handling.
MsalGuard can be used to protect routes (optional).
MsalBroadcastService helps listen for login/logout events.*/
