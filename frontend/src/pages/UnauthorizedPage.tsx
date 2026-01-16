import { Box, Typography, Button } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import HomeIcon from '@mui/icons-material/Home';
import LockIcon from '@mui/icons-material/Lock';

export function UnauthorizedPage() {
  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        minHeight: '60vh',
        textAlign: 'center',
      }}
    >
      <LockIcon sx={{ fontSize: 80, color: 'warning.main', mb: 2 }} />
      <Typography variant="h1" fontWeight={700} color="warning.main" sx={{ mb: 2 }}>
        403
      </Typography>
      <Typography variant="h5" gutterBottom>
        Access Denied
      </Typography>
      <Typography variant="body1" color="text.secondary" sx={{ mb: 4 }}>
        You don't have permission to access this page.
      </Typography>
      <Button
        component={RouterLink}
        to="/"
        variant="contained"
        startIcon={<HomeIcon />}
        size="large"
      >
        Go Home
      </Button>
    </Box>
  );
}
