import { ApplicationConfig } from '@angular/core';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideState } from '@ngrx/store';
import { articleReducer } from './store/articles/article.reducer';
import { ArticleEffects } from './store/articles/article.effects';

export const appConfig: ApplicationConfig = {
  providers: [
    provideStore(),
    provideState('articles', articleReducer),
    provideEffects([ArticleEffects]),
  ]
};
