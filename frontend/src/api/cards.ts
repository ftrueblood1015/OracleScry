import client from './client';
import type { CardDto, CardFilterDto, CardSummaryDto, PagedResult } from '../types';

/**
 * Search and filter cards with pagination
 */
export const searchCards = async (filter: CardFilterDto): Promise<PagedResult<CardSummaryDto>> => {
  const params = new URLSearchParams();

  // Backend expects 'Query' for text search
  if (filter.search) params.append('Query', filter.search);
  if (filter.name) params.append('Query', filter.name);
  // Backend expects 'SetCode' for set filter
  if (filter.set) params.append('SetCode', filter.set);
  if (filter.rarity) params.append('rarity', filter.rarity);
  if (filter.colors?.length) filter.colors.forEach((c) => params.append('colors', c));
  if (filter.colorIdentity?.length)
    filter.colorIdentity.forEach((c) => params.append('colorIdentity', c));
  if (filter.types?.length) filter.types.forEach((t) => params.append('types', t));
  if (filter.minCmc !== undefined) params.append('minCmc', filter.minCmc.toString());
  if (filter.maxCmc !== undefined) params.append('maxCmc', filter.maxCmc.toString());
  if (filter.format) params.append('format', filter.format);
  if (filter.page) params.append('page', filter.page.toString());
  if (filter.pageSize) params.append('pageSize', filter.pageSize.toString());
  if (filter.sortBy) params.append('sortBy', filter.sortBy);
  if (filter.sortDescending !== undefined)
    params.append('sortDescending', filter.sortDescending.toString());

  const response = await client.get<PagedResult<CardSummaryDto>>('/cards', { params });
  return response.data;
};

/**
 * Get card by internal ID
 */
export const getCardById = async (id: string): Promise<CardDto | null> => {
  try {
    const response = await client.get<CardDto>(`/cards/${id}`);
    return response.data;
  } catch {
    return null;
  }
};

/**
 * Get card by Scryfall ID
 */
export const getCardByScryfallId = async (scryfallId: string): Promise<CardDto | null> => {
  try {
    const response = await client.get<CardDto>(`/cards/scryfall/${scryfallId}`);
    return response.data;
  } catch {
    return null;
  }
};

/**
 * Get all available set codes
 */
export const getSets = async (): Promise<string[]> => {
  const response = await client.get<string[]>('/cards/sets');
  return response.data;
};

/**
 * Get all cards in a specific set
 */
export const getCardsBySet = async (setCode: string): Promise<CardSummaryDto[]> => {
  const response = await client.get<CardSummaryDto[]>(`/cards/sets/${setCode}`);
  return response.data;
};

/**
 * Get random cards
 */
export const getRandomCards = async (count = 10): Promise<CardSummaryDto[]> => {
  const response = await client.get<CardSummaryDto[]>('/cards/random', {
    params: { count },
  });
  return response.data;
};
