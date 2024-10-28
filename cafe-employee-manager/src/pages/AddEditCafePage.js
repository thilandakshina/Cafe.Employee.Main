import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from '@tanstack/react-router';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { Box, CircularProgress, Alert } from '@mui/material';
import { cafeApi } from '../api/cafeApi';
import { CafeForm } from '../components/cafe/CafeForm';
import { UnsavedChangesDialog } from '../components/cafe/UnsavedChangesDialog';

export function AddEditCafePage() {
  const navigate = useNavigate();
  const params = useParams();
  const queryClient = useQueryClient();
  
  const isEdit = Boolean(params?.id);

  const [formData, setFormData] = useState({
    name: '',
    description: '',
    location: '',
    logo: null
  });
  
  const [showLeaveDialog, setShowLeaveDialog] = useState(false);
  const [isDirty, setIsDirty] = useState(false);
  const [errors, setErrors] = useState({});
  const [previewLogo, setPreviewLogo] = useState(null);
  const [submitError, setSubmitError] = useState('');

  const { data: cafeData, isLoading: isLoadingCafe, error: fetchError } = useQuery({
    queryKey: ['cafe', params?.id],
    queryFn: () => cafeApi.getCafe(params?.id),
    enabled: isEdit,
    retry: 1, 
    staleTime: 30000, // Cache data for 30 seconds
  });

  useEffect(() => {
    if (cafeData && isEdit) {
      setFormData({
        name: cafeData.name || '',
        description: cafeData.description || '',
        location: cafeData.location || '',
        logo: null 
      });
      
      if (cafeData.logo) {
        setPreviewLogo(`data:image/png;base64,${cafeData.logo}`);
      }
    }
  }, [cafeData, isEdit]);

  const mutation = useMutation({
    mutationFn: async (data) => {
      try {
        console.log('Mutation starting with data:', data);
        console.log('Is Edit mode:', isEdit);
        console.log('Cafe ID:', params?.id);
        
        if (isEdit) {
          const updateData = {
            name: data.name,
            description: data.description,
            location: data.location,
            ...(data.logo instanceof File && { logo: data.logo })
          };
          
          console.log('Sending update with:', updateData);
          return await cafeApi.updateCafe(params.id, updateData);
        }
        
        return await cafeApi.createCafe(data);
      } catch (error) {
        console.error('Mutation function error:', error);
        throw error;
      }
    },
    onSuccess: (data) => {
      console.log('Mutation success:', data);
      queryClient.invalidateQueries(['cafes']);
      queryClient.invalidateQueries(['cafe', params?.id]);
      navigate({ to: '/' });
    },
    onError: (error) => {
      console.error('Mutation error with full details:', error);
      setSubmitError(error.message || 'Failed to save cafe');
    }
  });

  const validateField = (name, value) => {
    const newErrors = { ...errors };
  
    switch (name) {
      case 'name':
        if (!value || value.length < 6 || value.length > 10) {
          newErrors[name] = 'Name must be between 6 and 10 characters';
        } else {
          delete newErrors[name];
        }
        break;
      case 'description':
        if (!value) {
          newErrors[name] = 'Description is required';
        } else if (value.length > 256) {
          newErrors[name] = 'Description must be less than 256 characters';
        } else {
          delete newErrors[name];
        }
        break;
      case 'location':
        if (!value) {
          newErrors[name] = 'Location is required';
        } else {
          delete newErrors[name];
        }
        break;
    }
  
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const validateForm = () => {
    const requiredFields = ['name', 'description', 'location'];
    const newErrors = {};

    requiredFields.forEach(field => {
      if (!formData[field]) {
        newErrors[field] = `${field.charAt(0).toUpperCase() + field.slice(1)} is required`;
      } else {
        validateField(field, formData[field]);
      }
    });

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData(prev => ({ ...prev, [name]: value }));
    setIsDirty(true);
    validateField(name, value);
    setSubmitError('');
  };

  const handleFileChange = (event) => {
    const file = event.target.files[0];
    if (file) {
      if (file.size <= 2 * 1024 * 1024) {
        setFormData(prev => ({ ...prev, logo: file }));
        setIsDirty(true);
        
        setErrors(prev => {
          const newErrors = { ...prev };
          delete newErrors.logo;
          return newErrors;
        });
        
        const reader = new FileReader();
        reader.onloadend = () => {
          setPreviewLogo(reader.result);
        };
        reader.readAsDataURL(file);
      } else {
        setErrors(prev => ({ ...prev, logo: 'File size must be less than 2MB' }));
        event.target.value = '';
      }
    }
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    console.log('Form submitted with data:', formData);
    setSubmitError('');
    
    if (validateForm()) {
      try {
        console.log('Form valid, attempting mutation');
        await mutation.mutateAsync(formData);
      } catch (error) {
        console.error('Submit handler error:', error);
        setSubmitError(error.message || 'Failed to save cafe');
      }
    } else {
      console.log('Form validation failed', errors);
    }
  };

  const handleNavigateAway = () => {
    if (isDirty) {
      setShowLeaveDialog(true);
    } else {
      navigate({ to: '/' });
    }
  };

  if (isLoadingCafe) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="200px">
        <CircularProgress />
      </Box>
    );
  }

  if (fetchError && isEdit) {
    return (
      <Alert severity="error" sx={{ mt: 2 }}>
        Error loading cafe: {fetchError.message}
      </Alert>
    );
  }

  return (
    <>
      <CafeForm
        formData={formData}
        errors={errors}
        submitError={submitError}
        isEdit={isEdit}
        isPending={mutation.isPending}
        onSubmit={handleSubmit}
        onChange={handleChange}
        onFileChange={handleFileChange}
        onCancel={handleNavigateAway}
        previewLogo={previewLogo}
      />
      
      <UnsavedChangesDialog
        open={showLeaveDialog}
        onStay={() => setShowLeaveDialog(false)}
        onLeave={() => {
          setShowLeaveDialog(false);
          navigate({ to: '/' });
        }}
      />
    </>
  );
}