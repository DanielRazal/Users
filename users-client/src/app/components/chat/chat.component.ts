import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import Message from 'src/app/models/Message';
import User from 'src/app/models/User';
import { MessagesService } from 'src/app/services/messages.service';
import { SignalRService } from 'src/app/services/signal-r.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  user!: User;
  chatMessages: { user: User, content: string }[] = [];
  message = "";

  messages: Message[] = [];
  messageForm!: FormGroup;

  constructor(private signalRService: SignalRService, private cookieService: CookieService
    , private userService: UsersService, private messageService: MessagesService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.messageForm = this.formBuilder.group({
      content: [''],
    });

    this.user = JSON.parse(this.cookieService.get('user'));

    this.signalRService.addReceiveMessageListener((user, content) => {
      this.chatMessages.push({ user, content });
    });

    this.getAllMessages();
  }

  sendMessage(user: User, message: string) {
    this.signalRService.sendMessage(user, message);
    this.addMessage();
    this.message = "";
  }

  getPhotoUrl(photoUrl: string): string {
    return this.userService.getPhotoUrl(photoUrl);
  }

  addMessage() {
    const message: Message = this.messageForm.value;
    if (message.content) {
      this.messageService.addMessage(this.user.id, message).subscribe(() => {
        this.messageForm.reset();
      });
    }
  }


  getAllMessages() {
    this.messageService.getAllMessages().subscribe((messages) => {
      this.messages = messages;
      this.chatMessages = [...this.chatMessages, ...messages.map((message) => ({
        user: this.user,
        content: message.content
      }))];
    });
  }
  
}
