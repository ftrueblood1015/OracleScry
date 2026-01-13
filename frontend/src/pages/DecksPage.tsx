import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Typography, Button, Dialog, DialogTitle, DialogContent, DialogActions } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import { DeckList, CreateDeckDialog } from '../components';
import { useMyDecks, useCreateDeck, useDeleteDeck } from '../hooks';
import type { DeckSummaryDto } from '../types';

export function DecksPage() {
  const navigate = useNavigate();
  const [createDialogOpen, setCreateDialogOpen] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState<DeckSummaryDto | null>(null);
  const [createError, setCreateError] = useState<string | null>(null);

  const { data: decks, isLoading, error } = useMyDecks();
  const createDeck = useCreateDeck();
  const deleteDeck = useDeleteDeck();

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

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" fontWeight={700}>
          My Decks
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => setCreateDialogOpen(true)}
        >
          New Deck
        </Button>
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
