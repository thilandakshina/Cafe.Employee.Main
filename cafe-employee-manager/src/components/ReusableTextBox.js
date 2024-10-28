import React from 'react';
import { TextField } from '@mui/material';

export function ReusableTextBox({
  name,
  label,
  value,
  onChange,
  error,
  helperText,
  required,
  ...props
}) {
  return (
    <TextField
      fullWidth
      name={name}
      label={label}
      value={value}
      onChange={onChange}
      error={Boolean(error)}
      helperText={helperText}
      required={required}
      margin="normal"
      {...props}
    />
  );
}