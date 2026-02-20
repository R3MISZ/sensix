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
    sensorId: sensorId,
    timestampUtc: now - (count - i) * 600_000, 
    value: Number((min + Math.random() * (max - min)).toFixed(1))
  }));
};

export const mock_devices: Device[] = [
    { createdAtUtc: Date.now(), id: "dev_a", name: "Device A", isActive: true, location: "Lab-a"},
    { createdAtUtc: Date.now(), id: "dev_b", name: "Device B", isActive: true, location: "Lab-b" },
    { createdAtUtc: Date.now(), id: "dev_c", name: "Device C", isActive: false, location: "Lab-c"},
    { createdAtUtc: Date.now(), id: "dev_d", name: "Device D", isActive: false, location: "Lab-d"},
  ]

export const mock_sensors: Sensor[] = [
    { createdAtUtc: Date.now(), id: "sen_a", deviceId: "dev_a", name: "Sensor A", type: "Temperature",  unit: "°C", isActive: true},
    { createdAtUtc: Date.now(), id: "sen_x", deviceId: "dev_a", name: "Sensor X", type: "Humidity",  unit: "%", isActive: true},
    { createdAtUtc: Date.now(), id: "sen_b", deviceId: "dev_b", name: "Sensor B", type: "Humidity",  unit: "%", isActive: true},
    { createdAtUtc: Date.now(), id: "sen_c", deviceId: "dev_c", name: "Sensor C", type: "Pressure",  unit: "bar", isActive: false},
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

