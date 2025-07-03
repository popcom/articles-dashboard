import { TestBed } from '@angular/core/testing';
import { ArticleService } from './article.service';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { Article } from '../models/article.model';
import { ArticleFilter } from '../models/filter.model';
import { PagedResult } from '../models/paged-result.model';

describe('ArticleService', () => {
  let service: ArticleService;
  let httpMock: HttpTestingController;

  const apiUrl = 'https://localhost:7023/api/articles'; // Mocked backend base URL

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ArticleService],
    });

    service = TestBed.inject(ArticleService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); // Ensure no unmatched requests
  });

  it('should fetch all articles with filters', () => {
    const filters: ArticleFilter = {
      articleCategory: '',
      material: '',
      sortBy: 'netWeight',
      page: 1,
      pageSize: 10,
      bicycleCategoryIds: [1, 2],
    };

    const mockResponse: PagedResult<Article> = {
      items: [
        {
          id: 'a1',
          articleNumber: 101,
          name: 'Carbon Crank',
          articleCategory: 'Hub',
          material: 'Carbon',
          netWeight: 580,
          bicycleCategories: [],
          bicycleCategoryIds: [],
        },
      ],
      page: 1,
      pageSize: 10,
      totalCount: 1,
    };

    service.getAll(filters).subscribe((result) => {
      expect(result.totalCount).toBe(1);
      expect(result.items[0].name).toBe('Carbon Crank');
    });

    const req = httpMock.expectOne(
      (request) => request.url === apiUrl && request.params.get('page') === '1'
    );

    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should fetch article by ID', () => {
    const mockArticle: Article = {
      id: 'a1',
      articleNumber: 101,
      name: 'Hub Pro',
      articleCategory: 'Hub',
      material: 'Carbon',
      netWeight: 580,
      bicycleCategories: [],
      bicycleCategoryIds: [],
    };

    service.getById('a1').subscribe((result) => {
      expect(result.id).toBe('a1');
      expect(result.name).toBe('Hub Pro');
    });

    const req = httpMock.expectOne(`${apiUrl}/a1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockArticle);
  });

  it('should create a new article', () => {
    const newArticle: Article = {
      id: '',
      articleNumber: 0,
      name: 'Steel Rim',
      articleCategory: 'Hub',
      material: 'Carbon',
      netWeight: 580,
      bicycleCategories: [],
      bicycleCategoryIds: [],
    };

    service.create(newArticle).subscribe((response) => {
      expect(response).toBeDefined();
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body.name).toBe('Steel Rim');
    req.flush('generated-id-xyz');
  });

  it('should update an existing article', () => {
    const updatedArticle: Article = {
      id: 'a1',
      articleNumber: 101,
      name: 'Updated Name',
      articleCategory: 'Hub',
      material: 'Carbon',
      netWeight: 580,
      bicycleCategories: [],
      bicycleCategoryIds: [],
    };

    service.update('a1', updatedArticle).subscribe((response) => {
      expect(response).toBeNull(); // PUT returns void
    });

    const req = httpMock.expectOne(`${apiUrl}/a1`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body.name).toBe('Updated Name');
    req.flush(null);
  });
});
