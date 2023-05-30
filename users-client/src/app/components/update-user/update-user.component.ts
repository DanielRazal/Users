import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import UpdateUser from 'src/app/models/UpdateUser';
import User from 'src/app/models/User';
import { SwalService } from 'src/app/services/swal.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-update-user',
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.css']
})
export class UpdateUserComponent implements OnInit {


  updateForm!: FormGroup;
  selectedPhoto: File | undefined;
  user!: User;

  constructor(private formBuilder: FormBuilder, private userService: UsersService
    , private router: Router, private swalService: SwalService, private cookieService: CookieService) { }

  ngOnInit(): void {
    this.updateForm = this.formBuilder.group({
      password: [''],
      firstName: [''],
      lastName: [''],
      photo: ['']
    });

    this.user = JSON.parse(this.cookieService.get('user'));
  }

  
  onPhotoSelected(event: any) {
    if (event.target.files && event.target.files.length > 0) {
      this.selectedPhoto = event.target.files[0];
    }
  }


  onSubmit() {
    const updateUser = this.updateForm.value as UpdateUser;

    const formData = new FormData();
    formData.append('UpdateUser.FirstName', updateUser.firstName);
    formData.append('UpdateUser.LastName', updateUser.lastName);
    formData.append('UpdateUser.Password', updateUser.password);
    formData.append('Photo', this.selectedPhoto!);

    this.userService.updateUser(this.user.id, formData).subscribe((res) => {
      this.swalService.success(updateUser.firstName + ' ' + updateUser.lastName, res.message,
        `Status code: ${res.statusCode}`);
      this.router.navigate(['home']).then(() => {
        this.cookieService.delete('user');
        const updatedUser = res.user;
        this.cookieService.set('user', JSON.stringify(updatedUser));
      });
    
      this.updateForm.reset();
    }, (error) => {
      if (error.error && error.error.errors) {
        const errorKeys = Object.keys(error.error.errors);
        errorKeys.forEach((key) => {
          const errorMessage = error.error.errors[key][0];
          this.swalService.error('Error', errorMessage, `Status code: ${error.status}`);
        });
      }
    });
  }
}
