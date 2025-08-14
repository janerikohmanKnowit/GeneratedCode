import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ShoppingApiService, ShoppingItem, ShoppingList } from '../services/shopping-api.service';

@Component({
  selector: 'app-list-detail',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './list-detail.component.html',
  styleUrls: ['./list-detail.component.css']
})
export class ListDetailComponent {
  private api = inject(ShoppingApiService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  list?: ShoppingList;
  loading = false;
  error = '';

  newItemText = '';
  editingId: string | null = null;
  editingText = '';

  ngOnInit() {
    this.load();
  }

  load() {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;
    this.loading = true;
    this.api.getList(id).subscribe({
      next: (l) => { this.list = l; this.loading = false; },
      error: () => { this.error = 'Failed to load list'; this.loading = false; }
    });
  }

  back() { this.router.navigate(['/lists']); }

  addItem() {
    const text = this.newItemText.trim();
    if (!text || !this.list) return;
    this.api.addItem(this.list.id, text).subscribe({ next: () => { this.newItemText = ''; this.load(); } });
  }

  startEdit(item: ShoppingItem) {
    this.editingId = item.id;
    this.editingText = item.text;
  }

  cancelEdit() { this.editingId = null; this.editingText = ''; }

  saveEdit(item: ShoppingItem) {
    if (!this.list) return;
    const text = this.editingText.trim();
    if (!text) return;
    this.api.updateItem(this.list.id, item.id, text).subscribe({ next: () => { this.cancelEdit(); this.load(); } });
  }

  deleteItem(item: ShoppingItem) {
    if (!this.list) return;
    this.api.deleteItem(this.list.id, item.id).subscribe({ next: () => this.load() });
  }
}
