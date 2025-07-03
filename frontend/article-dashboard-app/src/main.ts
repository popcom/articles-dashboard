import { provideStore, provideState } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideHttpClient } from '@angular/common/http';
import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';

import { AppComponent } from './app/app.component';
import { routes } from './app/app.routes';
import { articleReducer } from './app/store/articles/article.reducer';
import { ArticleEffects } from './app/store/articles/article.effects';

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(),
    provideRouter(routes),
    provideStore(),
    provideState('articles', articleReducer),
    provideEffects([ArticleEffects])
  ]
});
