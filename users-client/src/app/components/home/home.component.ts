import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { UserRole } from 'src/app/enum/userRole';
import User from 'src/app/models/User';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  user!: User;
  users: User[] = [];
  UserRole = UserRole;
  userRoles = [
    { label: 'Main Admin', value: UserRole.MAIN_ADMIN },
    { label: 'Admin', value: UserRole.ADMIN },
    { label: 'User', value: UserRole.USER }
  ];

  constructor(private cookieService: CookieService, private userService: UsersService) { }

  ngOnInit(): void {
    this.user = JSON.parse(this.cookieService.get('user'));
    this.cookieService.get('token');
    this.getAllUsers();
  }

  getAllUsers() {
    this.userService.getAllUsers().subscribe((users) => {
      this.users = users.filter(user => user.role !== UserRole.MAIN_ADMIN);
    });
  }

  getPhotoUrl(photoUrl: string): string {
    return this.userService.getPhotoUrl(photoUrl);
  }

  ChangeRole(user: User) {
    this.userService.assignUserRole(user.id, user.role).subscribe(
      () => {
        alert('Role successfully updated');
      },
      error => {
        console.log('Error updating role:', error);
        alert('Error updating role. Please try again.');
      }
    );
  }

  getUserRoleString(role: UserRole): string {
    return this.userService.mapUserRole(role);
  }
}
