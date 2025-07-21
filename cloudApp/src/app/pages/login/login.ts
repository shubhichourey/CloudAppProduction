import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';
import { MsalService } from '@azure/msal-angular';
import { InteractionType, RedirectRequest } from '@azure/msal-browser';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {
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
      scopes: ['user.read', 'email',  'openid', 'profile', 'api://ca3023f2-9dc0-4fb7-b3ab-5d4b6fa3f3df/user_impersonation']
    } as RedirectRequest);
  }
}


