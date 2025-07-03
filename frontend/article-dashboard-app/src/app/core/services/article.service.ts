import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Article } from '../models/article.model';
import { PagedResult } from '../models/paged-result.model';
import { ArticleFilter } from '../models/filter.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ArticleService {
  private baseUrl = environment.apiUrl + 'articles';

  constructor(private http: HttpClient) {}

  getAll(filters: ArticleFilter): Observable<PagedResult<Article>> {
    const params = new HttpParams({ fromObject: { ...filters } });
    return this.http.get<PagedResult<Article>>(this.baseUrl, { params });
  }

  getById(id: string) {
    return this.http.get<Article>(`${this.baseUrl}/${id}`);
  }

  create(article: Article) {
    return this.http.post(this.baseUrl, article);
  }

  update(id: string, article: Article) {
    return this.http.put(`${this.baseUrl}/${id}`, article);
  }
}