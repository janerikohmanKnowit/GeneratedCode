import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ShoppingListSummary { id: string; name: string; itemCount: number; }
export interface ShoppingItem { id: string; text: string; }
export interface ShoppingList { id: string; name: string; items: ShoppingItem[]; }

@Injectable({ providedIn: 'root' })
export class ShoppingApiService {
  private http = inject(HttpClient);
  private base = '/api';

  getLists(): Observable<ShoppingListSummary[]> {
    return this.http.get<ShoppingListSummary[]>(`${this.base}/lists`);
  }
  getList(id: string): Observable<ShoppingList> {
    return this.http.get<ShoppingList>(`${this.base}/lists/${id}`);
  }
  createList(name: string): Observable<ShoppingList> {
    return this.http.post<ShoppingList>(`${this.base}/lists`, { name });
  }
  deleteList(id: string): Observable<void> {
    return this.http.delete<void>(`${this.base}/lists/${id}`);
  }
  addItem(listId: string, text: string): Observable<ShoppingItem> {
    return this.http.post<ShoppingItem>(`${this.base}/lists/${listId}/items`, { text });
  }
  updateItem(listId: string, itemId: string, text: string): Observable<void> {
    return this.http.put<void>(`${this.base}/lists/${listId}/items/${itemId}`, { text });
  }
  deleteItem(listId: string, itemId: string): Observable<void> {
    return this.http.delete<void>(`${this.base}/lists/${listId}/items/${itemId}`);
  }
  reset(): Observable<void> {
    return this.http.post<void>(`${this.base}/reset`, {});
  }
}
