import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  imports: [ReactiveFormsModule, CommonModule],
})
export class LoginComponent {
  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
  });

  error = '';
  success = '';

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) { }

  onSubmit() {
    if (this.form.invalid) {
      this.error = 'Please fill all fields correctly.';
      return;
    }

    this.auth.login(this.form.value).subscribe({
      next: () => {
        this.success = 'Login successful!';
        this.router.navigate(['/home']);
      },
      error: (err) => {
        this.error = err.error || 'Invalid credentials.';
      },
    });
  }
}
