import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MsalRedirectComponent } from '@azure/msal-angular';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: `
    <router-outlet></router-outlet>`

})
export class App {}



