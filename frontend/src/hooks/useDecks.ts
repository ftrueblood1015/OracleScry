import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import {
  getMyDecks,
  getDeck,
  createDeck,
  updateDeck,
  deleteDeck,
  addCardToDeck,
  updateDeckCard,
  removeCardFromDeck,
  getDeckStats,
} from '../api';
import type { CreateDeckRequest, UpdateDeckRequest, AddCardRequest, UpdateCardRequest } from '../types';

/**
 * Fetch all decks for the current user
 */
export const useMyDecks = () => {
  return useQuery({
    queryKey: ['decks', 'my'],
    queryFn: getMyDecks,
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
};

/**
 * Fetch a single deck by ID
 */
export const useDeck = (id: string, enabled = true) => {
  return useQuery({
    queryKey: ['decks', id],
    queryFn: () => getDeck(id),
    enabled: enabled && !!id,
    staleTime: 2 * 60 * 1000, // 2 minutes
  });
};

/**
 * Fetch deck statistics
 */
export const useDeckStats = (id: string, enabled = true) => {
  return useQuery({
    queryKey: ['decks', id, 'stats'],
    queryFn: () => getDeckStats(id),
    enabled: enabled && !!id,
    staleTime: 1 * 60 * 1000, // 1 minute
  });
};

/**
 * Create a new deck
 */
export const useCreateDeck = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (request: CreateDeckRequest) => createDeck(request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['decks'] });
    },
  });
};

/**
 * Update an existing deck
 */
export const useUpdateDeck = (id: string) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (request: UpdateDeckRequest) => updateDeck(id, request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['decks', id] });
      queryClient.invalidateQueries({ queryKey: ['decks', 'my'] });
    },
  });
};

/**
 * Delete a deck
 */
export const useDeleteDeck = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => deleteDeck(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['decks'] });
    },
  });
};

/**
 * Add a card to a deck
 */
export const useAddCardToDeck = (deckId: string) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (request: AddCardRequest) => addCardToDeck(deckId, request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['decks', deckId] });
      queryClient.invalidateQueries({ queryKey: ['decks', deckId, 'stats'] });
      queryClient.invalidateQueries({ queryKey: ['decks', 'my'] });
    },
  });
};

/**
 * Update a card in a deck
 */
export const useUpdateDeckCard = (deckId: string) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ cardId, request }: { cardId: string; request: UpdateCardRequest }) =>
      updateDeckCard(deckId, cardId, request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['decks', deckId] });
      queryClient.invalidateQueries({ queryKey: ['decks', deckId, 'stats'] });
      queryClient.invalidateQueries({ queryKey: ['decks', 'my'] });
    },
  });
};

/**
 * Remove a card from a deck
 */
export const useRemoveCardFromDeck = (deckId: string) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (cardId: string) => removeCardFromDeck(deckId, cardId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['decks', deckId] });
      queryClient.invalidateQueries({ queryKey: ['decks', deckId, 'stats'] });
      queryClient.invalidateQueries({ queryKey: ['decks', 'my'] });
    },
  });
};
