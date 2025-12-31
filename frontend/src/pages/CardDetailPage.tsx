import { Box, Button, Typography } from '@mui/material';
import { useParams, useNavigate } from 'react-router-dom';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { CardDetail, CardDetailSkeleton } from '../components';
import { useCard } from '../hooks';

export function CardDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { data: card, isLoading, error } = useCard(id || '');

  const handleBack = () => {
    if (window.history.length > 2) {
      navigate(-1);
    } else {
      navigate('/search');
    }
  };

  if (isLoading) {
    return (
      <Box>
        <Button startIcon={<ArrowBackIcon />} onClick={handleBack} sx={{ mb: 3 }}>
          Back
        </Button>
        <CardDetailSkeleton />
      </Box>
    );
  }

  if (error || !card) {
    return (
      <Box>
        <Button startIcon={<ArrowBackIcon />} onClick={handleBack} sx={{ mb: 3 }}>
          Back
        </Button>
        <Typography variant="h5" color="error">
          Card not found
        </Typography>
        <Typography color="text.secondary">
          The card you're looking for doesn't exist or has been removed.
        </Typography>
      </Box>
    );
  }

  return (
    <Box>
      <Button startIcon={<ArrowBackIcon />} onClick={handleBack} sx={{ mb: 3 }}>
        Back
      </Button>
      <CardDetail card={card} />
    </Box>
  );
}
