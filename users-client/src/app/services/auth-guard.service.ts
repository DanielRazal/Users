import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { SwalService } from './swal.service';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private router: Router, private swalService: SwalService,
    private cookieService: CookieService) { }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    var token = this.cookieService.get('token');
    if (!token) {
      this.swalService.error("Error to Navigate", "You need to login on the login screen to navigate the site",
      "Status code: 400")
      this.router.navigate(['/login']);
    }
    return true;
  }
}
