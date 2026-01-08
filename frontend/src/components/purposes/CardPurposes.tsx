import { Box, Typography, Skeleton, Stack } from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { PurposeChip } from './PurposeChip';
import { getPurposesForCard } from '../../api/purposes';
import type { CardPurposeAssignmentDto } from '../../types';

interface CardPurposesProps {
  cardId: string;
  showConfidence?: boolean;
}

export function CardPurposes({ cardId, showConfidence = true }: CardPurposesProps) {
  const { data: purposes, isLoading, error } = useQuery({
    queryKey: ['cardPurposes', cardId],
    queryFn: () => getPurposesForCard(cardId),
    staleTime: 5 * 60 * 1000, // 5 minutes
  });

  if (isLoading) {
    return (
      <Stack direction="row" spacing={1} flexWrap="wrap" useFlexGap>
        {Array.from({ length: 3 }).map((_, i) => (
          <Skeleton key={i} variant="rounded" width={80} height={24} />
        ))}
      </Stack>
    );
  }

  if (error) {
    return (
      <Typography variant="body2" color="error">
        Failed to load purposes
      </Typography>
    );
  }

  if (!purposes?.length) {
    return (
      <Typography variant="body2" color="text.secondary">
        No purposes assigned
      </Typography>
    );
  }

  return (
    <Box>
      <Stack direction="row" spacing={1} flexWrap="wrap" useFlexGap>
        {purposes.map((purpose: CardPurposeAssignmentDto) => (
          <PurposeChip
            key={purpose.purposeId}
            purpose={purpose}
            showConfidence={showConfidence}
          />
        ))}
      </Stack>
    </Box>
  );
}
