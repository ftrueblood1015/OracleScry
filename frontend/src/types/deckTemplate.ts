/**
 * Lightweight template summary for list views
 */
export interface DeckTemplateSummaryDto {
  id: string;
  name: string;
  description?: string;
  format?: string;
  setName?: string;
  mainboardCount: number;
  sideboardCount: number;
  previewImageUrl?: string;
  releasedAt?: string;
}

/**
 * Card within a deck template
 */
export interface DeckTemplateCardDto {
  cardId: string;
  name: string;
  manaCost?: string;
  cmc: number;
  typeLine?: string;
  rarity?: string;
  imageUrl?: string;
  quantity: number;
  isSideboard: boolean;
  isCommander: boolean;
}

/**
 * Full template details with cards
 */
export interface DeckTemplateDto {
  id: string;
  name: string;
  description?: string;
  format?: string;
  setCode?: string;
  setName?: string;
  releasedAt?: string;
  mainboardCount: number;
  sideboardCount: number;
  cards: DeckTemplateCardDto[];
}

/**
 * Request to create a deck from a template
 */
export interface CreateDeckFromTemplateRequest {
  name: string;
  description?: string;
  copyFormat?: boolean;
}
