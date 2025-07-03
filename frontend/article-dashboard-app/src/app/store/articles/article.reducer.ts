import { createReducer, on } from '@ngrx/store';
import * as ArticleActions from './article.actions';
import { Article } from '../../core/models/article.model';

export interface ArticleState {
  articles: Article[];
  totalCount: number;
  loading: boolean;
  error: any;
}

export const initialState: ArticleState = {
  articles: [],
  totalCount: 0,
  loading: false,
  error: null,
};

export const articleReducer = createReducer(
  initialState,
  on(ArticleActions.loadArticles, (state) => ({ ...state, loading: true })),
  on(ArticleActions.loadArticlesSuccess, (state, { articles, totalCount }) => ({
    ...state,
    articles,
    totalCount,
    loading: false,
  })),
  on(ArticleActions.loadArticlesFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  }))
);
