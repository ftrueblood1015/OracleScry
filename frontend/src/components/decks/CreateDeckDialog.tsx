import { useState } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Alert,
} from '@mui/material';
import { DECK_FORMAT_OPTIONS } from '../../types';
import type { CreateDeckRequest } from '../../types';

interface CreateDeckDialogProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (request: CreateDeckRequest) => Promise<void>;
  isLoading?: boolean;
  error?: string | null;
}

export function CreateDeckDialog({ open, onClose, onSubmit, isLoading, error }: CreateDeckDialogProps) {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [format, setFormat] = useState('');

  const handleSubmit = async () => {
    if (!name.trim()) return;

    await onSubmit({
      name: name.trim(),
      description: description.trim() || undefined,
      format: format || undefined,
    });

    // Reset form on success
    setName('');
    setDescription('');
    setFormat('');
  };

  const handleClose = () => {
    setName('');
    setDescription('');
    setFormat('');
    onClose();
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>Create New Deck</DialogTitle>
      <DialogContent>
        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        <TextField
          autoFocus
          margin="dense"
          label="Deck Name"
          fullWidth
          required
          value={name}
          onChange={(e) => setName(e.target.value)}
          disabled={isLoading}
          sx={{ mb: 2 }}
        />

        <TextField
          margin="dense"
          label="Description"
          fullWidth
          multiline
          rows={2}
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          disabled={isLoading}
          sx={{ mb: 2 }}
        />

        <FormControl fullWidth margin="dense">
          <InputLabel>Format (Optional)</InputLabel>
          <Select
            value={format}
            onChange={(e) => setFormat(e.target.value)}
            label="Format (Optional)"
            disabled={isLoading}
          >
            <MenuItem value="">
              <em>None</em>
            </MenuItem>
            {DECK_FORMAT_OPTIONS.map((option) => (
              <MenuItem key={option.value} value={option.value}>
                {option.label}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={isLoading}>
          Cancel
        </Button>
        <Button
          onClick={handleSubmit}
          variant="contained"
          disabled={isLoading || !name.trim()}
        >
          {isLoading ? 'Creating...' : 'Create Deck'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
