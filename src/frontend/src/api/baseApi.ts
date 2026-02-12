const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;
const API_PREFIX = "/api";

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

// #region Connection URL
export async function http<T>(url: string): Promise<T> {
  const fullUrl = `${API_BASE_URL}${API_PREFIX}${url}`;
  const response = await fetch(fullUrl);

  if (!response.ok) {
    throw new Error(`HTTP ${response.status} @ ${fullUrl}: ${await response.text()}`);
  }

  return response.json();
}
// #endregion

// #region Measurements
export function getMeasurements() {
  return http<MeasurementDto[]>("/Measurements");
}
// #endregion

// #region Sensors
export function getSensors() {
  return http<SensorDto[]>("/Sensors");
}

export function getSensorById(id: string) {
  return http<SensorDto>(`/Sensors/${id}`);
}
// #endregion