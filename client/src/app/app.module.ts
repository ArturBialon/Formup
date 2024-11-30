import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from './home/home.component';
import { SharedModule } from './_modules/shared.module';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { InputMaskModule } from '@ngneat/input-mask';
import { InputComponent } from './shared/input/input.component';
import { AccountService } from './_services/account.service';
import { API_BASE_URL, LoginService } from './_services/autogenerated/autogenerated.service';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { environment } from 'src/environments/environment';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    TestErrorsComponent,
    NotFoundComponent,
    ServerErrorComponent,
    InputComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    SharedModule,
    FontAwesomeModule,
    InputMaskModule,
    ReactiveFormsModule,
    InputMaskModule.forRoot({ inputSelector: 'input', isAsync: true })
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor, multi: true
    },
    {
      provide: AccountService,
      useClass: LoginService
    },
    {
      provide: API_BASE_URL,
      useValue: environment.baseApiUrl
    },
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
