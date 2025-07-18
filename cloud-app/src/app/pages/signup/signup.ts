import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  standalone: true,
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.scss',
  imports: [ReactiveFormsModule, CommonModule],
})
export class SignupComponent {
  form = this.fb.group({
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

  error = '';
  success = '';

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) { }

  onSubmit() {
    if (this.form.invalid || this.form.value.password !== this.form.value.confirmPassword) {
      this.error = 'Please fix the errors above.';
      return;
    }

    this.auth.register(this.form.value).subscribe({
      next: (res) => {
        this.success = 'Registration successful. Please check your email.';
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.error = err.error || 'Registration failed.';
      },
    });
  }
}
