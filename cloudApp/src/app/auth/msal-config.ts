import { PublicClientApplication } from '@azure/msal-browser';

export const msalInstance = new PublicClientApplication({
  auth: {
    clientId: 'ca3023f2-9dc0-4fb7-b3ab-5d4b6fa3f3df',
    authority: 'https://login.microsoftonline.com/b41b72d0-4e9f-4c26-8a69-f949f367c91d',
    redirectUri: 'http://localhost:4200',
  },
  cache: {
    cacheLocation: 'localStorage',
    storeAuthStateInCookie: true,
  },
});


