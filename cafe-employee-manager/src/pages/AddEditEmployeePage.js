import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from '@tanstack/react-router';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { 
  Box, 
  Button, 
  Typography,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  FormControl,
  FormLabel,
  RadioGroup,
  FormControlLabel,
  Radio,
  MenuItem,
  Select,
  InputLabel,
  Alert
} from '@mui/material';
import { ReusableTextBox } from '../components/ReusableTextBox';
import { employeeApi } from '../api/employeeApi';
import { GenderType, getGenderLabel } from '../constants/enums';
import { cafeApi } from '../api/cafeApi';

export function AddEditEmployeePage() {
  const navigate = useNavigate();
  const { id: employeeId } = useParams();
  const queryClient = useQueryClient();
  const isEdit = !!employeeId;
  const [showLeaveDialog, setShowLeaveDialog] = useState(false);
  const [isDirty, setIsDirty] = useState(false);
  const [apiError, setApiError] = useState('');

  const [formData, setFormData] = useState({
    name: '',
    emailAddress: '',
    phoneNumber: '',
    gender: '',
    cafeId: ''
  });

  const [errors, setErrors] = useState({});

  const { data: cafes = [] , isLoading: cafesLoading} = useQuery({
    queryKey: ['cafes', ],
    queryFn: () => cafeApi.getCafes()
  });


  const { data: employeeData, isLoading: employeeLoading } = useQuery({
    queryKey: ['employee', employeeId],
    queryFn: () => employeeApi.getEmployee(employeeId),
    enabled: !!employeeId,
    onSuccess: (data) => {
      console.log('Employee data received:', data);
    }
  });


  useEffect(() => {
    if (employeeData && !employeeLoading && !cafesLoading) {
      console.log('Setting form data with:', employeeData);
      setFormData({
        name: employeeData.name || '',
        emailAddress: employeeData.emailAddress || '',
        phoneNumber: employeeData.phoneNumber || '',
        gender: employeeData.gender?.toString() || '',
        cafeId: employeeData.cafeId ? employeeData.cafeId.toString() : '' // Convert to string, fallback to empty string
      });
    }
  }, [employeeData, employeeLoading, cafesLoading]);

  const mutation = useMutation({
    mutationFn: async (data) => {
      console.log('Mutation data:', data);
      try {
        if (isEdit) {
          return await employeeApi.updateEmployee(employeeId, data);
        }
        return await employeeApi.createEmployee(data);
      } catch (error) {
        console.error('API Error:', error);
        throw error;
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries(['employees']);
      navigate({ to: '/employees' });
    },
    onError: (error) => {
      setApiError(error.message || 'Failed to save employee');
      console.error('Mutation error:', error);
    }
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    console.log(`Field changed - ${name}:`, value);
    
    setFormData(prev => {
      const newData = { ...prev, [name]: value };
      console.log('Form data updated:', newData);
      return newData;
    });
    setIsDirty(true);
    setApiError('');
    validateField(name, value);
  };

  // Validation
  const validateField = (name, value) => {
    let newErrors = { ...errors };

    switch (name) {
      case 'name':
        if (!value || value.length < 6 || value.length > 10) {
          newErrors[name] = 'Name must be between 6 and 10 characters';
        } else {
          delete newErrors[name];
        }
        break;
      case 'emailAddress':
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!value || !emailRegex.test(value)) {
          newErrors[name] = 'Please enter a valid email address';
        } else {
          delete newErrors[name];
        }
        break;
      case 'phoneNumber':
        const phoneRegex = /^[89]\d{7}$/;
        if (!value || !phoneRegex.test(value)) {
          newErrors[name] = 'Phone number must start with 8 or 9 and have 8 digits';
        } else {
          delete newErrors[name];
        }
        break;
        case 'gender':
          if (value === '' || value === undefined || value === null) {
            newErrors[name] = 'Please select a gender';
          } else {
            delete newErrors[name];
          }
          break;
      default:
        break;
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  // Handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault();
    
    // Validate all fields
    const fieldsToValidate = ['name', 'emailAddress', 'phoneNumber', 'gender'];
    const isValid = fieldsToValidate.every(field => validateField(field, formData[field]));

    if (isValid) {
      try {
        // Prepare the data
        const submitData = {
          ...formData,
          gender: parseInt(formData.gender),
          cafeId: formData.cafeId || null
        };

        console.log('Prepared data for submission:', submitData);

        await mutation.mutateAsync(submitData);
      } catch (error) {
        console.error('Form submission error:', error);
        setApiError(error.message || 'Failed to submit form');
      }
    } else {
      console.log('Form validation failed', errors);
    }
  };

  

  if (employeeLoading || cafesLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
        <Typography>Loading...</Typography>
      </Box>
    );
  }

  const cafeSelectSection = (
    <FormControl fullWidth margin="normal">
    <InputLabel id="cafe-label">Assigned Cafe</InputLabel>
    <Select
  labelId="cafe-label"
  id="cafe-select"
  name="cafeId"
  value={formData.cafeId || ''}
  onChange={handleChange}
  label="Assigned Cafe"
>
  <MenuItem value="">
    <em>None</em>
  </MenuItem>
  {cafes.map(cafe => {
    console.log('Comparing cafe.id:', cafe.id, 'with formData.cafeId:', formData.cafeId);
    return (
      <MenuItem key={cafe.id} value={cafe.id.toString()}>
        {cafe.name}
      </MenuItem>
    );
  })}
</Select>
  </FormControl>
);
  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ maxWidth: 600, mx: 'auto', mt: 4 }}>
      <Typography variant="h4" sx={{ mb: 4 }}>
        {isEdit ? 'Edit Employee' : 'Add New Employee'}
      </Typography>

      {apiError && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {apiError}
        </Alert>
      )}

      <ReusableTextBox
        name="name"
        label="Name"
        value={formData.name}
        onChange={handleChange}
        error={errors.name}
        helperText={errors.name || '6-10 characters'}
        required
      />

      <ReusableTextBox
        name="emailAddress"
        label="Email Address"
        type="email"
        value={formData.emailAddress}
        onChange={handleChange}
        error={errors.emailAddress}
        helperText={errors.emailAddress}
        required
      />

      <ReusableTextBox
        name="phoneNumber"
        label="Phone Number"
        value={formData.phoneNumber}
        onChange={handleChange}
        error={errors.phoneNumber}
        helperText={errors.phoneNumber || 'Must start with 8 or 9 and have 8 digits'}
        required
      />

      <FormControl fullWidth margin="normal" error={!!errors.gender} required>
        <FormLabel id="gender-label">Gender</FormLabel>
        <RadioGroup
          aria-labelledby="gender-label"
          name="gender"
          value={formData.gender.toString()} // Convert number to string for RadioGroup
          onChange={(e) => {
            handleChange({
              target: {
                name: 'gender',
                value: parseInt(e.target.value) // Convert string back to number
              }
            });
          }}
          row
        >
        <FormControlLabel value={GenderType.Male.toString()} control={<Radio />} label="Male" />
    <FormControlLabel value={GenderType.Female.toString()} control={<Radio />} label="Female" />
    <FormControlLabel value={GenderType.Other.toString()} control={<Radio />} label="Other" />
  </RadioGroup>
  {errors.gender && (
    <Typography color="error" variant="caption">
      {errors.gender}
    </Typography>
  )}
    </FormControl>

      {cafeSelectSection}
  
      <Box sx={{ mt: 4, display: 'flex', gap: 2 }}>
        <Button
          variant="contained"
          type="submit"
          disabled={Object.keys(errors).length > 0 || mutation.isLoading}
        >
          {isEdit ? 'Update' : 'Create'} Employee
        </Button>
        <Button
          variant="outlined"
          onClick={() => isDirty ? setShowLeaveDialog(true) : navigate({ to: '/employees' })}
        >
          Cancel
        </Button>
      </Box>

      {/* Leave Dialog */}
      <Dialog open={showLeaveDialog} onClose={() => setShowLeaveDialog(false)}>
        <DialogTitle>Unsaved Changes</DialogTitle>
        <DialogContent>
          Are you sure you want to leave? All unsaved changes will be lost.
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setShowLeaveDialog(false)}>Stay</Button>
          <Button 
            onClick={() => {
              setShowLeaveDialog(false);
              navigate({ to: '/employees' });
            }} 
            color="error"
          >
            Leave
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}