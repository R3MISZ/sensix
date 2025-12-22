const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;
const API_PREFIX = "/api";

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
  const response = await fetch(`${API_BASE_URL}${API_PREFIX}${url}`);

  if (!response.ok) {
    throw new Error(await response.text());
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