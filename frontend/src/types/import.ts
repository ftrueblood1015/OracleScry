/**
 * Card import status enum
 */
export type CardImportStatus =
  | 'Pending'
  | 'Downloading'
  | 'Processing'
  | 'Completed'
  | 'Failed'
  | 'Cancelled';

/**
 * Individual card import error
 */
export interface CardImportErrorDto {
  id: string;
  oracleId?: string;
  cardName?: string;
  errorMessage: string;
}

/**
 * Full card import details
 */
export interface CardImportDto {
  id: string;
  startedAt: string;
  completedAt?: string;
  status: CardImportStatus;
  totalCardsInFile: number;
  cardsProcessed: number;
  cardsAdded: number;
  cardsUpdated: number;
  cardsSkipped: number;
  cardsFailed: number;
  bulkDataId: string;
  downloadUri: string;
  scryfallUpdatedAt: string;
  fileSizeBytes: number;
  errorMessage?: string;
  durationSeconds?: number;
  errors?: CardImportErrorDto[];
}

/**
 * Lightweight import summary for history listing
 */
export interface CardImportSummaryDto {
  id: string;
  startedAt: string;
  completedAt?: string;
  status: CardImportStatus;
  cardsAdded: number;
  cardsUpdated: number;
  cardsFailed: number;
  durationSeconds?: number;
}

/**
 * Aggregated import statistics
 */
export interface ImportStatsDto {
  totalImports: number;
  successfulImports: number;
  failedImports: number;
  totalCardsAdded: number;
  totalCardsUpdated: number;
  lastImportAt?: string;
  averageDurationSeconds?: number;
}

/**
 * Import status response
 */
export interface ImportStatusResponse {
  isRunning: boolean;
}
