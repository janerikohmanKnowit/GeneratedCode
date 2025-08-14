import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ShoppingApiService, ShoppingListSummary } from '../services/shopping-api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-lists',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent {
  private api = inject(ShoppingApiService);
  private router = inject(Router);

  lists: ShoppingListSummary[] = [];
  newListName = '';
  loading = false;
  error = '';

  ngOnInit() {
    this.refresh();
  }

  refresh() {
    this.loading = true;
    this.api.getLists().subscribe({
      next: (data) => { this.lists = data; this.loading = false; },
      error: () => { this.error = 'Failed to load lists'; this.loading = false; }
    });
  }

  open(list: ShoppingListSummary) {
    this.router.navigate(['/lists', list.id]);
  }

  addList() {
    const name = this.newListName.trim();
    if (!name) return;
    this.api.createList(name).subscribe({ next: () => { this.newListName = ''; this.refresh(); } });
  }

  deleteList(id: string) {
    if (!confirm('Delete this list?')) return;
    this.api.deleteList(id).subscribe({ next: () => this.refresh() });
  }
}
