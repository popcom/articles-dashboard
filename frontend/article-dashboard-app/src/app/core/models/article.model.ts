export interface Article {
  id: string;
  articleNumber: number;
  name: string;
  articleCategory: string;
  material: string;
  netWeight: number;
  bicycleCategoryIds: number[];
  bicycleCategories: Article[];
}