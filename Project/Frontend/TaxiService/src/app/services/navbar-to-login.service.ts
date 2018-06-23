import { Injectable, Output, EventEmitter } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NavbarToLoginService {

  @Output() change: EventEmitter<boolean> = new EventEmitter();

  logout(){
    this.change.emit(true);
  }
}
