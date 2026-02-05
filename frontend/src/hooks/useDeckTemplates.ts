import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import {
  getDeckTemplates,
  getDeckTemplate,
  createDeckFromTemplate,
} from '../api/deckTemplates';
import type { CreateDeckFromTemplateRequest } from '../types/deckTemplate';

/**
 * Fetch all deck templates with optional filtering
 */
export const useDeckTemplates = (format?: string, search?: string) => {
  return useQuery({
    queryKey: ['deck-templates', format, search],
    queryFn: () => getDeckTemplates(format, search),
    staleTime: 10 * 60 * 1000, // 10 minutes - templates rarely change
  });
};

/**
 * Fetch a single deck template by ID
 */
export const useDeckTemplate = (id: string, enabled = true) => {
  return useQuery({
    queryKey: ['deck-templates', id],
    queryFn: () => getDeckTemplate(id),
    enabled: enabled && !!id,
    staleTime: 10 * 60 * 1000, // 10 minutes
  });
};

/**
 * Create a new deck from a template
 */
export const useCreateDeckFromTemplate = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({
      templateId,
      request,
    }: {
      templateId: string;
      request: CreateDeckFromTemplateRequest;
    }) => createDeckFromTemplate(templateId, request),
    onSuccess: () => {
      // Invalidate deck queries so the new deck appears in lists
      queryClient.invalidateQueries({ queryKey: ['decks'] });
    },
  });
};
