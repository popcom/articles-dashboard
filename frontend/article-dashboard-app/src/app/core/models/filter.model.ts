export interface ArticleFilter {
  articleCategory?: string;
  bicycleCategoryIds: number[];
  material?: string;
  sortBy?: string;
  sortDirection?: string;
  page: number;
  pageSize: number;
}