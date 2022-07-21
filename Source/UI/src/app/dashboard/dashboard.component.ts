import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiService } from '../services/api.service';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  val1: number;
  responseData : any = '';
  st:any="ok";
  constructor(
    private apiService : ApiService,
  ) { }

  ngOnInit(){

  }

  userName(){
    this.apiService.dashboard().subscribe(response=>{
      this.responseData = response.user
      console.log(response.user)
    })
    }
    
  }

