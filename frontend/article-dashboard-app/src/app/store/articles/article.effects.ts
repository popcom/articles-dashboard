import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as ArticleActions from './article.actions';
import { catchError, map, mergeMap, of } from 'rxjs';
import { ArticleService } from '../../core/services/article.service';

@Injectable()
export class ArticleEffects {
  private actions$ = inject(Actions);
  private service = inject(ArticleService);

  loadArticles$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ArticleActions.loadArticles),
      mergeMap(({ filters }) =>
        this.service.getAll(filters).pipe(
          map((res) =>
            ArticleActions.loadArticlesSuccess({
              articles: res.items,
              totalCount: res.totalCount,
            })
          ),
          catchError((error) =>
            of(ArticleActions.loadArticlesFailure({ error }))
          )
        )
      )
    )
  );
}
