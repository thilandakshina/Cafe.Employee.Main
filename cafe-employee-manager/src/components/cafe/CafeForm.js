import React from 'react';
import { 
  Box, 
  Button, 
  Typography,
  Alert,
  FormControl,
  FormHelperText
} from '@mui/material';
import { ReusableTextBox } from '../ReusableTextBox';

export function CafeForm({
  formData,
  errors,
  submitError,
  isEdit,
  isPending,
  onSubmit,
  onChange,
  onFileChange,
  onCancel,
  previewLogo
}) {
  const isFormValid = 
    formData.name?.length >= 6 && 
    formData.name?.length <= 10 &&
    formData.description?.length > 0 &&
    formData.description?.length <= 256 &&
    formData.location?.length > 0 &&
    (formData.logo || previewLogo) && 
    Object.keys(errors).length === 0;

  return (
    <Box component="form" onSubmit={onSubmit} sx={{ maxWidth: 600, mx: 'auto', mt: 4 }}>
      <Typography variant="h4" sx={{ mb: 4 }}>
        {isEdit ? 'Edit Cafe' : 'Add New Cafe'}
      </Typography>

      {submitError && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {submitError}
        </Alert>
      )}

      <ReusableTextBox
        name="name"
        label="Name"
        value={formData.name || ''}
        onChange={onChange}
        error={Boolean(errors.name)}
        helperText={errors.name || '6-10 characters'}
        required
      />

      <ReusableTextBox
        name="description"
        label="Description"
        value={formData.description || ''}
        onChange={onChange}
        error={Boolean(errors.description)}
        helperText={errors.description || 'Max 256 characters'}
        multiline
        rows={4}
        required
      />

      <FormControl error={Boolean(errors.logo)} fullWidth sx={{ mt: 2, mb: 2 }}>
        <input
          accept="image/*"
          type="file"
          onChange={onFileChange}
          id="logo-upload"
          style={{ display: 'block', marginBottom: '8px' }}
          required={!isEdit || !previewLogo}
        />
        <FormHelperText error={Boolean(errors.logo)}>
          {errors.logo || '* Required - Image size should be less than 2MB'}
        </FormHelperText>
        {previewLogo && (
          <Box sx={{ mt: 2 }}>
            <img 
              src={previewLogo}
              alt="logo preview"
              style={{ 
                width: 100, 
                height: 100, 
                objectFit: 'cover',
                borderRadius: '4px'
              }}
            />
          </Box>
        )}
      </FormControl>

      <ReusableTextBox
        name="location"
        label="Location"
        value={formData.location || ''}
        onChange={onChange}
        error={Boolean(errors.location)}
        helperText={errors.location}
        required
      />

      <Box sx={{ mt: 4, display: 'flex', gap: 2 }}>
        <Button
          variant="contained"
          type="submit"
          disabled={!isFormValid || isPending}
          sx={{
            '&.Mui-disabled': {
              backgroundColor: '#ccc',
              color: '#666'
            }
          }}
        >
          {isPending ? 'Saving...' : (isEdit ? 'Update' : 'Create')} Cafe
        </Button>
        <Button
          variant="outlined"
          onClick={onCancel}
          disabled={isPending}
        >
          Cancel
        </Button>
      </Box>

      {/* <Box sx={{ mt: 2, p: 2, bgcolor: '#f5f5f5', borderRadius: 1 }}>
        <Typography variant="caption">
          Form Valid: {isFormValid ? 'Yes' : 'No'}<br />
          Name Valid: {formData.name?.length >= 6 && formData.name?.length <= 10 ? 'Yes' : 'No'}<br />
          Description Valid: {formData.description?.length > 0 && formData.description?.length <= 256 ? 'Yes' : 'No'}<br />
          Location Valid: {formData.location?.length > 0 ? 'Yes' : 'No'}<br />
          Logo Valid: {(formData.logo || previewLogo) ? 'Yes' : 'No'}<br />
          Errors: {Object.keys(errors).length}
        </Typography>
      </Box> */}
    </Box>
  );
}