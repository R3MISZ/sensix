export interface Device {
  createdAtUtc:number;
  id:string;
  name:string;
  isActive:boolean;
  location?:string;
  sensor_id?:string;
}

export interface Sensor {
  createdAtUtc:number;
  id:string;
  deviceId:string;
  name:string;
  isActive:boolean;
  unit:string;
  type:string;
}

export interface Measurement {
  timestampUtc:number;
  id:string;
  sensorId:string;
  value:number;
}