import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthenticatedResponse } from '../models/authenticated-response.model';
import { Router } from '@angular/router';
import { decode } from 'base-64';
window.atob = decode;

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7068/auth/';
  private isAuthorizedSubject = new BehaviorSubject<boolean>(this.isTokenValid());
  isAuthorized$ = this.isAuthorizedSubject.asObservable();

  private userRoleSubject = new BehaviorSubject<string | null>(this.getUserRoleFromToken());
  userRole$ = this.userRoleSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  login(username: string, password: string): Observable<AuthenticatedResponse> {
    return this.http.post<AuthenticatedResponse>(`${this.apiUrl}Auth/login`, { username, password }).pipe(
      tap((response: AuthenticatedResponse) => {
        localStorage.setItem('accessToken', response.accessToken);
        localStorage.setItem('refreshToken', response.refreshToken);
        const userRole = this.getUserRoleFromToken();
        this.userRoleSubject.next(userRole);
        this.isAuthorizedSubject.next(true);
      })
    );
  }

  register(user: { email: string; login: string; password: string }): Observable<AuthenticatedResponse> {
    return this.http.post<AuthenticatedResponse>(`${this.apiUrl}Auth/register`, user).pipe(
      tap((response: AuthenticatedResponse) => {
        localStorage.setItem('accessToken', response.accessToken);
        localStorage.setItem('refreshToken', response.refreshToken);
        const userRole = this.getUserRoleFromToken();
        this.userRoleSubject.next(userRole);
        this.isAuthorizedSubject.next(true);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    this.userRoleSubject.next(null);
    this.isAuthorizedSubject.next(false);
    this.router.navigate(['/login']);
  }

  refreshToken(): Observable<AuthenticatedResponse> {
    const refreshToken = localStorage.getItem('refreshToken');
    const accessToken = localStorage.getItem('accessToken');
    if (!refreshToken) {
      throw new Error('No refresh token available');
    }

    return this.http.post<AuthenticatedResponse>(`${this.apiUrl}api/Token/refresh`, { accessToken, refreshToken }).pipe(
      tap((response: AuthenticatedResponse) => {
        localStorage.setItem('accessToken', response.accessToken);
        localStorage.setItem('refreshToken', response.refreshToken);
        const userRole = this.getUserRoleFromToken();
        this.userRoleSubject.next(userRole);
      })
    );
  }

  private isTokenValid(): boolean {
    const token = localStorage.getItem('accessToken');
    return !!token;
  }

  private getUserRoleFromToken(): string | null {

    const token = localStorage.getItem('accessToken');

    if (!token) {
      return null;
    }

    const payload = token.split('.')[1];
    if (!payload) {
      return null;
    }

    const decodedPayload = atob(payload);
    const payloadObject = JSON.parse(decodedPayload);
    return payloadObject['role'] || payloadObject['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
  }


  isAdmin(): boolean {
    return this.userRoleSubject.value === "admin";
  }

  checkTokenValidity(): void {
    const tokenIsValid = this.isTokenValid();
    this.isAuthorizedSubject.next(tokenIsValid);
  }
}
