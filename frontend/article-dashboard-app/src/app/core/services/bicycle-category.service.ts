import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BicycleCategory } from '../models/bicycle-category.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class BicycleCategoryService {
  private baseUrl = environment.apiUrl + 'bicyclecategories';

  constructor(private http: HttpClient) {}

  getAll(): Observable<BicycleCategory[]> {
    return this.http.get<BicycleCategory[]>(this.baseUrl);
  }
}
