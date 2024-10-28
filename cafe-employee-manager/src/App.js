import React from 'react';
import { Link, Outlet } from '@tanstack/react-router';
import { 
  AppBar, 
  Toolbar, 
  Typography, 
  Container, 
  Box,
  Button
} from '@mui/material';

function App() {
  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Cafe Employee Manager
          </Typography>
          <Button 
            color="inherit" 
            component={Link}
            to="/"
          >
            Cafes
          </Button>
          <Button 
            color="inherit" 
            component={Link}
            to="/employees"
          >
            Employees
          </Button>
        </Toolbar>
      </AppBar>
      <Container sx={{ mt: 4 }}>
        <Outlet />
      </Container>
    </Box>
  );
}

export default App;