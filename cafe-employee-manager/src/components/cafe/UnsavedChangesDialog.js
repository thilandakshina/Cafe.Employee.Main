import React from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button
} from '@mui/material';

export function UnsavedChangesDialog({ open, onStay, onLeave }) {
  return (
    <Dialog open={open} onClose={onStay}>
      <DialogTitle>Unsaved Changes</DialogTitle>
      <DialogContent>
        Are you sure you want to leave? All unsaved changes will be lost.
      </DialogContent>
      <DialogActions>
        <Button onClick={onStay}>Stay</Button>
        <Button onClick={onLeave} color="error">
          Leave
        </Button>
      </DialogActions>
    </Dialog>
  );
}
