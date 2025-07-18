import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { msalInstance } from './app/auth/msal-config';
import { App } from './app/app';

async function main() {
  try {
    // IMPORTANT: Initialize MSAL instance before anything else
    await msalInstance.initialize();

    // Handle redirect after MS login
    const response = await msalInstance.handleRedirectPromise();

    if (response && response.account) {
      msalInstance.setActiveAccount(response.account);
      console.log('MSAL redirect success:', response.account);
      window.location.href = '/dashboard';
    } else {
      const accounts = msalInstance.getAllAccounts();
      if (accounts.length > 0) {
        msalInstance.setActiveAccount(accounts[0]);
      }
    }

    // Now bootstrap Angular app
    await bootstrapApplication(App, appConfig);
  } catch (error) {
    console.error('MSAL redirect error:', error);
  }
}

main();
