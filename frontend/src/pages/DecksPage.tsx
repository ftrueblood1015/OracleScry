import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Typography, Button, Dialog, DialogTitle, DialogContent, DialogActions, ButtonGroup } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import ContentCopyIcon from '@mui/icons-material/ContentCopy';
import { DeckList, CreateDeckDialog, CreateDeckFromTemplateDialog } from '../components';
import { useMyDecks, useCreateDeck, useDeleteDeck } from '../hooks';
import { useCreateDeckFromTemplate } from '../hooks/useDeckTemplates';
import type { DeckSummaryDto } from '../types';
import type { CreateDeckFromTemplateRequest } from '../types/deckTemplate';

export function DecksPage() {
  const navigate = useNavigate();
  const [createDialogOpen, setCreateDialogOpen] = useState(false);
  const [templateDialogOpen, setTemplateDialogOpen] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState<DeckSummaryDto | null>(null);
  const [createError, setCreateError] = useState<string | null>(null);
  const [templateError, setTemplateError] = useState<string | null>(null);

  const { data: decks, isLoading, error } = useMyDecks();
  const createDeck = useCreateDeck();
  const deleteDeck = useDeleteDeck();
  const createFromTemplate = useCreateDeckFromTemplate();

  const handleDeckClick = (deck: DeckSummaryDto) => {
    navigate(`/decks/${deck.id}`);
  };

  const handleCreateDeck = async (request: { name: string; description?: string; format?: string }) => {
    setCreateError(null);
    try {
      const newDeck = await createDeck.mutateAsync(request);
      setCreateDialogOpen(false);
      navigate(`/decks/${newDeck.id}`);
    } catch (err) {
      setCreateError(err instanceof Error ? err.message : 'Failed to create deck');
    }
  };

  const handleDeleteDeck = async () => {
    if (!deleteTarget) return;
    try {
      await deleteDeck.mutateAsync(deleteTarget.id);
      setDeleteTarget(null);
    } catch (err) {
      console.error('Failed to delete deck:', err);
    }
  };

  const handleCreateFromTemplate = async (templateId: string, request: CreateDeckFromTemplateRequest) => {
    setTemplateError(null);
    try {
      const newDeck = await createFromTemplate.mutateAsync({ templateId, request });
      setTemplateDialogOpen(false);
      navigate(`/decks/${newDeck.id}`);
    } catch (err) {
      setTemplateError(err instanceof Error ? err.message : 'Failed to create deck from template');
    }
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" fontWeight={700}>
          My Decks
        </Typography>
        <ButtonGroup variant="contained">
          <Button
            startIcon={<AddIcon />}
            onClick={() => setCreateDialogOpen(true)}
          >
            New Deck
          </Button>
          <Button
            startIcon={<ContentCopyIcon />}
            onClick={() => setTemplateDialogOpen(true)}
          >
            From Template
          </Button>
        </ButtonGroup>
      </Box>

      <DeckList
        decks={decks}
        isLoading={isLoading}
        error={error}
        onDeckClick={handleDeckClick}
        onDeckDelete={setDeleteTarget}
        emptyMessage="You don't have any decks yet. Create one to get started!"
      />

      <CreateDeckDialog
        open={createDialogOpen}
        onClose={() => {
          setCreateDialogOpen(false);
          setCreateError(null);
        }}
        onSubmit={handleCreateDeck}
        isLoading={createDeck.isPending}
        error={createError}
      />

      <CreateDeckFromTemplateDialog
        open={templateDialogOpen}
        onClose={() => {
          setTemplateDialogOpen(false);
          setTemplateError(null);
        }}
        onSubmit={handleCreateFromTemplate}
        isLoading={createFromTemplate.isPending}
        error={templateError}
      />

      {/* Delete Confirmation Dialog */}
      <Dialog open={!!deleteTarget} onClose={() => setDeleteTarget(null)}>
        <DialogTitle>Delete Deck</DialogTitle>
        <DialogContent>
          Are you sure you want to delete "{deleteTarget?.name}"? This action cannot be undone.
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteTarget(null)}>Cancel</Button>
          <Button
            onClick={handleDeleteDeck}
            color="error"
            variant="contained"
            disabled={deleteDeck.isPending}
          >
            {deleteDeck.isPending ? 'Deleting...' : 'Delete'}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}
