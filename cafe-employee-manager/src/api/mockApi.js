// Mock data
const cafes = [
  {
    id: '123e4567-e89b-12d3-a456-426614174000',
    name: 'Cafe One',
    description: 'A cozy cafe in the heart of the city',
    location: 'Downtown',
    employees: 5,
    logo: 'https://via.placeholder.com/150'
  },
  {
    id: '223e4567-e89b-12d3-a456-426614174001',
    name: 'Cafe Two',
    description: 'Beachfront cafe with amazing views',
    location: 'Beach Road',
    employees: 8,
    logo: 'https://via.placeholder.com/150'
  },
  {
    id: '323e4567-e89b-12d3-a456-426614174002',
    name: 'Cafe Three',
    description: 'Modern cafe with great workspace',
    location: 'Business District',
    employees: 3,
    logo: 'https://via.placeholder.com/150'
  }
];

const employees = [
  {
    id: 'UI1234567',
    name: 'John Smith',
    email_address: 'john@email.com',
    phone_number: '91234567',
    gender: 'Male',
    cafe: 'Cafe One',
    days_worked: 120
  },
  {
    id: 'UI2345678',
    name: 'Sarah Johnson',
    email_address: 'sarah@email.com',
    phone_number: '82345678',
    gender: 'Female',
    cafe: 'Cafe One',
    days_worked: 90
  },
  {
    id: 'UI3456789',
    name: 'Mike Wilson',
    email_address: 'mike@email.com',
    phone_number: '93456789',
    gender: 'Male',
    cafe: 'Cafe Two',
    days_worked: 150
  }
];

// Helper function to simulate API delay
const delay = (ms) => new Promise(resolve => setTimeout(resolve, ms));

export const mockApi = {
  // Cafe endpoints
  getCafes: async (location) => {
    await delay(500);
    if (location) {
      return cafes.filter(cafe => 
        cafe.location.toLowerCase().includes(location.toLowerCase())
      );
    }
    return cafes;
  },

  createCafe: async (cafeData) => {
    await delay(500);
    const newCafe = {
      id: crypto.randomUUID(),
      ...cafeData,
      employees: 0
    };
    cafes.push(newCafe);
    return newCafe;
  },

  updateCafe: async (id, cafeData) => {
    await delay(500);
    const index = cafes.findIndex(cafe => cafe.id === id);
    if (index === -1) throw new Error('Cafe not found');
    cafes[index] = { ...cafes[index], ...cafeData };
    return cafes[index];
  },

  deleteCafe: async (id) => {
    await delay(500);
    const index = cafes.findIndex(cafe => cafe.id === id);
    if (index === -1) throw new Error('Cafe not found');
    cafes.splice(index, 1);
    return true;
  },

  // Employee endpoints
  getEmployees: async (cafe) => {
    await delay(500);
    if (cafe) {
      return employees.filter(emp => emp.cafe === cafe);
    }
    return employees;
  },

  createEmployee: async (employeeData) => {
    await delay(500);
    const newEmployee = {
      id: `UI${Math.random().toString().slice(2, 9)}`,
      ...employeeData,
      days_worked: 0
    };
    employees.push(newEmployee);
    return newEmployee;
  },

  updateEmployee: async (id, employeeData) => {
    await delay(500);
    const index = employees.findIndex(emp => emp.id === id);
    if (index === -1) throw new Error('Employee not found');
    employees[index] = { ...employees[index], ...employeeData };
    return employees[index];
  },

  deleteEmployee: async (id) => {
    await delay(500);
    const index = employees.findIndex(emp => emp.id === id);
    if (index === -1) throw new Error('Employee not found');
    employees.splice(index, 1);
    return true;
  }
};