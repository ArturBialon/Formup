import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { NgxSonnerToaster } from 'ngx-sonner';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  standalone: true,
  imports: [RouterOutlet, NgxSonnerToaster]
})

export class AppComponent {
  title = 'Formup';

  constructor() {
    const translate = inject(TranslateService);
    translate.currentLang(); // Ustawia fallback
    translate.use('de');            // Włącza plik pl.json
  }
}