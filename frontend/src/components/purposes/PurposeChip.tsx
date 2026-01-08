import { Chip, Tooltip } from '@mui/material';
import type { CardPurposeAssignmentDto, CardPurposeSummaryDto } from '../../types';

// Color mapping for purpose categories
const categoryColors: Record<string, string> = {
  Removal: '#ef5350',       // Red
  CardAdvantage: '#42a5f5', // Blue
  Ramp: '#66bb6a',          // Green
  Counterspell: '#5c6bc0',  // Indigo
  Combat: '#ff7043',        // Deep Orange
  Protection: '#78909c',    // Blue Grey
  Recursion: '#ab47bc',     // Purple
  Tokens: '#26a69a',        // Teal
  LifeGain: '#ffee58',      // Yellow
  Damage: '#d32f2f',        // Dark Red
  Mill: '#7e57c2',          // Deep Purple
  Discard: '#8d6e63',       // Brown
  Other: '#9e9e9e',         // Grey
};

interface PurposeChipProps {
  purpose: CardPurposeAssignmentDto | CardPurposeSummaryDto;
  showConfidence?: boolean;
  onClick?: () => void;
  size?: 'small' | 'medium';
}

export function PurposeChip({ purpose, showConfidence = false, onClick, size = 'small' }: PurposeChipProps) {
  const color = categoryColors[purpose.category] ?? categoryColors.Other;
  const confidence = 'confidence' in purpose ? purpose.confidence : undefined;

  const label = showConfidence && confidence !== undefined
    ? `${purpose.name} (${Math.round(confidence * 100)}%)`
    : purpose.name;

  const tooltipContent = 'matchedPattern' in purpose && purpose.matchedPattern
    ? `Matched: "${purpose.matchedPattern}"`
    : purpose.slug;

  return (
    <Tooltip title={tooltipContent} arrow>
      <Chip
        label={label}
        size={size}
        onClick={onClick}
        sx={{
          backgroundColor: color,
          color: 'white',
          fontWeight: 500,
          '&:hover': onClick ? { opacity: 0.8 } : undefined,
          cursor: onClick ? 'pointer' : 'default',
        }}
      />
    </Tooltip>
  );
}
