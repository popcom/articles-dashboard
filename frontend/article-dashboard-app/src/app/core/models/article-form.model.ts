export interface ArticleFormModel {
  articleNumber: number;
  name: string | null;
  articleCategory: string | null;
  material: string | null;
  netWeight: number | null;
  bicycleCategoryIds: number[] | null;
}
