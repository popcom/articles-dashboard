import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick,
} from '@angular/core/testing';
import { ArticleFormComponent } from './article-form.component';
import { ArticleService } from '../../../core/services/article.service';
import { BicycleCategoryService } from '../../../core/services/bicycle-category.service';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import { Article } from '../../../core/models/article.model';
import { MaterialEnum } from '../../../core/constants/article.enums';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

describe('ArticleFormComponent in edit mode', () => {
  let component: ArticleFormComponent;
  let fixture: ComponentFixture<ArticleFormComponent>;
  let articleServiceSpy: jasmine.SpyObj<ArticleService>;
  let routerSpy: jasmine.SpyObj<Router>;

  const articleId = 'edit-id-456';

  beforeEach(async () => {
    articleServiceSpy = jasmine.createSpyObj('ArticleService', [
      'getById',
      'update',
      'create',
    ]);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    const mockArticle: Article = {
      id: articleId,
      articleNumber: 200,
      name: 'Existing Crank',
      articleCategory: 'Hub',
      material: 'Carbon',
      netWeight: 580,
      bicycleCategories: [],
      bicycleCategoryIds: [],
    };

    await TestBed.configureTestingModule({
      imports: [ArticleFormComponent],
      providers: [
        { provide: ArticleService, useValue: articleServiceSpy },
        {
          provide: BicycleCategoryService,
          useValue: { getAll: () => of([{ id: 2, name: 'Mountain' }]) },
        },
        { provide: Router, useValue: routerSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: (key: string) => (key === 'id' ? articleId : null),
              },
            },
          },
        },
      ],
    }).compileComponents();

    articleServiceSpy.getById.and.returnValue(of(mockArticle));

    fixture = TestBed.createComponent(ArticleFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should initialize form in edit mode with article data', () => {
    expect(component.isEdit).toBeTrue();
    expect(component.form.get('name')?.value).toBe('Existing Crank');
    expect(component.form.get('articleNumber')?.value).toBe(200);
    expect(component.form.get('articleCategory')?.value).toBe('Hub');
  });

  it('should submit update request on save', fakeAsync(() => {
    const updatedArticle = component.form.getRawValue();
    articleServiceSpy.update.and.returnValue(of({}));
    component.form.setValue({
      articleNumber: 200,
      name: 'Existing Crank',
      articleCategory: 'Hub',
      material: 'Carbon',
      netWeight: 580,
      bicycleCategoryIds: [1],
    });
    component.save();
    tick();

    expect(articleServiceSpy.update).toHaveBeenCalledWith(
      articleId,
      jasmine.objectContaining({
        name: updatedArticle.name,
        articleNumber: updatedArticle.articleNumber,
      })
    );
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/articles']);
  }));
});
