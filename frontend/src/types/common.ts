/**
 * Generic paged result for pagination
 */
export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

/**
 * API error response
 */
export interface ApiError {
  error: string;
  statusCode: number;
}
