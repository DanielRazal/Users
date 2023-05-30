import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import User from 'src/app/models/User';
import { SwalService } from 'src/app/services/swal.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-insite-header',
  templateUrl: './insite-header.component.html',
  styleUrls: ['./insite-header.component.css']
})
export class InsiteHeaderComponent implements OnInit {

  user!: User;
  
  constructor(private userService: UsersService, private router: Router,
    private cookieService: CookieService, private swalService: SwalService) { }

  ngOnInit(): void {
    this.user = JSON.parse(this.cookieService.get('user'));
  }

  getPhotoUrl(photoUrl: string): string {
    return this.userService.getPhotoUrl(photoUrl);
  }


  LogOut() {
    this.router.navigate(['login']);
    this.cookieService.deleteAll();
  }

  deleteUser() {
    this.swalService.delete().then((result) => {
      if (result.isConfirmed) {
        if (this.user) {
          this.userService.deleteUser(this.user.id).subscribe(() => {
            this.router.navigate(['login']);
            this.cookieService.deleteAll();
          });
        }
      }
    })
  }
}
