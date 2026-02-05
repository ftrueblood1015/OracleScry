import { useState } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  FormControlLabel,
  Checkbox,
  Alert,
  Box,
  Typography,
  Chip,
  Divider,
} from '@mui/material';
import { TemplateSelector } from './TemplateSelector';
import type { DeckTemplateSummaryDto } from '../../types/deckTemplate';
import type { CreateDeckFromTemplateRequest } from '../../types/deckTemplate';

interface CreateDeckFromTemplateDialogProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (templateId: string, request: CreateDeckFromTemplateRequest) => Promise<void>;
  isLoading?: boolean;
  error?: string | null;
}

export function CreateDeckFromTemplateDialog({
  open,
  onClose,
  onSubmit,
  isLoading,
  error,
}: CreateDeckFromTemplateDialogProps) {
  const [selectedTemplate, setSelectedTemplate] = useState<DeckTemplateSummaryDto | null>(null);
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [copyFormat, setCopyFormat] = useState(true);

  const handleTemplateSelect = (template: DeckTemplateSummaryDto) => {
    setSelectedTemplate(template);
    // Pre-fill name with template name
    if (!name) {
      setName(template.name);
    }
  };

  const handleSubmit = async () => {
    if (!selectedTemplate || !name.trim()) return;

    await onSubmit(selectedTemplate.id, {
      name: name.trim(),
      description: description.trim() || undefined,
      copyFormat,
    });

    // Reset form on success
    handleReset();
  };

  const handleReset = () => {
    setSelectedTemplate(null);
    setName('');
    setDescription('');
    setCopyFormat(true);
  };

  const handleClose = () => {
    handleReset();
    onClose();
  };

  const totalCards = selectedTemplate
    ? selectedTemplate.mainboardCount + selectedTemplate.sideboardCount
    : 0;

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="md" fullWidth>
      <DialogTitle>Create Deck from Template</DialogTitle>
      <DialogContent>
        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        {!selectedTemplate ? (
          <Box sx={{ mt: 1 }}>
            <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
              Select a preconstructed deck to use as a template:
            </Typography>
            <TemplateSelector
              onSelect={handleTemplateSelect}
              selectedId={selectedTemplate?.id}
            />
          </Box>
        ) : (
          <Box sx={{ mt: 1 }}>
            {/* Selected template summary */}
            <Box sx={{ mb: 3, p: 2, bgcolor: 'action.hover', borderRadius: 1 }}>
              <Typography variant="subtitle1" fontWeight={600}>
                {selectedTemplate.name}
              </Typography>
              {selectedTemplate.setName && (
                <Typography variant="body2" color="text.secondary">
                  {selectedTemplate.setName}
                </Typography>
              )}
              <Box sx={{ display: 'flex', gap: 1, mt: 1 }}>
                {selectedTemplate.format && (
                  <Chip label={selectedTemplate.format} size="small" color="primary" />
                )}
                <Chip label={`${totalCards} cards`} size="small" variant="outlined" />
              </Box>
              <Button
                size="small"
                onClick={() => setSelectedTemplate(null)}
                sx={{ mt: 1 }}
              >
                Change Template
              </Button>
            </Box>

            <Divider sx={{ my: 2 }} />

            {/* Deck name and options */}
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
              label="Description (Optional)"
              fullWidth
              multiline
              rows={2}
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              disabled={isLoading}
              sx={{ mb: 2 }}
            />

            <FormControlLabel
              control={
                <Checkbox
                  checked={copyFormat}
                  onChange={(e) => setCopyFormat(e.target.checked)}
                  disabled={isLoading}
                />
              }
              label={`Copy format from template${selectedTemplate.format ? ` (${selectedTemplate.format})` : ''}`}
            />
          </Box>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={isLoading}>
          Cancel
        </Button>
        <Button
          onClick={handleSubmit}
          variant="contained"
          disabled={isLoading || !selectedTemplate || !name.trim()}
        >
          {isLoading ? 'Creating...' : 'Create Deck'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
