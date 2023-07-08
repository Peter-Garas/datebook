import { Component, Input, OnInit } from '@angular/core';
import { Observable, of, take } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  loggedin = false;
  user: User | undefined;
  currentuser$: Observable<User | null> = of(null);

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if(user) {
          this.loggedin = true;
        }
      }
    })
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }  

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
}
