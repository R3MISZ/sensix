import { http } from "./http";

export type MeasurementDto = {
  id: string;
  sensorId: string;
  timestampUtc: string;
  value: number;
};

export function getMeasurements() {
  return http<MeasurementDto[]>("/Measurements");
}