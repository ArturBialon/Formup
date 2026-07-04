import { Component, inject, ElementRef, HostListener, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslatePipe],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.scss'
})
export class UserProfileComponent {
  public accountService = inject(AccountService);
  private elementRef = inject(ElementRef);

  isOpen = false;
  currentUser = this.accountService.userName;

  toggleDropdown() {
    this.isOpen = !this.isOpen;
  }

  logout() {
    this.isOpen = false;
    this.accountService.logout();
  }

  @HostListener('document:click', ['$event'])
  clickOut(event: Event) {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.isOpen = false;
    }
  }
}