import React, { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { useNavigate, useSearch } from '@tanstack/react-router';
import { AgGridReact } from 'ag-grid-react';
import { 
  Button, 
  Box, 
  Typography,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Paper,
  Breadcrumbs,
  Link,
  IconButton,
  Tooltip,
  Alert,
  Fade,
  TextField,
  InputAdornment
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import SearchIcon from '@mui/icons-material/Search';
import ClearIcon from '@mui/icons-material/Clear';
import HomeIcon from '@mui/icons-material/Home';
import EmailIcon from '@mui/icons-material/Email';
import PhoneIcon from '@mui/icons-material/Phone';
import WorkIcon from '@mui/icons-material/Work';
import PersonIcon from '@mui/icons-material/Person';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-material.css';
import { employeeApi } from '../api/employeeApi';
import { getGenderLabel } from '../constants/enums';


export function EmployeesPage() {
  const navigate = useNavigate();
  const search = useSearch();
  const selectedCafeName = search.cafename;
  const selectedCafe = search.cafe;
  const [deleteId, setDeleteId] = useState(null);
  const [showSuccessAlert, setShowSuccessAlert] = useState(false);
  const [nameFilter, setNameFilter] = useState('');
  const [isDeleting, setIsDeleting] = useState(false);

  const { data: employees = [], isLoading, refetch } = useQuery({
    queryKey: ['employees', selectedCafe],
    queryFn: () => employeeApi.getEmployees(selectedCafe)
  });

  
  const filteredEmployees = employees.filter(employee => 
    employee.name.toLowerCase().includes(nameFilter.toLowerCase())
  );

  const handleDelete = async () => {
    setIsDeleting(true);
    try {
      await employeeApi.deleteEmployee(deleteId);
      setShowSuccessAlert(true);
      setTimeout(() => setShowSuccessAlert(false), 3000);
      refetch();
    } catch (error) {
      console.error('Error deleting employee:', error);
    } finally {
      setIsDeleting(false);
      setDeleteId(null);
    }
  };

  const handleClearFilter = () => {
    setNameFilter('');
  };

  const columnDefs = [
    { 
      field: 'id', 
      hide: true //hide uuid
    },
    { 
      field: 'employeeId', 
      headerName: 'Employee ID',
      width: 130,
      cellRenderer: params => (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <PersonIcon sx={{ fontSize: 20, color: 'primary.main' }} />
          {params.value}
        </Box>
      )
    },
    { 
      field: 'name', 
      headerName: 'Name', 
      flex: 1,
      cellRenderer: params => (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          {params.value}
        </Box>
      )
    },
    { 
      field: 'emailAddress',
      headerName: 'Email', 
      flex: 1,
      cellRenderer: params => (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <EmailIcon sx={{ fontSize: 20, color: 'primary.main' }} />
          {params.value}
        </Box>
      )
    },
    { 
      field: 'phoneNumber',
      headerName: 'Phone', 
      width: 150,
      cellRenderer: params => (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <PhoneIcon sx={{ fontSize: 20, color: 'primary.main' }} />
          {params.value}
        </Box>
      )
    },
    { 
      field: 'daysWorked',
      headerName: 'Days Worked', 
      width: 130,
      sort: 'desc',
      cellRenderer: params => (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <WorkIcon sx={{ fontSize: 20, color: 'primary.main' }} />
          {params.value}
        </Box>
      )
    },
    { 
      field: 'cafeName',
      headerName: 'Cafe', 
      flex: 1,
      cellRenderer: params => (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <HomeIcon sx={{ fontSize: 20, color: 'primary.main' }} />
          {params.value || 'Unassigned'}
        </Box>
      )
    },
    { 
      field: 'gender',
      headerName: 'Gender',
      width: 120,
      cellRenderer: params => (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          {getGenderLabel(params.value)}
        </Box>
      )
    },
    {
      headerName: 'Actions',
      width: 120,
      cellRenderer: params => (
        <Box sx={{ display: 'flex', gap: 1 }}>
          <Tooltip title="Edit Employee">
            <IconButton
              size="small"
              onClick={() => {
                console.log('Editing employee with ID:', params.data.id); 
                const id = params.data.id;
                console.log('Navigating to edit employee:', id);
                navigate({
                  to: '/employee/edit/$id',
                  params: { id }
                });
              }}
            >
              <EditIcon />
            </IconButton>
          </Tooltip>
          <Tooltip title="Delete Employee">
            <IconButton
              size="small"
              color="error"
              onClick={() => setDeleteId(params.data.id)}
            >
              <DeleteIcon />
            </IconButton>
          </Tooltip>
        </Box>
      )
    }
];

  return (
    <Box>
      {/* Success Alert */}
      <Fade in={showSuccessAlert}>
        <Alert 
          severity="success" 
          sx={{ 
            position: 'fixed', 
            top: 20, 
            right: 20, 
            zIndex: 1000 
          }}
        >
          Employee deleted successfully
        </Alert>
      </Fade>

      <Paper sx={{ p: 3, mb: 3 }}>
        {/* Breadcrumbs */}
        <Breadcrumbs sx={{ mb: 2 }}>
          <Link
            component="button"
            onClick={() => navigate({ to: '/' })}
            sx={{ 
              display: 'flex', 
              alignItems: 'center',
              gap: 0.5,
              textDecoration: 'none'
            }}
          >
            <HomeIcon sx={{ fontSize: 20 }} />
            Cafes
          </Link>
          <Typography color="text.primary" sx={{ display: 'flex', alignItems: 'center' }}>
            <PersonIcon sx={{ mr: 0.5 }} />
            {selectedCafe ? `${selectedCafeName} Employees` : 'All Employees'}
          </Typography>
        </Breadcrumbs>

        <Box sx={{ 
          display: 'flex', 
          justifyContent: 'space-between', 
          alignItems: 'center',
          mb: 3
        }}>
          <Typography variant="h4">
            {selectedCafe ? `${selectedCafeName} Employees` : 'All Employees'}
          </Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={() => navigate({ to: '/employee/add' })}
          >
            Add New Employee
          </Button>
        </Box>

        <TextField
          fullWidth
          label="Filter by Name"
          variant="outlined"
          value={nameFilter}
          onChange={(e) => setNameFilter(e.target.value)}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <SearchIcon />
              </InputAdornment>
            ),
            endAdornment: nameFilter && (
              <InputAdornment position="end">
                <IconButton size="small" onClick={handleClearFilter}>
                  <ClearIcon />
                </IconButton>
              </InputAdornment>
            )
          }}
          sx={{ mb: 2 }}
        />

        {isLoading ? (
          <Typography>Loading...</Typography>
        ) : filteredEmployees.length === 0 ? (
          <Alert severity="info">
            {nameFilter 
              ? `No employees found with name "${nameFilter}"`
              : selectedCafe 
                ? `No employees found in ${selectedCafe}`
                : 'No employees available'}
          </Alert>
        ) : (
          <div className="ag-theme-material" style={{ height: 600, width: '100%' }}>
            <AgGridReact
              rowData={filteredEmployees}
              columnDefs={columnDefs}
              defaultColDef={{
                sortable: true,
                filter: true,
                resizable: true
              }}
              pagination={true}
              paginationPageSize={10}
              domLayout='autoHeight'
            />
          </div>
        )}
      </Paper>

      {/* Delete Confirmation Dialog */}
      <Dialog 
        open={!!deleteId} 
        onClose={() => setDeleteId(null)}
        maxWidth="xs"
        fullWidth
      >
        <DialogTitle>Confirm Delete</DialogTitle>
        <DialogContent>
          Are you sure you want to delete this employee?
        </DialogContent>
        <DialogActions>
  <Button 
    onClick={() => setDeleteId(null)} 
    disabled={isDeleting}
  >
    Cancel
  </Button>
  <Button 
    onClick={handleDelete} 
    color="error" 
    variant="contained"
    disabled={isDeleting}
  >
    {isDeleting ? 'Deleting...' : 'Delete'}
  </Button>
</DialogActions>
      </Dialog>
    </Box>
  );
}