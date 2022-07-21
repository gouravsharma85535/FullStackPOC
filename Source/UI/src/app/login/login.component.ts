import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ApiService} from "../services/api.service";
import {Router} from "@angular/router";
import { CookieService } from 'ngx-cookie-service'


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  hide = true;

  constructor(
    public router: Router,
    private apiService: ApiService,
    private formBuilder: FormBuilder,
    private cookieService: CookieService
  ) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      userName: ['', Validators.required],
      password: ['', [Validators.required, Validators.pattern('^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{8,}$')]]
    })
  }

get loginFormCtrl() {
  return this.loginForm.controls
}

  login() {
    this.apiService.login(this.loginForm.value).subscribe(response => {
      console.log(response.value.success)
      console.log(response.value.message)
      console.log(response.value.token)
      this.cookieService.set('bearer_token', response.token,{expires:0.5})
      alert(response.value.message)
      if(response.value.success){ 
      this.router.navigate(['dashboard'])
      }
      
      
    }, (err) => {
      alert(err.value.message)
    })
  }

}