import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.css']
})
export class NotFoundComponent implements OnInit {

  token: string = "";
  constructor(private cookieService: CookieService, private router: Router) { }

  ngOnInit(): void {
    this.token = this.cookieService.get('token');
  }

  goToLogin(router: string): void {
    this.router.navigate([router]);
  }
}
