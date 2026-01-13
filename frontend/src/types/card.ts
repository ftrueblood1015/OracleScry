/**
 * Card image URIs for different sizes
 */
export interface CardImageUris {
  small?: string;
  normal?: string;
  large?: string;
  png?: string;
  artCrop?: string;
  borderCrop?: string;
}

/**
 * Card prices from different sources
 */
export interface CardPrices {
  usd?: string;
  usdFoil?: string;
  usdEtched?: string;
  eur?: string;
  eurFoil?: string;
  tix?: string;
}

/**
 * Card legality in various formats
 */
export interface CardLegalities {
  standard?: string;
  future?: string;
  historic?: string;
  timeless?: string;
  gladiator?: string;
  pioneer?: string;
  explorer?: string;
  modern?: string;
  legacy?: string;
  pauper?: string;
  vintage?: string;
  penny?: string;
  commander?: string;
  oathbreaker?: string;
  standardbrawl?: string;
  brawl?: string;
  alchemy?: string;
  paupercommander?: string;
  duel?: string;
  oldschool?: string;
  premodern?: string;
  predh?: string;
}

/**
 * Card face for multi-faced cards
 */
export interface CardFaceDto {
  id: string;
  name: string;
  manaCost?: string;
  typeLine?: string;
  oracleText?: string;
  flavorText?: string;
  colors: string[];
  colorIndicator: string[];
  power?: string;
  toughness?: string;
  loyalty?: string;
  defense?: string;
  artist?: string;
  artistId?: string;
  illustrationId?: string;
  imageUris?: CardImageUris;
  faceIndex: number;
}

/**
 * Related card reference
 */
export interface RelatedCardDto {
  id: string;
  scryfallId: string;
  component: string;
  name: string;
  typeLine?: string;
  uri?: string;
}

/**
 * Card preview information
 */
export interface CardPreview {
  previewedAt?: string;
  sourceUri?: string;
  source?: string;
}

/**
 * Full card details DTO
 */
export interface CardDto {
  id: string;
  scryfallId: string;
  oracleId?: string;
  mtgoId?: number;
  mtgoFoilId?: number;
  tcgplayerId?: number;
  tcgplayerEtchedId?: number;
  cardmarketId?: number;
  arenaId?: number;
  name: string;
  lang: string;
  releasedAt?: string;
  uri?: string;
  scryfallUri?: string;
  layout: string;
  highresImage: boolean;
  imageStatus?: string;
  imageUris?: CardImageUris;
  manaCost?: string;
  cmc: number;
  typeLine?: string;
  oracleText?: string;
  power?: string;
  toughness?: string;
  loyalty?: string;
  defense?: string;
  colors: string[];
  colorIdentity: string[];
  colorIndicator: string[];
  keywords: string[];
  allParts: RelatedCardDto[];
  cardFaces: CardFaceDto[];
  legalities?: CardLegalities;
  games: string[];
  reserved: boolean;
  foil: boolean;
  nonfoil: boolean;
  finishes: string[];
  oversized: boolean;
  promo: boolean;
  reprint: boolean;
  variation: boolean;
  setId?: string;
  set: string;
  setName?: string;
  setType?: string;
  setUri?: string;
  setSearchUri?: string;
  scryfallSetUri?: string;
  rulingsUri?: string;
  printsSearchUri?: string;
  collectorNumber?: string;
  digital: boolean;
  rarity?: string;
  flavorText?: string;
  cardBackId?: string;
  artist?: string;
  artistIds: string[];
  illustrationId?: string;
  borderColor?: string;
  frame?: string;
  frameEffects: string[];
  securityStamp?: string;
  fullArt: boolean;
  textless: boolean;
  booster: boolean;
  storySpotlight: boolean;
  promoTypes: string[];
  edhrecRank?: number;
  pennyRank?: number;
  prices?: CardPrices;
  relatedUris?: Record<string, string>;
  purchaseUris?: Record<string, string>;
  preview?: CardPreview;
  producedMana: string[];
  lifeModifier?: string;
  handModifier?: string;
  attractionLights: number[];
  contentWarning: boolean;
  flavorName?: string;
  watermark?: string;
  importedOn: string;
  lastUpdatedOn: string;
}

/**
 * Card summary for list views
 */
export interface CardSummaryDto {
  id: string;
  scryfallId: string;
  name: string;
  manaCost?: string;
  cmc: number;
  typeLine?: string;
  rarity?: string;
  set: string;
  setName?: string;
  collectorNumber?: string;
  imageUri?: string;
  prices?: CardPrices;
}

/**
 * Card filter parameters for search
 */
export interface CardFilterDto {
  search?: string;
  name?: string;
  set?: string;
  rarity?: string;
  colors?: string[];
  colorIdentity?: string[];
  types?: string[];
  minCmc?: number;
  maxCmc?: number;
  format?: string;
  purposes?: string[];
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDescending?: boolean;
}

/**
 * Format legality values
 */
export type LegalityStatus = 'legal' | 'not_legal' | 'restricted' | 'banned';

/**
 * Card rarity values
 */
export type CardRarity = 'common' | 'uncommon' | 'rare' | 'mythic' | 'special' | 'bonus';

/**
 * MTG color codes
 */
export type MtgColor = 'W' | 'U' | 'B' | 'R' | 'G';
