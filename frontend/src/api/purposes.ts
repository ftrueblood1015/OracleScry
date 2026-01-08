import client from './client';
import type {
  CardPurposeDto,
  CardPurposeSummaryDto,
  CardPurposeAssignmentDto,
  PurposeExtractionJobDto,
  PurposeExtractionJobSummaryDto,
  ExtractionStatsDto,
  ExtractionStatusResponse,
  CreateCardPurposeRequest,
  UpdateCardPurposeRequest,
  PagedResult,
} from '../types';

// ============ Purpose CRUD ============

/**
 * Get all active purposes
 */
export const getAllPurposes = async (): Promise<CardPurposeSummaryDto[]> => {
  const response = await client.get<CardPurposeSummaryDto[]>('/purposes');
  return response.data;
};

/**
 * Get purposes grouped by category
 */
export const getPurposesByCategory = async (): Promise<Record<string, CardPurposeSummaryDto[]>> => {
  const response = await client.get<Record<string, CardPurposeSummaryDto[]>>('/purposes/by-category');
  return response.data;
};

/**
 * Get a purpose by ID
 */
export const getPurposeById = async (id: string): Promise<CardPurposeDto | null> => {
  try {
    const response = await client.get<CardPurposeDto>(`/purposes/${id}`);
    return response.data;
  } catch {
    return null;
  }
};

/**
 * Get a purpose by slug
 */
export const getPurposeBySlug = async (slug: string): Promise<CardPurposeDto | null> => {
  try {
    const response = await client.get<CardPurposeDto>(`/purposes/by-slug/${slug}`);
    return response.data;
  } catch {
    return null;
  }
};

/**
 * Create a new purpose
 */
export const createPurpose = async (request: CreateCardPurposeRequest): Promise<CardPurposeDto> => {
  const response = await client.post<CardPurposeDto>('/purposes', request);
  return response.data;
};

/**
 * Update an existing purpose
 */
export const updatePurpose = async (id: string, request: UpdateCardPurposeRequest): Promise<CardPurposeDto | null> => {
  try {
    const response = await client.put<CardPurposeDto>(`/purposes/${id}`, request);
    return response.data;
  } catch {
    return null;
  }
};

/**
 * Delete a purpose
 */
export const deletePurpose = async (id: string): Promise<boolean> => {
  try {
    await client.delete(`/purposes/${id}`);
    return true;
  } catch {
    return false;
  }
};

/**
 * Get purposes assigned to a specific card
 */
export const getPurposesForCard = async (cardId: string): Promise<CardPurposeAssignmentDto[]> => {
  const response = await client.get<CardPurposeAssignmentDto[]>(`/purposes/for-card/${cardId}`);
  return response.data;
};

// ============ Extraction Jobs ============

/**
 * Trigger a purpose extraction job
 */
export const triggerExtraction = async (reprocessAll = false): Promise<PurposeExtractionJobDto> => {
  const response = await client.post<PurposeExtractionJobDto>('/purposes/extraction/trigger', null, {
    params: { reprocessAll },
  });
  return response.data;
};

/**
 * Get paginated extraction job history
 */
export const getExtractionHistory = async (
  page = 1,
  pageSize = 20
): Promise<PagedResult<PurposeExtractionJobSummaryDto>> => {
  const response = await client.get<PagedResult<PurposeExtractionJobSummaryDto>>('/purposes/extraction/history', {
    params: { page, pageSize },
  });
  return response.data;
};

/**
 * Get a specific extraction job
 */
export const getExtractionJob = async (id: string): Promise<PurposeExtractionJobDto | null> => {
  try {
    const response = await client.get<PurposeExtractionJobDto>(`/purposes/extraction/${id}`);
    return response.data;
  } catch {
    return null;
  }
};

/**
 * Get the most recent extraction job
 */
export const getLatestExtraction = async (): Promise<PurposeExtractionJobSummaryDto | null> => {
  try {
    const response = await client.get<PurposeExtractionJobSummaryDto>('/purposes/extraction/latest');
    return response.data;
  } catch {
    return null;
  }
};

/**
 * Get extraction statistics
 */
export const getExtractionStats = async (): Promise<ExtractionStatsDto> => {
  const response = await client.get<ExtractionStatsDto>('/purposes/extraction/stats');
  return response.data;
};

/**
 * Check if an extraction is currently running
 */
export const getExtractionStatus = async (): Promise<ExtractionStatusResponse> => {
  const response = await client.get<ExtractionStatusResponse>('/purposes/extraction/status');
  return response.data;
};
