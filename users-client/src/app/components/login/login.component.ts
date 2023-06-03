import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { SwalService } from 'src/app/services/swal.service';
import { UsersService } from 'src/app/services/users.service';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;

  constructor(private formBuilder: FormBuilder, private userService: UsersService
    , private swalService: SwalService, private router: Router,
    private cookieService: CookieService) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      userName: [''],
      password: ['']
    });
  }

  
  onSubmit() {
    this.userService.login(this.loginForm.value)
      .subscribe((res) => {
        this.cookieService.set('token', res.token);
        this.cookieService.set('user', JSON.stringify(res._user));
        console.log(res._user);
        
        this.router.navigate(['home']);
        this.loginForm.reset();
        this.swalService.success('Success', res.message, `Status code: ${res.statusCode}`);
      }, (error) => {
        if (error.status === 400) {
          const errorKeys = Object.keys(error.error.errors);
          errorKeys.forEach((key) => {
            const errorMessage = error.error.errors[key][0];
            this.swalService.error('Error', errorMessage, `Status code: ${error.status}`);
          });
        }
        else{
          this.swalService.error('Error', error.error, `Status code: ${error.status}`);
        }
      });
  }


  sendDetailsToEmail() {
    this.swalService.showEmailInput().then((result) => {
      if (result.isConfirmed && result.value) {
        const email = result.value;
        this.userService.sendDetailsToEmail(email).subscribe((response) => {
          this.swalService.success('Success', response.message, `Status code: ${response.statusCode}`);
        }, (error) => {
          const errorKeys = Object.keys(error.error.errors);
          errorKeys.forEach((key) => {
            const errorMessage = error.error.errors[key][0];
            this.swalService.error('Error', errorMessage, `Status code: ${error.status}`);
          });
        });
      }
    });
  }  
}