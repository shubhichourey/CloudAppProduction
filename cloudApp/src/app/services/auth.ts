import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  //private readonly baseUrl = 'https://localhost:7180/api/auth';
  private readonly baseUrl = 'https://cloudappservice-abhkgshqhthfhxhq.centralindia-01.azurewebsites.net/api/auth';


  constructor(private http: HttpClient) {}

  register(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/register`, data);
  }

  login(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/login`, data);
  }
}
