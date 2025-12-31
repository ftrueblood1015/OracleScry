import { Chip, CircularProgress } from '@mui/material';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import ErrorIcon from '@mui/icons-material/Error';
import HourglassEmptyIcon from '@mui/icons-material/HourglassEmpty';
import CancelIcon from '@mui/icons-material/Cancel';
import type { CardImportStatus } from '../../types';

interface ImportStatusChipProps {
  status: CardImportStatus;
  size?: 'small' | 'medium';
}

export function ImportStatusChip({ status, size = 'small' }: ImportStatusChipProps) {
  const getChipProps = () => {
    switch (status) {
      case 'Pending':
        return {
          color: 'default' as const,
          icon: <HourglassEmptyIcon />,
          label: 'Pending',
        };
      case 'Downloading':
        return {
          color: 'info' as const,
          icon: <CircularProgress size={16} color="inherit" />,
          label: 'Downloading',
        };
      case 'Processing':
        return {
          color: 'info' as const,
          icon: <CircularProgress size={16} color="inherit" />,
          label: 'Processing',
        };
      case 'Completed':
        return {
          color: 'success' as const,
          icon: <CheckCircleIcon />,
          label: 'Completed',
        };
      case 'Failed':
        return {
          color: 'error' as const,
          icon: <ErrorIcon />,
          label: 'Failed',
        };
      case 'Cancelled':
        return {
          color: 'warning' as const,
          icon: <CancelIcon />,
          label: 'Cancelled',
        };
      default:
        return {
          color: 'default' as const,
          label: status,
        };
    }
  };

  const props = getChipProps();

  return <Chip {...props} size={size} />;
}
