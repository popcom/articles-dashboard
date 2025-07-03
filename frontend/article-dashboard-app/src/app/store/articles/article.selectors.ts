import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ArticleState } from './article.reducer';

export const selectArticleState =
  createFeatureSelector<ArticleState>('articles');
export const selectArticleList = createSelector(
  selectArticleState,
  (state) => state.articles
);
export const selectTotalCount = createSelector(
  selectArticleState,
  (state) => state.totalCount
);
