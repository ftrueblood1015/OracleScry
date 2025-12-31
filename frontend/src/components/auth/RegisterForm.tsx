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
import { useRegister } from '../../hooks';

interface RegisterFormProps {
  onSuccess?: () => void;
}

export function RegisterForm({ onSuccess }: RegisterFormProps) {
  const [email, setEmail] = useState('');
  const [displayName, setDisplayName] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [validationError, setValidationError] = useState<string | null>(null);
  const register = useRegister();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setValidationError(null);

    if (password !== confirmPassword) {
      setValidationError('Passwords do not match');
      return;
    }

    if (password.length < 6) {
      setValidationError('Password must be at least 6 characters');
      return;
    }

    const result = await register.mutateAsync({ email, password, confirmPassword, displayName });
    if (result.success) {
      onSuccess?.();
    }
  };

  const error = validationError || (register.data?.errors?.[0]);

  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ width: '100%', maxWidth: 400 }}>
      <Typography variant="h5" fontWeight={600} gutterBottom>
        Create Account
      </Typography>
      <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
        Join OracleScry to build decks and track your collection
      </Typography>

      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      <TextField
        fullWidth
        label="Display Name"
        value={displayName}
        onChange={(e) => setDisplayName(e.target.value)}
        required
        autoComplete="name"
        sx={{ mb: 2 }}
      />

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
        autoComplete="new-password"
        helperText="At least 6 characters with uppercase, lowercase, and digit"
        sx={{ mb: 2 }}
      />

      <TextField
        fullWidth
        label="Confirm Password"
        type="password"
        value={confirmPassword}
        onChange={(e) => setConfirmPassword(e.target.value)}
        required
        autoComplete="new-password"
        error={!!confirmPassword && password !== confirmPassword}
        helperText={confirmPassword && password !== confirmPassword ? 'Passwords do not match' : ''}
        sx={{ mb: 3 }}
      />

      <Button
        type="submit"
        variant="contained"
        fullWidth
        size="large"
        disabled={register.isPending}
        sx={{ mb: 2 }}
      >
        {register.isPending ? <CircularProgress size={24} /> : 'Create Account'}
      </Button>

      <Typography variant="body2" align="center">
        Already have an account?{' '}
        <MuiLink component={Link} to="/login">
          Sign in
        </MuiLink>
      </Typography>
    </Box>
  );
}
