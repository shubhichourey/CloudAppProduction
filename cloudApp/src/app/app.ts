import { Component, OnInit  } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppInsightsService } from './services/app-insights.service';
import { MsalRedirectComponent } from '@azure/msal-angular';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: `
    <router-outlet></router-outlet>`

})
export class App implements OnInit {

  constructor(private appInsights: AppInsightsService) {}

  ngOnInit(): void {
    this.appInsights.logPageView('App Loaded');
  }
}



