import client from './client';
import type {
  DeckDto,
  DeckSummaryDto,
  DeckCardDto,
  DeckStatsDto,
  CreateDeckRequest,
  UpdateDeckRequest,
  AddCardRequest,
  UpdateCardRequest,
} from '../types';

/**
 * Get all decks for the current user
 */
export const getMyDecks = async (): Promise<DeckSummaryDto[]> => {
  const response = await client.get<DeckSummaryDto[]>('/decks');
  return response.data;
};

/**
 * Get a specific deck by ID
 */
export const getDeck = async (id: string): Promise<DeckDto> => {
  const response = await client.get<DeckDto>(`/decks/${id}`);
  return response.data;
};

/**
 * Create a new deck
 */
export const createDeck = async (request: CreateDeckRequest): Promise<DeckDto> => {
  const response = await client.post<DeckDto>('/decks', request);
  return response.data;
};

/**
 * Update a deck
 */
export const updateDeck = async (id: string, request: UpdateDeckRequest): Promise<DeckDto> => {
  const response = await client.put<DeckDto>(`/decks/${id}`, request);
  return response.data;
};

/**
 * Delete a deck
 */
export const deleteDeck = async (id: string): Promise<void> => {
  await client.delete(`/decks/${id}`);
};

/**
 * Add a card to a deck
 */
export const addCardToDeck = async (deckId: string, request: AddCardRequest): Promise<DeckCardDto> => {
  const response = await client.post<DeckCardDto>(`/decks/${deckId}/cards`, request);
  return response.data;
};

/**
 * Update a card in a deck
 */
export const updateDeckCard = async (
  deckId: string,
  cardId: string,
  request: UpdateCardRequest
): Promise<DeckCardDto> => {
  const response = await client.put<DeckCardDto>(`/decks/${deckId}/cards/${cardId}`, request);
  return response.data;
};

/**
 * Remove a card from a deck
 */
export const removeCardFromDeck = async (deckId: string, cardId: string): Promise<void> => {
  await client.delete(`/decks/${deckId}/cards/${cardId}`);
};

/**
 * Get deck statistics
 */
export const getDeckStats = async (id: string): Promise<DeckStatsDto> => {
  const response = await client.get<DeckStatsDto>(`/decks/${id}/stats`);
  return response.data;
};
