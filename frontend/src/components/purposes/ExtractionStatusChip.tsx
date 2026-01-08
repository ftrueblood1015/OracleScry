import { Chip } from '@mui/material';
import type { ExtractionJobStatus } from '../../types';

interface ExtractionStatusChipProps {
  status: ExtractionJobStatus | string;
}

const statusConfig: Record<string, { color: 'default' | 'primary' | 'secondary' | 'error' | 'info' | 'success' | 'warning'; label: string }> = {
  Pending: { color: 'default', label: 'Pending' },
  Running: { color: 'primary', label: 'Running' },
  Completed: { color: 'success', label: 'Completed' },
  Failed: { color: 'error', label: 'Failed' },
  Cancelled: { color: 'warning', label: 'Cancelled' },
};

export function ExtractionStatusChip({ status }: ExtractionStatusChipProps) {
  const config = statusConfig[status] ?? { color: 'default' as const, label: status };

  return (
    <Chip
      label={config.label}
      color={config.color}
      size="small"
      variant="filled"
    />
  );
}
