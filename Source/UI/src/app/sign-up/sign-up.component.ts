import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Router} from "@angular/router";
import {ApiService} from "../services/api.service";
import {count } from"../app.component"

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss'],
})
export class SignUpComponent implements OnInit{

  signupForm: FormGroup
  hide = true;
  public count1=count;
  messageEnable = false
  message= [
    {severity:'success', summary:'Success', detail:'Message Content'}
  ];

  constructor(
    public router: Router,
    private formBuilder: FormBuilder,
    private apiService: ApiService,
  ) {}

  ngOnInit() {
    this.signupForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.pattern('^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{8,}$')]]
    })
  }

  get signupFormCtrl() {
    return this.signupForm.controls
  }


  submit() {
    this.apiService.signupApi(this.signupForm.value).subscribe(response => {
      if(response) this.messageEnable = true
    }, (err) => {
      this.messageEnable = false
    })
  }

}
