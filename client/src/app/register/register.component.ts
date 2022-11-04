import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UserAdd } from '../models/userAdd';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  newUser: UserAdd;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  register(){
    this.accountService.register(this.newUser).subscribe(res =>{
      console.log(res);
      this.cancel();
    }, err=>{
      console.error(err);
      
    });
  }

  cancel(){
    this.cancelRegister.emit(false);
  }

}
