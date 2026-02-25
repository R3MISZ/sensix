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
    timestampUtc: new Date(now - (count - i) * 600_000).toISOString(),
    value: Number((min + Math.random() * (max - min)).toFixed(1))
  }));
};

export const mock_devices: Device[] = [
  { createdAtUtc: new Date().toISOString(), id: crypto.randomUUID(), name: "Device A", isActive: true, location: "Lab-A"},
  { createdAtUtc: new Date().toISOString(), id: crypto.randomUUID(), name: "Device B", isActive: true, location: "Lab-B" },
  { createdAtUtc: new Date().toISOString(), id: crypto.randomUUID(), name: "Device C", isActive: false, location: "Lab-C"},
  { createdAtUtc: new Date().toISOString(), id: crypto.randomUUID(), name: "Device D", isActive: false, location: "Lab-D"},
]

export const mock_sensors: Sensor[] = [
  { createdAtUtc: new Date().toISOString(), id: crypto.randomUUID(), deviceId: mock_devices[0].id, name: "Sensor A", type: "Temperature",  unit: "°C", isActive: true},
  { createdAtUtc: new Date().toISOString(), id: crypto.randomUUID(), deviceId: mock_devices[0].id, name: "Sensor X", type: "Humidity",  unit: "%", isActive: true},
  { createdAtUtc: new Date().toISOString(), id: crypto.randomUUID(), deviceId: mock_devices[1].id, name: "Sensor B", type: "Humidity",  unit: "%", isActive: true},
  { createdAtUtc: new Date().toISOString(), id: crypto.randomUUID(), deviceId: mock_devices[2].id, name: "Sensor C", type: "Pressure",  unit: "bar", isActive: false},
]

export const mock_measurements: Measurement[] = [
  ...generateMockData(mock_sensors[0].id, 20, 0, 28),
  ...generateMockData(mock_sensors[1].id, 20, 0, 100),
  ...generateMockData(mock_sensors[2].id, 20, 0, 100),
]