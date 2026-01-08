/**
 * Purpose category enum
 */
export type PurposeCategory =
  | 'Removal'
  | 'CardAdvantage'
  | 'Ramp'
  | 'Counterspell'
  | 'Combat'
  | 'Protection'
  | 'Recursion'
  | 'Tokens'
  | 'LifeGain'
  | 'Damage'
  | 'Mill'
  | 'Discard'
  | 'Other';

/**
 * Extraction job status enum
 */
export type ExtractionJobStatus =
  | 'Pending'
  | 'Running'
  | 'Completed'
  | 'Failed'
  | 'Cancelled';

/**
 * Full card purpose details
 */
export interface CardPurposeDto {
  id: string;
  name: string;
  slug: string;
  description?: string;
  category: PurposeCategory;
  displayOrder: number;
  isActive: boolean;
  patterns?: string;
}

/**
 * Lightweight purpose summary
 */
export interface CardPurposeSummaryDto {
  id: string;
  name: string;
  slug: string;
  description?: string;
  category: string;
}

/**
 * Purpose assignment to a card
 */
export interface CardPurposeAssignmentDto {
  purposeId: string;
  name: string;
  slug: string;
  category: string;
  confidence: number;
  matchedPattern?: string;
  assignedAt: string;
  assignedBy: string;
}

/**
 * Full extraction job details
 */
export interface PurposeExtractionJobDto {
  id: string;
  startedAt: string;
  completedAt?: string;
  status: ExtractionJobStatus;
  totalCards: number;
  processedCards: number;
  purposesAssigned: number;
  errorCount: number;
  errorMessage?: string;
  reprocessAll: boolean;
  durationSeconds?: number;
}

/**
 * Lightweight extraction job summary
 */
export interface PurposeExtractionJobSummaryDto {
  id: string;
  startedAt: string;
  completedAt?: string;
  status: string;
  processedCards: number;
  purposesAssigned: number;
}

/**
 * Aggregated extraction statistics
 */
export interface ExtractionStatsDto {
  totalJobs: number;
  successfulJobs: number;
  failedJobs: number;
  totalCardsProcessed: number;
  totalPurposesAssigned: number;
  lastExtraction?: string;
  cardsWithPurposes: number;
  cardsWithoutPurposes: number;
}

/**
 * Create purpose request
 */
export interface CreateCardPurposeRequest {
  name: string;
  description?: string;
  category: PurposeCategory;
  displayOrder: number;
  patterns?: string;
}

/**
 * Update purpose request
 */
export interface UpdateCardPurposeRequest {
  name?: string;
  description?: string;
  category?: PurposeCategory;
  displayOrder?: number;
  isActive?: boolean;
  patterns?: string;
}

/**
 * Extraction status response
 */
export interface ExtractionStatusResponse {
  isRunning: boolean;
}
