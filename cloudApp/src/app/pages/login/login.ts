import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';
import { MsalService } from '@azure/msal-angular';
import { InteractionStatus, RedirectRequest } from '@azure/msal-browser';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  error = '';
  success = '';

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private msalService: MsalService
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  ngOnInit() {
    const account = this.msalService.instance.getActiveAccount(); //gets activated account
    if (!account) {
      console.warn('No active account found after redirect login.');
      return;
    }

    this.msalService.instance.acquireTokenSilent({ //silently takes the token so that user doesn't have to login again
      account,
      scopes: ['user.read', 'email', 'openid', 'profile', 'api://efe7d3e6-8fe5-4b82-b937-3b7ed8e9b2e7/user_impersonation']
    }).then(result => {
      console.log('Access Token:', result.accessToken);
      console.log('ID Token:', result.idToken);
      const email = account.username || '';
      console.log('Microsoft login email:', email);
    }).catch(err => {
      //On success, logs the access and ID tokens and the user's email
      console.error('Failed to acquire token silently:', err);
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.error = 'Please enter valid email and password.';
      return;
    }

    this.auth.login(this.form.value).subscribe({
      next: (res: any) => {
        this.success = res.message;

        Swal.fire({
          icon: 'success',
          title: 'Email Sent',
          text: 'Login successful. A confirmation email has been sent!',
          confirmButtonColor: '#3085d6'
        });

        this.router.navigate(['/dashboard']);
      },
      error: (err: any) => {
        if (err.error?.message) {
          this.error = err.error.message;
        } else {
          this.error = 'Invalid email or password.';
        }

        Swal.fire({
          icon: 'error',
          title: 'Login Failed',
          text: this.error,
          confirmButtonColor: '#d33'
        });
      },
    });
  }

  loginWithMicrosoft(): void {
    this.msalService.loginRedirect({
      scopes: ['user.read', 'email', 'openid', 'profile', 'api://efe7d3e6-8fe5-4b82-b937-3b7ed8e9b2e7/user_impersonation']
    } as RedirectRequest);
  }
}
/*
Triggers a redirect-based login with Microsoft/Azure AD,
On successful login, the user is redirected back to your app.
The MSAL will store the access token and ID token in cache.
After redirection, ngOnInit handles token retrieval silently.
 */
