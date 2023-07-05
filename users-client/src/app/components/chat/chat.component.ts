import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { switchMap } from 'rxjs';
import Message from 'src/app/models/Message';
import User from 'src/app/models/User';
import { MessagesService } from 'src/app/services/messages.service';
import { SignalRService } from 'src/app/services/signal-r.service';
import { SwalService } from 'src/app/services/swal.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  user!: User;
  chatMessages: { user: User, content: string, messageId: number }[] = [];

  message = '';
  users: User[] = [];
  messageForm!: FormGroup;

  constructor(private signalRService: SignalRService, private cookieService: CookieService
    , private userService: UsersService, private messageService: MessagesService,
    private formBuilder: FormBuilder, private swalService: SwalService) { }

  ngOnInit(): void {
    this.messageForm = this.formBuilder.group({
      content: [''],
    });

    this.user = JSON.parse(this.cookieService.get('user'));

    this.signalRService.addReceiveMessageListener((user, content) => {
      this.chatMessages.push({ user, content, messageId: 1 });
    });

    this.messageForm.get('content')!.valueChanges.subscribe(value => {
      this.message = value;
    });

    this.getAllUsers();

  }

  sendMessage(user: User, message: string) {
    if (message && message.trim() !== '') {
      this.signalRService.sendMessage(user, message);
      this.addMessage();
      this.message = '';
    } 
    else {
      this.swalService.error('Error', 'Message is empty. Please enter a message.', 'Status code: 400');
    }
  }


  getPhotoUrl(photoUrl: string): string {
    return this.userService.getPhotoUrl(photoUrl);
  }

  addMessage() {
    const message: Message = this.messageForm.value;
    if (message.content) {
      this.messageService.addMessage(this.user.id, message).pipe(
        switchMap((res) => {
          return this.userService.getUserById(res.userId);
        })
      ).subscribe((res) => {
        this.cookieService.delete('user');
        this.cookieService.set('user', JSON.stringify(res));
        this.messageForm.reset();
      })
    }
  }
  getAllUsers() {
    this.userService.getAllUsers().subscribe((users) => {
      this.users = users;
      this.chatMessages = [];

      users.forEach((user) => {
        user.messages.forEach((message) => {
          this.chatMessages.push({
            user: user,
            content: message.content,
            messageId: message.id
          });
        });
      });

      this.chatMessages.sort((a, b) => a.messageId - b.messageId);
    });
  }
}
