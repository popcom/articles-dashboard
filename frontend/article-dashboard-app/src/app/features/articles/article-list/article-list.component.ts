import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { FormsModule } from '@angular/forms';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { loadArticles } from '../../../store/articles/article.actions';
import {
  selectArticleList,
  selectTotalCount,
} from '../../../store/articles/article.selectors';
import {
  ArticleCategoryNames,
  ArticleCategoryMap,
  MaterialNames,
  MaterialMap,
} from '../../../core/constants/article.enums';
import { MapEnumPipe } from '../../../shared/map-enum.pipe';
import { JoinNamesPipe } from '../../../shared/join-names.pipe';
import { BicycleCategory } from '../../../core/models/bicycle-category.model';
import { BicycleCategoryService } from '../../../core/services/bicycle-category.service';
import { ArticleFilter } from '../../../core/models/filter.model';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-article-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatSortModule,
    MatCardModule,
    MatIconModule,
    MatTooltipModule,
    MapEnumPipe,
    JoinNamesPipe,
    RouterModule,
  ],
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.css'],
})
export class ArticleListComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private store = inject(Store);
  private categoryService = inject(BicycleCategoryService);

  ArticleCategoryMap = ArticleCategoryMap;
  articleCategoryNames = ArticleCategoryNames;
  articleCategoryOptions = Object.values(ArticleCategoryNames);
  MaterialMap = MaterialMap;
  materialNames = MaterialNames;
  materialOptions = Object.values(MaterialNames);

  bicycleCategoryOptions: BicycleCategory[] = [];

  articles$ = this.store.select(selectArticleList);
  total$ = this.store.select(selectTotalCount);

  filters: ArticleFilter = {
    articleCategory: '',
    bicycleCategoryIds: [],
    material: '',
    sortBy: 'netWeight',
    sortDirection: 'asc',
    page: 1,
    pageSize: 10,
  };

  displayedColumns = [
    'articleNumber',
    'name',
    'articleCategory',
    'bicycleCategories',
    'material',
    'netWeight',
    'actions',
  ];

  ngOnInit(): void {
    this.route.queryParams.pipe().subscribe(() => {
      this.loadFiltersFromUrl();
      this.loadData();
    });

    this.loadCategoryOptions();
  }

  loadFiltersFromUrl() {
    const query = this.route.snapshot.queryParamMap;
    this.filters.articleCategory = query.get('articleCategory') ?? '';
    this.filters.material = query.get('material') ?? '';
    this.filters.sortBy = query.get('sortBy') ?? 'netWeight';
    this.filters.sortDirection = query.get('sortDirection') ?? 'asc';
    this.filters.page = +(query.get('page') ?? 1);
    this.filters.pageSize = +(query.get('pageSize') ?? 10);
    this.filters.bicycleCategoryIds = query
      .getAll('bicycleCategoryIds')
      .map(Number);
  }

  updateUrlParams() {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        articleCategory: this.filters.articleCategory || null,
        material: this.filters.material || null,
        sortBy: this.filters.sortBy || null,
        sortDirection: this.filters.sortDirection || null,
        page: this.filters.page,
        pageSize: this.filters.pageSize,
        bicycleCategoryIds: this.filters.bicycleCategoryIds ?? [],
      },
      queryParamsHandling: 'merge',
    });
  }

  loadData(resetPage: boolean = false) {
    if (resetPage) {
      this.filters.page = 1; // reset to first page
    }
    this.updateUrlParams();
    this.store.dispatch(loadArticles({ filters: { ...this.filters } }));
  }

  loadCategoryOptions() {
    this.categoryService.getAll().subscribe({
      next: (categories) => (this.bicycleCategoryOptions = categories),
      error: (err) => console.error('Failed to load bicycle categories', err),
    });
  }

  onPageChange(e: PageEvent) {
    this.filters.page = e.pageIndex + 1;
    this.filters.pageSize = e.pageSize;
    this.loadData();
  }

  onSortChange(event: Sort) {
    this.filters.sortBy = event.active; // e.g., "netWeight"
    this.filters.sortDirection = event.direction; // e.g., "asc"
    this.loadData(true); // reset to page 1
  }

  resetFilters() {
    this.filters = {
      articleCategory: '',
      bicycleCategoryIds: [],
      material: '',
      sortBy: 'netWeight',
      sortDirection: 'asc',
      page: 1,
      pageSize: 10,
    };
    this.loadData();
  }
}
