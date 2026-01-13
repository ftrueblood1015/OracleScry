import { Box, Grid, Typography, CircularProgress, Alert } from '@mui/material';
import { DeckCard } from './DeckCard';
import type { DeckSummaryDto } from '../../types';

interface DeckListProps {
  decks: DeckSummaryDto[] | undefined;
  isLoading: boolean;
  error?: Error | null;
  onDeckClick?: (deck: DeckSummaryDto) => void;
  onDeckEdit?: (deck: DeckSummaryDto) => void;
  onDeckDelete?: (deck: DeckSummaryDto) => void;
  emptyMessage?: string;
}

export function DeckList({
  decks,
  isLoading,
  error,
  onDeckClick,
  onDeckEdit,
  onDeckDelete,
  emptyMessage = 'No decks found',
}: DeckListProps) {
  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Alert severity="error" sx={{ my: 2 }}>
        Failed to load decks: {error.message}
      </Alert>
    );
  }

  if (!decks || decks.length === 0) {
    return (
      <Box sx={{ textAlign: 'center', py: 8 }}>
        <Typography variant="h6" color="text.secondary">
          {emptyMessage}
        </Typography>
      </Box>
    );
  }

  return (
    <Grid container spacing={3}>
      {decks.map((deck) => (
        <Grid key={deck.id} size={{ xs: 12, sm: 6, md: 4, lg: 3 }}>
          <DeckCard
            deck={deck}
            onClick={onDeckClick ? () => onDeckClick(deck) : undefined}
            onEdit={onDeckEdit ? () => onDeckEdit(deck) : undefined}
            onDelete={onDeckDelete ? () => onDeckDelete(deck) : undefined}
          />
        </Grid>
      ))}
    </Grid>
  );
}
