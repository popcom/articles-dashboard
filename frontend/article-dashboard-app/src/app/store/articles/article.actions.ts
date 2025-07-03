import { createAction, props } from '@ngrx/store';
import { Article } from '../../core/models/article.model';
import { ArticleFilter } from '../../core/models/filter.model';

export const loadArticles = createAction(
  '[Article] Load Articles',
  props<{ filters: ArticleFilter }>()
);
export const loadArticlesSuccess = createAction(
  '[Article] Load Articles Success',
  props<{ articles: Article[]; totalCount: number }>()
);
export const loadArticlesFailure = createAction(
  '[Article] Load Articles Failure',
  props<{ error: any }>()
);
