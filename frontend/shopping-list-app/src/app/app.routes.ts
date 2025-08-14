import { Routes } from '@angular/router';
import { ListsComponent } from './lists/lists.component';
import { ListDetailComponent } from './list-detail/list-detail.component';

export const routes: Routes = [
	{ path: '', pathMatch: 'full', redirectTo: 'lists' },
	{ path: 'lists', component: ListsComponent },
	{ path: 'lists/:id', component: ListDetailComponent },
	{ path: '**', redirectTo: 'lists' }
];
