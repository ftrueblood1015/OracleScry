import { Card, CardContent, CardMedia, Typography, Box, Chip, IconButton } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import type { DeckSummaryDto } from '../../types';

interface DeckCardProps {
  deck: DeckSummaryDto;
  onClick?: () => void;
  onEdit?: () => void;
  onDelete?: () => void;
}

export function DeckCard({ deck, onClick, onEdit, onDelete }: DeckCardProps) {
  const totalCards = deck.mainboardCount + deck.sideboardCount;

  return (
    <Card
      sx={{
        cursor: onClick ? 'pointer' : 'default',
        transition: 'transform 0.2s, box-shadow 0.2s',
        '&:hover': onClick
          ? {
              transform: 'translateY(-4px)',
              boxShadow: 4,
            }
          : {},
      }}
      onClick={onClick}
    >
      <CardMedia
        component="img"
        height="180"
        image={deck.previewImageUrl || '/placeholder-card.png'}
        alt={deck.name}
        sx={{ objectFit: 'cover', bgcolor: 'grey.800' }}
      />
      <CardContent>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
          <Typography variant="h6" component="div" noWrap sx={{ flex: 1 }}>
            {deck.name}
          </Typography>
          <Box sx={{ display: 'flex', gap: 0.5 }}>
            {onEdit && (
              <IconButton
                size="small"
                onClick={(e) => {
                  e.stopPropagation();
                  onEdit();
                }}
              >
                <EditIcon fontSize="small" />
              </IconButton>
            )}
            {onDelete && (
              <IconButton
                size="small"
                color="error"
                onClick={(e) => {
                  e.stopPropagation();
                  onDelete();
                }}
              >
                <DeleteIcon fontSize="small" />
              </IconButton>
            )}
          </Box>
        </Box>

        {deck.description && (
          <Typography variant="body2" color="text.secondary" noWrap sx={{ mt: 0.5 }}>
            {deck.description}
          </Typography>
        )}

        <Box sx={{ display: 'flex', gap: 1, mt: 1.5, flexWrap: 'wrap' }}>
          {deck.format && (
            <Chip label={deck.format} size="small" color="primary" variant="outlined" />
          )}
          <Chip label={`${totalCards} cards`} size="small" variant="outlined" />
          {deck.sideboardCount > 0 && (
            <Chip label={`${deck.sideboardCount} sideboard`} size="small" variant="outlined" />
          )}
        </Box>

        <Typography variant="caption" color="text.secondary" sx={{ display: 'block', mt: 1 }}>
          Updated {new Date(deck.updatedAt).toLocaleDateString()}
        </Typography>
      </CardContent>
    </Card>
  );
}
