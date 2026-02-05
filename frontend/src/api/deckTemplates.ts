import client from './client';
import type { DeckDto } from '../types';
import type {
  DeckTemplateSummaryDto,
  DeckTemplateDto,
  CreateDeckFromTemplateRequest,
} from '../types/deckTemplate';

/**
 * Get all available deck templates with optional filtering
 */
export const getDeckTemplates = async (
  format?: string,
  search?: string
): Promise<DeckTemplateSummaryDto[]> => {
  const params = new URLSearchParams();
  if (format) params.append('format', format);
  if (search) params.append('search', search);

  const queryString = params.toString();
  const url = queryString ? `/deck-templates?${queryString}` : '/deck-templates';

  const response = await client.get<DeckTemplateSummaryDto[]>(url);
  return response.data;
};

/**
 * Get a specific deck template by ID with all card details
 */
export const getDeckTemplate = async (id: string): Promise<DeckTemplateDto> => {
  const response = await client.get<DeckTemplateDto>(`/deck-templates/${id}`);
  return response.data;
};

/**
 * Create a new deck by copying a template
 */
export const createDeckFromTemplate = async (
  templateId: string,
  request: CreateDeckFromTemplateRequest
): Promise<DeckDto> => {
  const response = await client.post<DeckDto>(`/deck-templates/${templateId}/create-deck`, request);
  return response.data;
};
