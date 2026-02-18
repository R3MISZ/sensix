export interface Device {
  createdAt?:string;
  id:string;
  name:string;
  status:string;
  location?:string;
  sensor_id?:string;
}

export interface Sensor {
  createdAt?:string;
  id:string;
  device_id:string;
  name:string;
  status:string;
  unit:string;
}

export interface Measurement {
  createdAt?:string;
  id:string;
  sensor_id:string;
  timeStamp: number;
  value:number;
}