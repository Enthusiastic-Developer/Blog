import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  status!: boolean;
  constructor(public accountService1: AccountService) {}
  getstatus() {
    this.status = this.accountService1.isLoggedIn();
  }

  ngOnInit(): void {
    this.getstatus();
  }
}