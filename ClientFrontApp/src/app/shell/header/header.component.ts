import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/authentication/auth.service';
import { Roles } from 'src/app/shared/roles.enum';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  userName: string;
  isAuthenticated: boolean;
  isAdmin: boolean;
  isManager: boolean;
  isUser: boolean;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.userName = this.authService.name;
    this.isAdmin = this.authService.isInRole(Roles.Admin);
    this.isManager = this.authService.isInRole(Roles.Manager);
    this.isUser = this.authService.isInRole(Roles.User);
  }

  signout() {
    this.authService.signout();
  }
}
