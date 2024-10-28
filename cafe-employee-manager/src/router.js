import { 
  Route,
  RootRoute,
  Router,
} from '@tanstack/react-router';
import App from './App';
import { CafesPage } from './pages/CafesPage';
import { EmployeesPage } from './pages/EmployeesPage';
import { AddEditCafePage } from './pages/AddEditCafePage';
import { AddEditEmployeePage } from './pages/AddEditEmployeePage';

// Root route
const rootRoute = new RootRoute({
  component: App,
});

const indexRoute = new Route({
  getParentRoute: () => rootRoute,
  path: '/',
  component: CafesPage,
});

//emp
const employeesRoute = new Route({
  getParentRoute: () => rootRoute,
  path: '/employees',
  component: EmployeesPage,
});

const employeeEditRoute = new Route({
  getParentRoute: () => rootRoute,
  path: '/employee/edit/$id',  // Using $id for TanStack Router
  component: AddEditEmployeePage,
});

const employeeAddRoute = new Route({
  getParentRoute: () => rootRoute,
  path: '/employee/add',
  component: AddEditEmployeePage,
});

//cafe
const addCafeRoute = new Route({
  getParentRoute: () => rootRoute,
  path: '/cafe/add',
  component: AddEditCafePage,
});

const editCafeRoute = new Route({
  getParentRoute: () => rootRoute,
  path: '/cafe/edit/$id',
  component: AddEditCafePage,
});


const routeTree = rootRoute.addChildren([
  indexRoute,
  employeesRoute,
  employeeAddRoute,
  employeeEditRoute, 
  addCafeRoute,
  editCafeRoute,
]);

const router = new Router({
  routeTree,
  defaultPreload: 'intent',
});

export default router;