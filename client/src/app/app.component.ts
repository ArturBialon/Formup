import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { NgxSonnerToaster } from 'ngx-sonner';
import { LanguageSelectorComponent } from './shared/language-selector/language-selector.component';
import { UserProfileComponent } from './shared/user-profile/user-profile.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  standalone: true,
  imports: [RouterOutlet, NgxSonnerToaster, LanguageSelectorComponent, UserProfileComponent],
})

export class AppComponent {
  title = 'Formup';
  lastSelectedLanguage = localStorage.getItem('lang') || 'pl';

  constructor() {
    const translate = inject(TranslateService);
    translate.currentLang();
    translate.use(this.lastSelectedLanguage);
  }
}