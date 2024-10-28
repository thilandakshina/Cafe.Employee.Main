const BASE_URL = 'https://localhost:5002/api';

export const employeeApi = {

  getEmployees: async (cafeId) => {
    try {
      console.log('cscscsc');

      const url = `${BASE_URL}/Employee${cafeId ? `?cafeId=${cafeId}` : ''}`;
      const response = await fetch(url);
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const result = await response.json();
      return result.data;
    } catch (error) {
      console.error('Failed to fetch employees:', error);
      throw error;
    }
  },

  getEmployee: async (id) => {
    try {
      console.log('cscscsc');
      const response = await fetch(`${BASE_URL}/Employee/${id}`);
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const result = await response.json();
      return result.data;
    } catch (error) {
      console.error('Failed to fetch employee:', error);
      throw error;
    }
  },

  updateEmployee: async (id, employeeData) => {
    try {
      const response = await fetch(`${BASE_URL}/Employee/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          ...employeeData,
          gender: parseInt(employeeData.gender) // Ensure gender is sent as number
        })
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Failed to update employee');
      }

      return response.json();
    } catch (error) {
      console.error('Failed to update employee:', error);
      throw error;
    }
  },

  createEmployee: async (employeeData) => {
      try {
        // Log the data being sent
        console.log('Sending employee data:', {
          name: employeeData.name,
          emailAddress: employeeData.emailAddress,
          phoneNumber: employeeData.phoneNumber,
          gender: parseInt(employeeData.gender),
          cafeId: employeeData.cafeId || null
        });
  
        const response = await fetch(`${BASE_URL}/Employee`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            name: employeeData.name,
            emailAddress: employeeData.emailAddress,
            phoneNumber: employeeData.phoneNumber,
            gender: parseInt(employeeData.gender),
            cafeId: employeeData.cafeId || null
          })
        });
  
        if (!response.ok) {
          const errorData = await response.json();
          console.error('Server error response:', errorData);
          throw new Error(errorData.message || 'Failed to create employee');
        }
  
        const result = await response.json();
        console.log('Success response:', result);
        return result;
      } catch (error) {
        console.error('Create employee error details:', error);
        throw error;
      }
    },

    deleteEmployee: async (id) => {
    try {
      const response = await fetch(`${BASE_URL}/Employee/${id}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (!response.ok) {
        throw new Error(`Failed to delete employee: ${response.statusText}`);
      }

      return await response.json();
    } catch (error) {
      console.error('Delete employee error:', error);
      throw error;
    }
  },

};