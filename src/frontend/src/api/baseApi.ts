
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL; // http://localhost:5000
const API_PREFIX = "/api";
export const API_URL = `${API_BASE_URL}${API_PREFIX}`

console.log("CURRENT API-URL:", API_BASE_URL);

export type MeasurementDto = {
  id: string;
  sensorId: string;
  timestampUtc: string;
  value: number;
};

export type SensorDto = {
  id: string;
  name: string;
  isActive: boolean;
  deviceId?: string | null;
};

// #endregion

export const api = {
  getDevices: async () => {
    const response = await fetch(`${API_URL}/devices`);
    if (!response.ok) throw new Error(`Failed to fetch devices. Response: ${response}`);
    return response.json();
  },

  // Sensors
  getSensors: async () => {
    const response = await fetch(`${API_URL}/sensors`);
    if (!response.ok) throw new Error(`Failed to fetch sensors. Response: ${response}`);
    return response.json();
  },

  // Measurements
  getMeasurements: async () => {
    const response = await fetch(`${API_URL}/measurements`);
    if (!response.ok) throw new Error(`Failed to fetch measurements. Response: ${response}`);
    return response.json();
  },

  // Neues Measurement erstellen (Post)
  createMeasurement: async (data: { sensor_id: string; value: number; timeStamp: number }) => {
    const response = await fetch(`${API_URL}/measurements`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });
    return response.json();
  }
};