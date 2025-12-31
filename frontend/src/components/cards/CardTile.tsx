import { Card, CardMedia, CardContent, Typography, Box, Chip, Skeleton } from '@mui/material';
import type { CardSummaryDto } from '../../types';

interface CardTileProps {
  card: CardSummaryDto;
  onClick?: (card: CardSummaryDto) => void;
}

const rarityColors: Record<string, string> = {
  common: '#9ca3af',
  uncommon: '#60a5fa',
  rare: '#fbbf24',
  mythic: '#f97316',
  special: '#a855f7',
  bonus: '#a855f7',
};

export function CardTile({ card, onClick }: CardTileProps) {
  const handleClick = () => onClick?.(card);

  return (
    <Card
      sx={{
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        cursor: onClick ? 'pointer' : 'default',
        transition: 'transform 0.2s, box-shadow 0.2s',
        '&:hover': onClick
          ? {
              transform: 'translateY(-4px)',
              boxShadow: 6,
            }
          : {},
      }}
      onClick={handleClick}
    >
      <CardMedia
        component="img"
        image={card.imageUri || '/card-back.png'}
        alt={card.name}
        sx={{
          aspectRatio: '488/680',
          objectFit: 'cover',
          borderRadius: 1,
        }}
      />
      <CardContent sx={{ flexGrow: 1, py: 1, px: 1.5 }}>
        <Typography variant="body2" fontWeight={600} noWrap title={card.name}>
          {card.name}
        </Typography>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5, mt: 0.5 }}>
          <Typography variant="caption" color="text.secondary" noWrap sx={{ flex: 1 }}>
            {card.setName || card.set.toUpperCase()} #{card.collectorNumber}
          </Typography>
          {card.rarity && (
            <Chip
              label={card.rarity[0].toUpperCase()}
              size="small"
              sx={{
                height: 18,
                fontSize: '0.65rem',
                bgcolor: rarityColors[card.rarity] || rarityColors.common,
                color: 'black',
                fontWeight: 600,
              }}
            />
          )}
        </Box>
        {card.prices?.usd && (
          <Typography variant="caption" color="secondary.main" fontWeight={500}>
            ${card.prices.usd}
          </Typography>
        )}
      </CardContent>
    </Card>
  );
}

export function CardTileSkeleton() {
  return (
    <Card sx={{ height: '100%' }}>
      <Skeleton variant="rectangular" sx={{ aspectRatio: '488/680' }} />
      <CardContent sx={{ py: 1, px: 1.5 }}>
        <Skeleton variant="text" width="80%" />
        <Skeleton variant="text" width="60%" />
      </CardContent>
    </Card>
  );
}
