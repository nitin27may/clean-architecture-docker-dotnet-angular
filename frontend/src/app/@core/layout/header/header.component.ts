import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { UserService } from "../../services/user.service";
import { CommonModule } from "@angular/common";
import { LoginService } from "../../../feature/user/login/login.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  standalone: true,
  imports: [
    RouterLink,
    MatMenuModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    CommonModule
  ],
  styleUrls: ['./header.component.scss'],
  providers: [LoginService]
})
export class HeaderComponent implements OnInit {
  user: any;
  constructor(private userService: UserService,
              private loginService: LoginService) {}
  logOut() {
    this.loginService.logout();
  }
  ngOnInit() {
    this.userService.getCurrentUser().subscribe((user: any) => {
      this.user = user;
    });
  }
}
