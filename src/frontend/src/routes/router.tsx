import { createBrowserRouter } from "react-router-dom";

import { DashboardPage } from "../pages/DashboardPage";
import { SensorPage } from "../pages/SensorPage";

export const router = createBrowserRouter([
  { path: "/", element: <DashboardPage/> },
  { path: "/sensors/:id", element: <SensorPage/> },
]);