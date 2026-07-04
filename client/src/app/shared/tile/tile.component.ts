import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-tile',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslatePipe],
  templateUrl: './tile.component.html',
  styleUrl: './tile.component.scss'
})
export class TileComponent {
  @Input({ required: true }) labelKey!: string; 
  @Input({ required: true }) iconName!: string; 
  @Input({ required: true }) routePath?: string; 
  
  get iconWithLocation(): string {
    return `assets/icons/${this.iconName}`;
  }
}