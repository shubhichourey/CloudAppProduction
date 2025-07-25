import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { msalInstance } from './app/auth/msal-config';
import { App } from './app/app';

async function main() {
  try {
    //Initialize MSAL instance
    await msalInstance.initialize();

    // Handle redirect after MS login
    const response = await msalInstance.handleRedirectPromise();

    if (response && response.account) { //If there is a response (user just logged in) AND it contains account details
      msalInstance.setActiveAccount(response.account);
      console.log('MSAL redirect success:', response.account);
      window.location.href = '/dashboard';
    } else {
      const accounts = msalInstance.getAllAccounts();
      //Checks if there are any previously logged-in accounts stored in cache/localStorage.
      // If found, it sets the first account as the active account.
      if (accounts.length > 0) {
        msalInstance.setActiveAccount(accounts[0]);
      }
    }

    await bootstrapApplication(App, appConfig);
  } catch (error) {
    console.error('MSAL redirect error:', error);
  }
}

main();
