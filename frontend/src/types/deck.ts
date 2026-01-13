/**
 * Deck format options
 */
export type DeckFormat =
  | 'freeform'
  | 'standard'
  | 'pioneer'
  | 'modern'
  | 'legacy'
  | 'vintage'
  | 'commander'
  | 'pauper'
  | 'brawl';

/**
 * Lightweight deck summary for list views
 */
export interface DeckSummaryDto {
  id: string;
  name: string;
  description?: string;
  format?: string;
  mainboardCount: number;
  sideboardCount: number;
  previewImageUrl?: string;
  updatedAt: string;
}

/**
 * Card within a deck
 */
export interface DeckCardDto {
  cardId: string;
  name: string;
  manaCost?: string;
  cmc: number;
  typeLine: string;
  rarity?: string;
  imageUrl?: string;
  quantity: number;
  isSideboard: boolean;
  purposes: string[];
}

/**
 * Full deck details with cards
 */
export interface DeckDto {
  id: string;
  name: string;
  description?: string;
  format?: string;
  isPublic: boolean;
  mainboardCount: number;
  sideboardCount: number;
  createdAt: string;
  updatedAt: string;
  cards: DeckCardDto[];
}

/**
 * Deck statistics
 */
export interface DeckStatsDto {
  totalCards: number;
  mainboardCount: number;
  sideboardCount: number;
  uniqueCards: number;
  manaCurve: Record<number, number>;
  colorDistribution: Record<string, number>;
  typeDistribution: Record<string, number>;
  purposeBreakdown: Record<string, number>;
  rarityDistribution: Record<string, number>;
  averageCmc: number;
  estimatedPrice?: number;
}

/**
 * Request to create a deck
 */
export interface CreateDeckRequest {
  name: string;
  description?: string;
  format?: string;
}

/**
 * Request to update a deck
 */
export interface UpdateDeckRequest {
  name?: string;
  description?: string;
  format?: string;
  isPublic?: boolean;
}

/**
 * Request to add a card to a deck
 */
export interface AddCardRequest {
  cardId: string;
  quantity?: number;
  isSideboard?: boolean;
}

/**
 * Request to update a card in a deck
 */
export interface UpdateCardRequest {
  quantity?: number;
  isSideboard?: boolean;
}

/**
 * Format display options
 */
export const DECK_FORMAT_OPTIONS: { value: DeckFormat; label: string }[] = [
  { value: 'freeform', label: 'Freeform' },
  { value: 'standard', label: 'Standard' },
  { value: 'pioneer', label: 'Pioneer' },
  { value: 'modern', label: 'Modern' },
  { value: 'legacy', label: 'Legacy' },
  { value: 'vintage', label: 'Vintage' },
  { value: 'commander', label: 'Commander' },
  { value: 'pauper', label: 'Pauper' },
  { value: 'brawl', label: 'Brawl' },
];
