export interface Device {
  createdAtUtc:string;
  id:string;
  name:string;
  isActive:boolean;
  location?:string;
  sensor_id?:string;
}

export interface Sensor {
  createdAtUtc:string;
  id:string;
  deviceId:string;
  name:string;
  isActive:boolean;
  unit:string;
  type:string;
}

export interface Measurement {
  timestampUtc:string;
  id:string;
  sensorId:string;
  value:number;
}

export interface CreateDeviceRequest {
  name: string;
  location: string;
  isActive: boolean;
}

export interface CreateSensorRequest {
  deviceId: string;
  name: string;
  type: string;
  unit: string;
  isActive: boolean;
}

export interface CreateMeasurementRequest {
  sensorid:string;
  value: number;
  timestampUtc:number;
}

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