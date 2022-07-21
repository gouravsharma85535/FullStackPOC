import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(
    private httpClient: HttpClient
  ) { }

  signupApi(data: any): Observable<any> {
    return this.httpClient.post('https://localhost:7149/api/Values/Register', data)
  }

  login(data: any): Observable<any> {
    return this.httpClient.post('https://localhost:7149/api/Values/Login', data)
  }

  dashboard(): Observable<any>{
    return this.httpClient.get('https://localhost:7149/api/token')
  }
}

// Test API
//https://jsonplaceholder.typicode.com/users
