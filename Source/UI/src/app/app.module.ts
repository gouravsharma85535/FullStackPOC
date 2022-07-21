import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule, HTTP_INTERCEPTORS} from "@angular/common/http";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {MatButtonModule} from "@angular/material/button";
import {MatCardModule} from "@angular/material/card";
import { SignUpComponent } from './sign-up/sign-up.component';
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatTabsModule} from '@angular/material/tabs';
import {MatIconModule} from '@angular/material/icon';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import {ButtonModule} from "primeng-lts/button";
import {RatingModule} from "primeng-lts/rating";
import {MessagesModule} from 'primeng-lts/messages';
import {MessageModule} from 'primeng-lts/message';
import {MatDialogModule} from "@angular/material/dialog";
import {ApiInterceptor} from "./api.interceptor"


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent,
    SignUpComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    FlexLayoutModule,
    MatToolbarModule,
    MatTabsModule,
    MatIconModule,
    MatSlideToggleModule,
    ButtonModule,
    RatingModule,
    MessageModule,
    MessagesModule,
    MatDialogModule,
  
  ],
  providers: [{
    provide : HTTP_INTERCEPTORS,
    useClass : ApiInterceptor,
    multi : true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
