import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { BicycleCategoryService } from './bicycle-category.service';
import { BicycleCategory } from '../models/bicycle-category.model';

describe('BicycleCategoryService', () => {
  let service: BicycleCategoryService;
  let httpMock: HttpTestingController;

  const apiUrl = 'https://localhost:7023/api/bicyclecategories';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [BicycleCategoryService]
    });

    service = TestBed.inject(BicycleCategoryService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch all bicycle categories', () => {
    const mockCategories: BicycleCategory[] = [
      { id: 1, name: 'Road' },
      { id: 2, name: 'Mountain' }
    ];

    service.getAll().subscribe(result => {
      expect(result.length).toBe(2);
      expect(result[0].name).toBe('Road');
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockCategories);
  });
});
