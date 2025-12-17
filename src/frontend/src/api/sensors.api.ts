import { http } from "./http";

export type SensorDto = {
  id: string;
  name: string;
  isActive: boolean;
  deviceId?: string | null; // optional, falls du es im Response hast
};

export function getSensors() {
  return http<SensorDto[]>("/Sensors"); // wenn du http.ts mit /api Prefix machst
  // oder: return http<SensorDto[]>("/api/Sensors"); wenn nicht
}

export function getSensorById(id: string) {
  return http<SensorDto>(`/Sensors/${id}`);
}