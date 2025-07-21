import { Injectable } from '@angular/core';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';

@Injectable({
  providedIn: 'root'
})
export class AppInsightsService {
  appInsights: ApplicationInsights;

  constructor() {
    this.appInsights = new ApplicationInsights({
      config: {
        connectionString: 'InstrumentationKey=1d6f704d-005f-42b9-b795-f4dd5e505d67;IngestionEndpoint=https://centralindia-0.in.applicationinsights.azure.com/;LiveEndpoint=https://centralindia.livediagnostics.monitor.azure.com/;ApplicationId=207462e7-78de-4bba-a9aa-2f85805fbf81',
        enableAutoRouteTracking: true,
        enableCorsCorrelation: true
      }
    });
    this.appInsights.loadAppInsights();
  }

  logPageView(name?: string) {
    this.appInsights.trackPageView({ name });
  }

  logEvent(name: string, properties?: { [key: string]: any }) {
    this.appInsights.trackEvent({ name }, properties);
  }

  logException(exception: Error, severityLevel?: number) {
    this.appInsights.trackException({ exception, severityLevel });
  }
}
