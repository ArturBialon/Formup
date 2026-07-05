import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TileComponent } from '../tile/tile.component';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterOutlet, TileComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  public accountService = inject(AccountService);
}