import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-admin-panel-component',
  standalone: true,
  templateUrl: './admin-panel-component.component.html',
  styleUrls: ['./admin-panel-component.component.css']
})
export class AdminPanelComponentComponent {
  @Input() request: any; // Input property to accept request details
  @Output() startChatEvent = new EventEmitter<any>(); // Event emitter to notify the parent component

  constructor() { }

  // Method to emit the startChatEvent when the button is clicked
  startChat() {
    this.startChatEvent.emit(this.request);
  }
}
