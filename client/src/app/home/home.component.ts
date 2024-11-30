import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.formWrapper();
  }

  onSubmit(): void {}

  formWrapper() {
    this.loginForm = this.fb.group({
      password: [null, Validators.compose([Validators.required])],
      email: [
        null,
        Validators.compose([
          Validators.required,
          Validators.pattern(/([\w\.\-]+)@([\w\-]+)((\.(\w){2,9})+)$/),
        ]),
      ],
    });
  }
}
