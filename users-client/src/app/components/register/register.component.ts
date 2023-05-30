import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import UserDTO from 'src/app/models/UserDTO';
import { SwalService } from 'src/app/services/swal.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  registerForm!: FormGroup;
  selectedPhoto: File | undefined;

  constructor(private formBuilder: FormBuilder, private userService: UsersService
    , private router: Router, private swalService: SwalService) { }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      userName: [''],
      password: [''],
      firstName: [''],
      lastName: [''],
      email: [''],
      photo: [''],
      acceptedTerms: [false, Validators.requiredTrue]
    });
  }

  onPhotoSelected(event: any) {
    if (event.target.files && event.target.files.length > 0) {
      this.selectedPhoto = event.target.files[0];
    }
  }

  onSubmit() {
    const userDTO = this.registerForm.value as UserDTO;

    const formData = new FormData();
    formData.append('UserDTO.FirstName', userDTO.firstName);
    formData.append('UserDTO.LastName', userDTO.lastName);
    formData.append('UserDTO.UserName', userDTO.userName);
    formData.append('UserDTO.Password', userDTO.password);
    formData.append('UserDTO.Email', userDTO.email);
    formData.append('UserDTO.AcceptedTerms', userDTO.acceptedTerms.toString());
    formData.append('Photo', this.selectedPhoto!);
  
    this.userService.register(formData).subscribe((res) => {
      this.swalService.success(userDTO.firstName + ' ' + userDTO.lastName, res.message,
        `Status code: ${res.statusCode}`);
      this.router.navigate(['login']);
      this.registerForm.reset();
    }, (error) => {
      if (error.status === 401) {
        this.swalService.error('Error', error.error, `Status code: ${error.status}`);
      } else if (error.error && error.error.errors) {
        const errorKeys = Object.keys(error.error.errors);
  
        errorKeys.forEach((key) => {
          const errorMessage = error.error.errors[key][0];
          this.swalService.error('Error', errorMessage, `Status code: ${error.status}`);
        });
      }
    });
  }
  

}
