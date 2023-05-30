import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class SwalService {

  constructor() { }

  showEmailInput(): Promise<any> {
    return Swal.fire({
      title: 'Forgot Password?',
      text: 'Please enter your email:',
      input: 'email',
      showCancelButton: true,
      confirmButtonText: 'Send',
      cancelButtonText: 'Cancel',
      inputValidator: (value) => {
        if (!value) {
          return 'The Email field is required.';
        }
        return null;
      }
    });
  }

  success(title: string, content: string, footer: string): Promise<any> {
    return Swal.fire({
      icon: 'success',
      title: title,
      text: content,
      footer: footer
    });
  }

  error(title: string, content: string, footer: string): Promise<any> {
    return Swal.fire({
      icon: 'error',
      title: title,
      text: content,
      footer: footer
    });
  }

  delete(): Promise<any> {
    return Swal.fire({
      title: 'Are you sure?',
      html: '<strong>This action cannot be undone.</strong>',
      showCancelButton: true,
      confirmButtonText: 'Delete',
      cancelButtonText: 'Cancel',
    });
  }
}
