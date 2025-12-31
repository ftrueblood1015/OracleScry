import { Box, Paper } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { RegisterForm } from '../components';
import { useAuthStore } from '../stores';
import { useEffect } from 'react';

export function RegisterPage() {
  const navigate = useNavigate();
  const { isAuthenticated } = useAuthStore();

  useEffect(() => {
    if (isAuthenticated) {
      navigate('/', { replace: true });
    }
  }, [isAuthenticated, navigate]);

  const handleSuccess = () => {
    navigate('/', { replace: true });
  };

  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        minHeight: '60vh',
      }}
    >
      <Paper sx={{ p: 4, maxWidth: 440, width: '100%' }}>
        <RegisterForm onSuccess={handleSuccess} />
      </Paper>
    </Box>
  );
}
