import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {SignUpComponent} from "./sign-up/sign-up.component";
import {DashboardComponent} from "./dashboard/dashboard.component";

const routes: Routes = [
  {path: '', pathMatch: 'full', redirectTo: 'sign-up'},
  {path: 'login', component: LoginComponent},
  {path: 'dashboard', component: DashboardComponent},
  {path: 'sign-up', component: SignUpComponent},
  {path: '**', redirectTo: 'login'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
