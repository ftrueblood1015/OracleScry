import { useState } from 'react';
import {
  Box,
  TextField,
  Button,
  Typography,
  Alert,
  CircularProgress,
  Link as MuiLink,
} from '@mui/material';
import { Link } from 'react-router-dom';
import { useLogin } from '../../hooks';

interface LoginFormProps {
  onSuccess?: () => void;
}

export function LoginForm({ onSuccess }: LoginFormProps) {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const login = useLogin();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const result = await login.mutateAsync({ email, password });
    if (result.success) {
      onSuccess?.();
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ width: '100%', maxWidth: 400 }}>
      <Typography variant="h5" fontWeight={600} gutterBottom>
        Welcome Back
      </Typography>
      <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
        Sign in to access your collection and decks
      </Typography>

      {(login.isError || (login.data && !login.data.success)) && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {login.data?.errors?.[0] || 'An error occurred during login'}
        </Alert>
      )}

      <TextField
        fullWidth
        label="Email"
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        required
        autoComplete="email"
        sx={{ mb: 2 }}
      />

      <TextField
        fullWidth
        label="Password"
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        required
        autoComplete="current-password"
        sx={{ mb: 3 }}
      />

      <Button
        type="submit"
        variant="contained"
        fullWidth
        size="large"
        disabled={login.isPending}
        sx={{ mb: 2 }}
      >
        {login.isPending ? <CircularProgress size={24} /> : 'Sign In'}
      </Button>

      <Typography variant="body2" align="center">
        Don't have an account?{' '}
        <MuiLink component={Link} to="/register">
          Sign up
        </MuiLink>
      </Typography>
    </Box>
  );
}
