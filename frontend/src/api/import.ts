import client from './client';
import type {
  CardImportDto,
  CardImportSummaryDto,
  ImportStatsDto,
  ImportStatusResponse,
  PagedResult,
} from '../types';

/**
 * Get paginated import history
 */
export const getImportHistory = async (
  page = 1,
  pageSize = 20
): Promise<PagedResult<CardImportSummaryDto>> => {
  const response = await client.get<PagedResult<CardImportSummaryDto>>('/import/history', {
    params: { page, pageSize },
  });
  return response.data;
};

/**
 * Get single import details with errors
 */
export const getImportById = async (id: string): Promise<CardImportDto | null> => {
  try {
    const response = await client.get<CardImportDto>(`/import/${id}`);
    return response.data;
  } catch {
    return null;
  }
};

/**
 * Get the most recent import
 */
export const getLatestImport = async (): Promise<CardImportSummaryDto | null> => {
  try {
    const response = await client.get<CardImportSummaryDto>('/import/latest');
    return response.data;
  } catch {
    return null;
  }
};

/**
 * Get aggregated import statistics
 */
export const getImportStats = async (): Promise<ImportStatsDto> => {
  const response = await client.get<ImportStatsDto>('/import/stats');
  return response.data;
};

/**
 * Check if an import is currently running
 */
export const getImportStatus = async (): Promise<ImportStatusResponse> => {
  const response = await client.get<ImportStatusResponse>('/import/status');
  return response.data;
};
