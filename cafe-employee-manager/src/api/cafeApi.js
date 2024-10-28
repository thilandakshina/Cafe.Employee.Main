import axios from 'axios';

const BASE_URL = 'https://localhost:5002/api';

const axiosInstance = axios.create({
  baseURL: BASE_URL,
});

const convertFileToBase64 = (file) => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = (error) => reject(error);
  });
};

const handleApiResponse = (response) => {
  if (response?.data) {
    if (response.data.success && response.data.data) {
      return response.data.data;
    }
    if (response.data.success) {
      return response.data;
    }
    if (response.data.message) {
      throw new Error(response.data.message);
    }
  }
  return response.data;
};

const cafeApi = {
  getCafes: async (location = '') => {
    try {
      const response = await axiosInstance.get('/Cafe', {
        params: { location }
      });
      return handleApiResponse(response);
    } catch (error) {
      console.error('Get cafes error:', error);
      throw new Error('Failed to fetch cafes');
    }
  },

  getCafe: async (id) => {
    try {
      console.log('Getting cafe:', id);
      const response = await axiosInstance.get(`/Cafe/${id}`);
      return handleApiResponse(response);
    } catch (error) {
      console.error('Get cafe error:', error);
      throw new Error('Failed to fetch cafe');
    }
  },

  createCafe: async (data) => {
    try {
      const requestData = {
        name: data.name,
        description: data.description,
        location: data.location,
      };

      if (data.logo instanceof File) {
        const base64Logo = await convertFileToBase64(data.logo);
        requestData.logo = base64Logo.split(',')[1];
      }

      const response = await axiosInstance.post('/Cafe', requestData);
      return handleApiResponse(response);
    } catch (error) {
      console.error('Create cafe error:', error);
      throw new Error('Failed to create cafe');
    }
  },

  updateCafe: async (id, data) => {
    try {
      console.log('Updating cafe:', id, data);
      
      const requestData = {
        id: id,
        name: data.name,
        description: data.description,
        location: data.location,
      };

      if (data.logo instanceof File) {
        const base64Logo = await convertFileToBase64(data.logo);
        requestData.logo = base64Logo.split(',')[1];
      }

      console.log('Update request data:', requestData);

      const response = await axiosInstance.put(`/Cafe/${id}`, requestData);
      return handleApiResponse(response);
    } catch (error) {
      console.error('Update cafe error:', error);
      throw new Error(error.response?.data?.message || 'Failed to update cafe');
    }
  },

  deleteCafe: async (id) => {
    try {
      const response = await axiosInstance.delete(`/Cafe/${id}`);
      return handleApiResponse(response);
    } catch (error) {
      console.error('Delete cafe error:', error);
      throw new Error('Failed to delete cafe');
    }
  }
};

export { cafeApi };