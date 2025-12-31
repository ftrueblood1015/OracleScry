import { Box, Paper } from '@mui/material';
import { useNavigate, useLocation } from 'react-router-dom';
import { LoginForm } from '../components';
import { useAuthStore } from '../stores';
import { useEffect } from 'react';

export function LoginPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { isAuthenticated } = useAuthStore();

  const from = (location.state as { from?: { pathname: string } })?.from?.pathname || '/';

  useEffect(() => {
    if (isAuthenticated) {
      navigate(from, { replace: true });
    }
  }, [isAuthenticated, navigate, from]);

  const handleSuccess = () => {
    navigate(from, { replace: true });
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
        <LoginForm onSuccess={handleSuccess} />
      </Paper>
    </Box>
  );
}
