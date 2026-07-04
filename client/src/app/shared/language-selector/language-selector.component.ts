import { Component, inject, HostListener, ElementRef } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-language-selector',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './language-selector.component.html',
  styleUrl: './language-selector.component.scss'
})
export class LanguageSelectorComponent {
  private translate = inject(TranslateService);
  private elementRef = inject(ElementRef);

  isOpen = false;
  availableLangs = ['pl', 'en', 'de'];
  currentLang = this.translate.currentLang() || localStorage.getItem('lang') || 'en';
  get iconWithLocation(): string {
      return `assets/icons/language.svg`;
    }

  toggleDropdown() {
    this.isOpen = !this.isOpen;
  }

  changeLanguage(lang: string) {
    this.translate.use(lang);
    this.currentLang = lang;
    this.isOpen = false;
    localStorage.setItem('lang', lang);
  }

  @HostListener('document:click', ['$event'])
  clickOut(event: Event) {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.isOpen = false;
    }
  }
}