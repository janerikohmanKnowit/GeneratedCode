import { Component, inject } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { ShoppingApiService } from './services/shopping-api.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'shopping-list-app';
  private api = inject(ShoppingApiService);

  reset() {
    if (!confirm('Reset all data to defaults?')) return;
  this.api.reset().subscribe({ next: () => window.location.reload() });
  }
}
