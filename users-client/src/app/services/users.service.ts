import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import User from '../models/User';
import { Observable } from 'rxjs';
import Login from '../models/Login';
import { environment } from 'src/environments/environment';
import { UserRole } from '../enum/userRole';

@Injectable({
  providedIn: 'root'
})

export class UsersService {

  private baseUrl = environment.baseUrl;
  private api = environment.userApi;
  private loginUrl = environment.login;
  private detailsUrl = environment.details;
  private registration = environment.registration;
  private update = environment.update;
  private assignRoleUrl = environment.assignRole;

  constructor(private http: HttpClient) { }

  private headers() {
    let httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
      withCredentials: true
    };
    return httpOptions;
  }

  login(user: Login): Observable<Login> {
    return this.http.post<Login>(this.baseUrl + this.api + this.loginUrl, user, this.headers())
  }

  sendDetailsToEmail(email: string): Observable<any> {
    return this.http.post<any>(this.baseUrl + this.api + this.detailsUrl + '?email=' + encodeURIComponent(email), { email }, this.headers());
  }

  register(formData: FormData): Observable<any> {

    return this.http.post<any>(this.baseUrl + this.api + this.registration, formData);
  }

  getUserById(id: number): Observable<User> {
    return this.http.get<User>(this.baseUrl + this.api + '/' + id);
  }

  deleteUser(id: number): Observable<User> {
    return this.http.delete<User>(this.baseUrl + this.api + '/' + id);
  }

  deleteAllUsers(): Observable<User[]> {
    return this.http.delete<User[]>(this.baseUrl + this.api);
  }

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + this.api);
  }

  updateUser(id: number, formData: FormData): Observable<User> {
    return this.http.put<User>(this.baseUrl + this.api + this.update + '?id=' + id, formData);
  }

  assignUserRole(id: number, userRole: UserRole): Observable<User> {
    return this.http.patch<User>(this.baseUrl + this.api + this.assignRoleUrl + '?id=' + id
      + '&userRole=' + userRole,
      userRole, this.headers());
  }

  mapUserRole(role: UserRole): string {
    switch (role) {
      case UserRole.MAIN_ADMIN:
        return 'MAIN_ADMIN';
      case UserRole.ADMIN:
        return 'ADMIN';
      case UserRole.USER:
        return 'USER';
      default:
        return '';
    }
  }

  getPhotoUrl(photoUrl: string): string {
    const serverBaseUrl = environment.baseUrl;
    return `${serverBaseUrl}${photoUrl}`;
  }
}
