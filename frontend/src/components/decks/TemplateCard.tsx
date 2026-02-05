import { Card, CardContent, CardMedia, Typography, Box, Chip } from '@mui/material';
import type { DeckTemplateSummaryDto } from '../../types/deckTemplate';

interface TemplateCardProps {
  template: DeckTemplateSummaryDto;
  onClick?: () => void;
  selected?: boolean;
}

export function TemplateCard({ template, onClick, selected }: TemplateCardProps) {
  const totalCards = template.mainboardCount + template.sideboardCount;

  return (
    <Card
      sx={{
        cursor: onClick ? 'pointer' : 'default',
        transition: 'transform 0.2s, box-shadow 0.2s',
        border: selected ? 2 : 0,
        borderColor: 'primary.main',
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
        image={template.previewImageUrl || '/placeholder-card.png'}
        alt={template.name}
        sx={{ objectFit: 'cover', bgcolor: 'grey.800' }}
      />
      <CardContent>
        <Typography variant="h6" component="div" noWrap>
          {template.name}
        </Typography>

        {template.setName && (
          <Typography variant="body2" color="text.secondary" noWrap sx={{ mt: 0.5 }}>
            {template.setName}
          </Typography>
        )}

        {template.description && (
          <Typography
            variant="body2"
            color="text.secondary"
            sx={{
              mt: 0.5,
              display: '-webkit-box',
              WebkitLineClamp: 2,
              WebkitBoxOrient: 'vertical',
              overflow: 'hidden',
            }}
          >
            {template.description}
          </Typography>
        )}

        <Box sx={{ display: 'flex', gap: 1, mt: 1.5, flexWrap: 'wrap' }}>
          {template.format && (
            <Chip label={template.format} size="small" color="primary" variant="outlined" />
          )}
          <Chip label={`${totalCards} cards`} size="small" variant="outlined" />
          {template.sideboardCount > 0 && (
            <Chip label={`${template.sideboardCount} sideboard`} size="small" variant="outlined" />
          )}
        </Box>

        {template.releasedAt && (
          <Typography variant="caption" color="text.secondary" sx={{ display: 'block', mt: 1 }}>
            Released {new Date(template.releasedAt).toLocaleDateString()}
          </Typography>
        )}
      </CardContent>
    </Card>
  );
}
