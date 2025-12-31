import { Grid, Box, Typography, CircularProgress } from '@mui/material';
import { CardTile, CardTileSkeleton } from './CardTile';
import type { CardSummaryDto } from '../../types';

interface CardGridProps {
  cards: CardSummaryDto[];
  isLoading?: boolean;
  onCardClick?: (card: CardSummaryDto) => void;
  emptyMessage?: string;
}

export function CardGrid({
  cards,
  isLoading = false,
  onCardClick,
  emptyMessage = 'No cards found',
}: CardGridProps) {
  if (isLoading) {
    return (
      <Grid container spacing={2}>
        {Array.from({ length: 12 }).map((_, index) => (
          <Grid key={index} size={{ xs: 6, sm: 4, md: 3, lg: 2 }}>
            <CardTileSkeleton />
          </Grid>
        ))}
      </Grid>
    );
  }

  if (cards.length === 0) {
    return (
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'center',
          py: 8,
        }}
      >
        <Typography variant="h6" color="text.secondary">
          {emptyMessage}
        </Typography>
      </Box>
    );
  }

  return (
    <Grid container spacing={2}>
      {cards.map((card) => (
        <Grid key={card.id} size={{ xs: 6, sm: 4, md: 3, lg: 2 }}>
          <CardTile card={card} onClick={onCardClick} />
        </Grid>
      ))}
    </Grid>
  );
}

interface CardGridWithLoadMoreProps extends CardGridProps {
  hasMore?: boolean;
  loadingMore?: boolean;
  onLoadMore?: () => void;
}

export function CardGridWithLoadMore({
  cards,
  isLoading,
  onCardClick,
  emptyMessage,
  hasMore,
  loadingMore,
  onLoadMore,
}: CardGridWithLoadMoreProps) {
  return (
    <Box>
      <CardGrid
        cards={cards}
        isLoading={isLoading}
        onCardClick={onCardClick}
        emptyMessage={emptyMessage}
      />
      {hasMore && !isLoading && (
        <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
          {loadingMore ? (
            <CircularProgress size={32} />
          ) : (
            <Typography
              variant="body2"
              color="primary"
              sx={{ cursor: 'pointer', '&:hover': { textDecoration: 'underline' } }}
              onClick={onLoadMore}
            >
              Load more
            </Typography>
          )}
        </Box>
      )}
    </Box>
  );
}
