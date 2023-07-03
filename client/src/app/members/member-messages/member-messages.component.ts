import { ChangeDetectionStrategy, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit  {
  
  @Input() username?: string;
  @ViewChild('messageForm') messageForm?: NgForm;
  messageContent = '';

  constructor(public messagesService: MessageService) { }

  ngOnInit(): void {
    
  }

  sendMessage() {
    if(!this.username)
      return;
    this.messagesService.sendMessage(this.username, this.messageContent).then(() => {
      this.messageForm?.reset();
    })
  }

  
}
