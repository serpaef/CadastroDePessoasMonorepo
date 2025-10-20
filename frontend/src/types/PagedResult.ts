export interface PagedResult<T> {
  currentPage: number;
  totalPages: number;
  totalItems: number;
  items: T[];
} 