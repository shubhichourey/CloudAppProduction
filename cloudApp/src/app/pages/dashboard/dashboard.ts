import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div style="text-align: center; margin-top: 50px;">
      <h1>Welcome to Angular Application 🎉</h1>
    </div>
  `
})
export class DashboardComponent {}
