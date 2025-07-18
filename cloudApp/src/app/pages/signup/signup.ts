import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import {AuthService} from '../../services/auth';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './signup.html',
  styleUrls: ['./signup.css']
})
export class SignupComponent {
  form: FormGroup;
  error = '';
  success = '';

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      email: [
        '',
        [
          Validators.required,
          Validators.email,
          Validators.pattern(/^[a-zA-Z0-9._%+-]+@(gmail\.com|epam\.com)$/),
        ],
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$/),
        ],
      ],
      confirmPassword: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.error = 'Please correct the errors.';
      return;
    }

    if (this.form.value.password !== this.form.value.confirmPassword) {
      this.error = 'Passwords do not match.';
      return;
    }

    const requestData = {
      name: this.form.value.name,
      email: this.form.value.email,
      password: this.form.value.password,
      confirmPassword: this.form.value.confirmPassword // <- 🔥 IMPORTANT
    };

    this.auth.register(requestData).subscribe({
      next: (res: any) => {
        this.success = 'Registration successful!';
        //this.router.navigate(['/login']);
      },
      error: (err: any) => {
        console.error('Registration error (full object):', err);
        console.log('err.error:', err.error);

        if (err.status === 400 && err.error?.errors) {
          const messages = Array.isArray(err.error.errors)
            ? err.error.errors
            : Object.values(err.error.errors).flat();  // support both string[] or object
          this.error = messages.join(', ');
        } else if (typeof err.error === 'string') {
          this.error = err.error;
        } else if (err.error?.message) {
          this.error = err.error.message;
        } else {
          this.error = 'Registration failed.';
        }
      }
    });
  }
}
