import { Routes } from '@angular/router';
import { ArticleListComponent } from './features/articles/article-list/article-list.component';
import { ArticleFormComponent } from './features/articles/article-form/article-form.component';

export const routes: Routes = [
  { path: 'articles/new', component: ArticleFormComponent },
  { path: 'articles/edit/:id', component: ArticleFormComponent },
  { path: 'articles', component: ArticleListComponent },
  { path: '', component: ArticleListComponent },
];

