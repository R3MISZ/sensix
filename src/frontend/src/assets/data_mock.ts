import type { Device, Sensor, Measurement } from "../types/Entities"

const generateMockData = (
  sensorId: string, 
  count: number, 
  min: number, 
  max: number,
): Measurement[] => {
  const now = Date.now();
  return Array.from({ length: count }).map((_, i) => ({
    id: `mea_${sensorId}_${i}`,
    sensor_id: sensorId,
    timeStamp: now - (count - i) * 600_000, 
    value: Number((min + Math.random() * (max - min)).toFixed(1))
  }));
};

export const mock_devices: Device[] = [
    { id: "dev_a", name: "Device A", status: "online", location: "location-a"},
    { id: "dev_b", name: "Device B", status: "online", location: "location-b" },
    { id: "dev_c", name: "Device C", status: "offline", location: "location-c"},
    { id: "dev_d", name: "Device D", status: "offline", location: "location-d"},
  ]

export const mock_sensors: Sensor[] = [
    { id: "sen_a", device_id: "dev_a", name: "Sensor A", status: "online", unit: "°C"},
    { id: "sen_x", device_id: "dev_a", name: "Sensor X", status: "online", unit: "%"},
    { id: "sen_b", device_id: "dev_b", name: "Sensor B", status: "online", unit: "%" },
    { id: "sen_c", device_id: "dev_c", name: "Sensor C", status: "offline", unit: "bar"},
  ]

export const mock_measurements: Measurement[] = [
    ...generateMockData("sen_a", 20, 0, 28),
    ...generateMockData("sen_b", 20, 0, 100),
    
    /*
    { id: "mea_1", sensor_id: "sen_a", timeStamp: Date.now() - 300_000, value: 21},
    { id: "mea_2", sensor_id: "sen_a", timeStamp: Date.now() - 150_000, value: 15},
    { id: "mea_3", sensor_id: "sen_a", timeStamp: Date.now(), value: 5},

    { id: "mea_4", sensor_id: "sen_b", timeStamp: Date.now(), value: 22},
    { id: "mea_5", sensor_id: "sen_c", timeStamp: Date.now(), value: 23},
     */
  ]

