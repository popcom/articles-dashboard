import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ArticleListComponent } from './article-list.component';
import { provideMockStore } from '@ngrx/store/testing';
import { of } from 'rxjs';
import { BicycleCategoryService } from '../../../core/services/bicycle-category.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import * as fromArticles from '../../../store/articles/article.selectors';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTableHarness } from '@angular/material/table/testing';
import { MatSelectHarness } from '@angular/material/select/testing';
import { MatButtonHarness } from '@angular/material/button/testing';
import { HarnessLoader } from '@angular/cdk/testing';
import { TestbedHarnessEnvironment } from '@angular/cdk/testing/testbed';
import { convertToParamMap } from '@angular/router';

describe('ArticleListComponent', () => {
  let component: ArticleListComponent;
  let fixture: ComponentFixture<ArticleListComponent>;
  let routerSpy: jasmine.SpyObj<Router>;
  let categoryServiceSpy: jasmine.SpyObj<BicycleCategoryService>;

  const mockActivatedRoute = {
    snapshot: {
      queryParamMap: {
        get: (key: string) => null,
        getAll: (key: string) => [],
      },
    },
    queryParams: of({}),
  };

  beforeEach(async () => {
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    categoryServiceSpy = jasmine.createSpyObj('BicycleCategoryService', [
      'getAll',
    ]);
    categoryServiceSpy.getAll.and.returnValue(of([]));

    await TestBed.configureTestingModule({
      imports: [ArticleListComponent],
      providers: [
        provideMockStore({
          selectors: [
            { selector: fromArticles.selectArticleList, value: [] },
            { selector: fromArticles.selectTotalCount, value: 0 },
          ],
        }),
        { provide: BicycleCategoryService, useValue: categoryServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: ActivatedRoute, useValue: mockActivatedRoute },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ArticleListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should reset filters and call loadData()', () => {
    spyOn(component, 'loadData');
    component.filters.articleCategory = 'Hub';
    component.resetFilters();
    expect(component.filters.articleCategory).toBe('');
    expect(component.loadData).toHaveBeenCalled();
  });

  it('should update page and call loadData() on page change', () => {
    spyOn(component, 'loadData');
    const event: PageEvent = { pageIndex: 2, pageSize: 20, length: 100 };
    component.onPageChange(event);
    expect(component.filters.page).toBe(3); // because it's zero-indexed
    expect(component.filters.pageSize).toBe(20);
    expect(component.loadData).toHaveBeenCalled();
  });

  it('should update sorting and call loadData()', () => {
    spyOn(component, 'loadData');
    const sort: Sort = { active: 'netWeight', direction: 'desc' };
    component.onSortChange(sort);
    expect(component.filters.sortBy).toBe('netWeight');
    expect(component.filters.sortDirection).toBe('desc');
    expect(component.loadData).toHaveBeenCalledWith(true);
  });

  it('should call loadFiltersFromUrl() and loadData() on init', () => {
    const spy1 = spyOn(component as any, 'loadFiltersFromUrl');
    const spy2 = spyOn(component, 'loadData');
    component.ngOnInit();
    expect(spy1).toHaveBeenCalled();
    expect(spy2).toHaveBeenCalled();
  });
});

describe('ArticleListComponent (HTML Harness)', () => {
  let fixture: ComponentFixture<ArticleListComponent>;
  let loader: HarnessLoader;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArticleListComponent, BrowserAnimationsModule],
      providers: [
        provideMockStore({
          selectors: [
            { selector: fromArticles.selectArticleList, value: [
              {
                id: '1',
                articleNumber: 100,
                name: 'Carbon Crank',
                articleCategory: 1,
                material: 1,
                netWeight: 580,
                bicycleCategories: [{ id: 1, name: 'Road' }]
              }
            ]},
            { selector: fromArticles.selectTotalCount, value: 1 }
          ]
        }),
        { provide: BicycleCategoryService, useValue: { getAll: () => of([]) } },
        { provide: Router, useValue: { navigate: jasmine.createSpy('navigate') } },
        { provide: ActivatedRoute, useValue: { snapshot: { queryParamMap: convertToParamMap({}) }, queryParams: of({}) } },
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ArticleListComponent);
    loader = TestbedHarnessEnvironment.loader(fixture);
    fixture.detectChanges();
  });

  it('should render one row in the table', async () => {
    const table = await loader.getHarness(MatTableHarness);
    const rows = await table.getRows();
    expect(rows.length).toBe(1);

    const cells = await rows[0].getCellTextByIndex();
    expect(cells).toContain('Carbon Crank');
  });

  it('should display Add New Article button', async () => {
    const buttons = await loader.getAllHarnesses(MatButtonHarness.with({ text: /Add New Article/i }));
    expect(buttons.length).toBe(1);
  });

  it('should have working filters', async () => {
    const selects = await loader.getAllHarnesses(MatSelectHarness);
    expect(selects.length).toBeGreaterThan(0);
  });
});
