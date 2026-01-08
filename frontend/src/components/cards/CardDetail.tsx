import {
  Box,
  Typography,
  Chip,
  Divider,
  Grid,
  Paper,
  Skeleton,
  Link,
} from '@mui/material';
import type { CardDto } from '../../types';
import { CardPurposes } from '../purposes';

interface CardDetailProps {
  card: CardDto;
}

const formatLegality = (status: string | undefined): { label: string; color: string } => {
  switch (status) {
    case 'legal':
      return { label: 'Legal', color: '#22c55e' };
    case 'not_legal':
      return { label: 'Not Legal', color: '#6b7280' };
    case 'banned':
      return { label: 'Banned', color: '#ef4444' };
    case 'restricted':
      return { label: 'Restricted', color: '#f59e0b' };
    default:
      return { label: 'Unknown', color: '#6b7280' };
  }
};

const formatMap: Record<string, string> = {
  standard: 'Standard',
  pioneer: 'Pioneer',
  modern: 'Modern',
  legacy: 'Legacy',
  vintage: 'Vintage',
  commander: 'Commander',
  pauper: 'Pauper',
  historic: 'Historic',
  explorer: 'Explorer',
  brawl: 'Brawl',
};

export function CardDetail({ card }: CardDetailProps) {
  const mainImage =
    card.imageUris?.large ||
    card.imageUris?.normal ||
    card.cardFaces?.[0]?.imageUris?.large ||
    card.cardFaces?.[0]?.imageUris?.normal;

  return (
    <Grid container spacing={4}>
      {/* Card Image */}
      <Grid size={{ xs: 12, md: 5 }}>
        <Box
          component="img"
          src={mainImage || '/card-back.png'}
          alt={card.name}
          sx={{
            width: '100%',
            maxWidth: 400,
            borderRadius: 2,
            boxShadow: 3,
          }}
        />
        {card.cardFaces && card.cardFaces.length > 1 && (
          <Box sx={{ display: 'flex', gap: 2, mt: 2 }}>
            {card.cardFaces.map((face) => (
              <Box
                key={face.id}
                component="img"
                src={face.imageUris?.normal || '/card-back.png'}
                alt={face.name}
                sx={{
                  width: 120,
                  borderRadius: 1,
                  cursor: 'pointer',
                  opacity: 0.8,
                  '&:hover': { opacity: 1 },
                }}
              />
            ))}
          </Box>
        )}
      </Grid>

      {/* Card Details */}
      <Grid size={{ xs: 12, md: 7 }}>
        <Typography variant="h4" fontWeight={700} gutterBottom>
          {card.name}
        </Typography>

        {card.manaCost && (
          <Typography variant="h6" color="text.secondary" gutterBottom>
            {card.manaCost}
          </Typography>
        )}

        <Typography variant="body1" color="text.secondary" gutterBottom>
          {card.typeLine}
        </Typography>

        <Divider sx={{ my: 2 }} />

        {/* Oracle Text */}
        {card.oracleText && (
          <Paper sx={{ p: 2, mb: 2, bgcolor: 'background.default' }}>
            <Typography variant="body1" sx={{ whiteSpace: 'pre-wrap' }}>
              {card.oracleText}
            </Typography>
          </Paper>
        )}

        {/* Card Purposes */}
        <Box sx={{ mb: 2 }}>
          <Typography variant="subtitle2" gutterBottom>
            Card Functions
          </Typography>
          <CardPurposes cardId={card.id} showConfidence />
        </Box>

        {/* Flavor Text */}
        {card.flavorText && (
          <Typography variant="body2" color="text.secondary" fontStyle="italic" sx={{ mb: 2 }}>
            {card.flavorText}
          </Typography>
        )}

        {/* Power/Toughness or Loyalty */}
        {(card.power || card.toughness) && (
          <Typography variant="h6" sx={{ mb: 2 }}>
            {card.power}/{card.toughness}
          </Typography>
        )}
        {card.loyalty && (
          <Typography variant="h6" sx={{ mb: 2 }}>
            Loyalty: {card.loyalty}
          </Typography>
        )}

        <Divider sx={{ my: 2 }} />

        {/* Set & Rarity Info */}
        <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1, mb: 2 }}>
          <Chip label={card.setName || card.set} variant="outlined" />
          <Chip label={`#${card.collectorNumber}`} variant="outlined" />
          {card.rarity && <Chip label={card.rarity} variant="outlined" color="primary" />}
          {card.artist && <Chip label={`Art: ${card.artist}`} variant="outlined" size="small" />}
        </Box>

        {/* Prices */}
        {card.prices && (
          <Box sx={{ mb: 2 }}>
            <Typography variant="subtitle2" gutterBottom>
              Prices
            </Typography>
            <Box sx={{ display: 'flex', gap: 2 }}>
              {card.prices.usd && (
                <Typography variant="body2">
                  USD: <strong>${card.prices.usd}</strong>
                </Typography>
              )}
              {card.prices.usdFoil && (
                <Typography variant="body2">
                  Foil: <strong>${card.prices.usdFoil}</strong>
                </Typography>
              )}
              {card.prices.eur && (
                <Typography variant="body2">
                  EUR: <strong>{card.prices.eur}</strong>
                </Typography>
              )}
            </Box>
          </Box>
        )}

        {/* Legalities */}
        {card.legalities && (
          <Box>
            <Typography variant="subtitle2" gutterBottom>
              Format Legality
            </Typography>
            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1 }}>
              {Object.entries(formatMap).map(([key, label]) => {
                const legality = card.legalities?.[key as keyof typeof card.legalities];
                const { label: statusLabel, color } = formatLegality(legality);
                return (
                  <Chip
                    key={key}
                    label={`${label}: ${statusLabel}`}
                    size="small"
                    sx={{
                      bgcolor: color,
                      color: color === '#6b7280' ? 'text.secondary' : 'black',
                      fontWeight: 500,
                    }}
                  />
                );
              })}
            </Box>
          </Box>
        )}

        {/* External Links */}
        {card.scryfallUri && (
          <Box sx={{ mt: 3 }}>
            <Link href={card.scryfallUri} target="_blank" rel="noopener noreferrer">
              View on Scryfall
            </Link>
          </Box>
        )}

        {/* Related Cards */}
        {card.allParts && card.allParts.length > 0 && (
          <Box sx={{ mt: 3 }}>
            <Divider sx={{ mb: 2 }} />
            <Typography variant="subtitle2" gutterBottom>
              Related Cards
            </Typography>
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
              {card.allParts.map((related) => (
                <Paper
                  key={related.id}
                  sx={{
                    p: 1.5,
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignItems: 'center',
                    bgcolor: 'background.default',
                  }}
                >
                  <Box>
                    <Typography variant="body2" fontWeight={600}>
                      {related.name}
                    </Typography>
                    {related.typeLine && (
                      <Typography variant="caption" color="text.secondary">
                        {related.typeLine}
                      </Typography>
                    )}
                  </Box>
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    <Chip
                      label={related.component.replace('_', ' ')}
                      size="small"
                      variant="outlined"
                      sx={{ textTransform: 'capitalize' }}
                    />
                    {related.uri && (
                      <Link
                        href={related.uri}
                        target="_blank"
                        rel="noopener noreferrer"
                        sx={{ fontSize: '0.75rem' }}
                      >
                        View
                      </Link>
                    )}
                  </Box>
                </Paper>
              ))}
            </Box>
          </Box>
        )}
      </Grid>
    </Grid>
  );
}

export function CardDetailSkeleton() {
  return (
    <Grid container spacing={4}>
      <Grid size={{ xs: 12, md: 5 }}>
        <Skeleton variant="rectangular" sx={{ aspectRatio: '488/680', borderRadius: 2 }} />
      </Grid>
      <Grid size={{ xs: 12, md: 7 }}>
        <Skeleton variant="text" width="60%" height={48} />
        <Skeleton variant="text" width="30%" />
        <Skeleton variant="text" width="40%" />
        <Divider sx={{ my: 2 }} />
        <Skeleton variant="rectangular" height={100} sx={{ mb: 2 }} />
        <Skeleton variant="text" width="80%" />
        <Skeleton variant="text" width="60%" />
      </Grid>
    </Grid>
  );
}
