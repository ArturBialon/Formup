import { Component, input } from '@angular/core';

@Component({
  selector: 'app-work-case-details',
  standalone: true,
  imports: [],
  templateUrl: './work-case-details.component.html',
  styleUrl: './work-case-details.component.scss',
})
export class WorkCaseDetailsComponent {
  id = input<string>();
}
