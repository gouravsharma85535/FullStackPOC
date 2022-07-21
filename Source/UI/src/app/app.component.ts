import {Component, EventEmitter, HostBinding, Output} from '@angular/core';
import {MatSlideToggleChange} from "@angular/material/slide-toggle";
import {FormControl} from "@angular/forms";

export let count=12;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})


export class AppComponent {


  public isDark = false;

  @HostBinding('class')
  get themeMode(){
    return this.isDark ? 'theme-dark' : 'theme-light';
  }


  scaleUp() {
  count++

  }

  scaleDown(){
    count--
  }

  returnCount(){
    return count
  }
}
