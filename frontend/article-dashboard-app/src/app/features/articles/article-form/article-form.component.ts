import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { BicycleCategoryService } from '../../../core/services/bicycle-category.service';
import { Article } from '../../../core/models/article.model';
import { BicycleCategory } from '../../../core/models/bicycle-category.model';
import { ArticleService } from '../../../core/services/article.service';
import {
  ArticleCategoryEnum,
  ArticleCategoryNames,
  MaterialEnum,
} from '../../../core/constants/article.enums';

@Component({
  selector: 'app-article-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCardModule,
  ],
  templateUrl: './article-form.component.html',
  styleUrls: ['./article-form.component.css'],
})
export class ArticleFormComponent implements OnInit {
  private fb = inject(NonNullableFormBuilder);
  private service = inject(ArticleService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private categoryService = inject(BicycleCategoryService);

  form = this.fb.group({
    articleNumber: this.fb.control(0, Validators.required),
    name: this.fb.control('', Validators.required),
    articleCategory: this.fb.control('', Validators.required),
    material: this.fb.control('', Validators.required),
    netWeight: this.fb.control(0, [Validators.required, Validators.min(0.1)]),
    bicycleCategoryIds: this.fb.control([] as number[], [Validators.required]),
  });

  isEdit = false;
  articleId: string | null = null;

  articleCategories = Object.values(ArticleCategoryNames);
  bicycleCategoryOptions: BicycleCategory[] = [];
  articleCategoryOptions = Object.entries(ArticleCategoryEnum)
    .filter(([k, v]) => typeof v === 'number') // filter enum keys
    .map(([k, v]) => ({ label: k, value: v as number }));
  materialOptions = Object.entries(MaterialEnum)
    .filter(([k, v]) => typeof v === 'number') // filter enum keys
    .map(([k, v]) => ({ label: k, value: v as number }));

  ngOnInit(): void {
    this.articleId = this.route.snapshot.paramMap.get('id');
    this.isEdit = !!this.articleId;

    this.categoryService
      .getAll()
      .subscribe((cats) => (this.bicycleCategoryOptions = cats));

    if (this.isEdit) {
      this.service.getById(this.articleId!).subscribe((article) => {
        this.form.patchValue({
          articleNumber: article.articleNumber,
          name: article.name,
          articleCategory: article.articleCategory,
          material: article.material,
          netWeight: article.netWeight,
          bicycleCategoryIds: article.bicycleCategories.map((c) =>
            Number.parseInt(c.id)
          ),
        });
      });
    }
  }

  save() {
    if (this.form.invalid) return;
    const data = this.form.getRawValue();

    const payload: Article = {
      id: this.articleId ?? '',
      articleNumber: data.articleNumber,
      name: data.name,
      articleCategory: data.articleCategory,
      material: data.material,
      netWeight: data.netWeight,
      bicycleCategoryIds: data.bicycleCategoryIds,
      bicycleCategories: [],
    };

    const request$ = this.isEdit
      ? this.service.update(this.articleId!, payload)
      : this.service.create(payload);

    request$.subscribe({
      next: () => this.router.navigate(['/articles']),
      error: (err) => alert('Save failed: ' + err.message),
    });
  }

  cancel() {
    this.router.navigate(['/articles']);
  }
}
