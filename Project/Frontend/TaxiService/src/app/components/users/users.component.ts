import { UsersService } from './../../services/users.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  constructor(private usersService: UsersService) { }
  users: any;

  ngOnInit() {
    
  }

  getAll(){
    return this.usersService.getAll();
  }

  getById(){
    return this.usersService.getById(1);
  }

}
