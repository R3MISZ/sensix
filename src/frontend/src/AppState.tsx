import  { useState, useEffect, useMemo } from "react";

import type { Measurement, Device, Sensor, CreateDeviceRequest, CreateSensorRequest } from "./types"
import { mock_devices, mock_measurements, mock_sensors } from "./assets/data_mock";

import { deviceService, sensorService, measurementService } from "./api/services";

export function appState() {
  const [isDemoActive, setIsDemoActive] = useState(false);

  const [isAddDeviceOpen, setIsAddDeviceOpen] = useState(false);
  const [isAddSensorOpen, setIsAddSensorOpen] = useState(false);
  const [isAddMeasurementOpen, setIsAddMeasurementOpen] = useState(false);

  const [isModifyDeviceOpen, setIsModifyDeviceOpen] = useState(false);
  const [isModifySensorOpen, setIsModifySensorOpen] = useState(false);

  const [devices, setDevices] = useState<Device[]>([]);
  const [sensors, setSensors] = useState<Sensor[]>([]);
  const [measurements, setMeasurements] = useState<Measurement[]>([]);

  const [selectedDevice, setSelectedDevice] = useState<Device | undefined>(undefined);
  const [selectedSensor, setSelectedSensor] = useState<Sensor | undefined>(undefined); 

  const [loading, setLoading] = useState<boolean>(true);
  const [loadingError, setLoadingError] = useState<boolean>(false);

  const [searchTerm, setSearchTerm] = useState("");

  const filteredDevices = useMemo(() => {
    if (!searchTerm.trim()) return devices;
    
    const term = searchTerm.toLowerCase();
    return devices.filter(device => {
      // Device passt, wenn Name passt...
      const deviceMatches = device.name.toLowerCase().includes(term);
      // ...oder wenn mindestens einer seiner Sensoren passt
      const hasMatchingSensor = sensors.some(s => 
        s.deviceId === device.id && s.name.toLowerCase().includes(term)
      );
      return deviceMatches || hasMatchingSensor;
    });
  }, [devices, sensors, searchTerm]);

  const selectedMeasurements = useMemo(() => {
    if (!selectedSensor) {
      return [];
    }
    else {
      return measurements
        .filter(m => m.sensorId === selectedSensor.id)
        .sort((a, b) => new Date(a.timestampUtc).getTime() - new Date(b.timestampUtc).getTime());
    }
  }, [measurements, selectedSensor]);

  // Load backend data
  useEffect(() => {
    setApiData();
  }, []);

  const setApiData = async () => {
      try {
        setDevices([])
        setSensors([])
        setMeasurements([])

        setLoading(true);
        setLoadingError(false);

        const [api_devices, api_sensors, api_measurements] = await Promise.all([deviceService.getAll(), sensorService.getAll(), measurementService.getAll()]);

        setDevices(api_devices)
        setSensors(api_sensors);
        setMeasurements(api_measurements);
      }
      catch (error) {
        console.error("API Error:", error);
        setLoadingError(true);
      }
      finally {
        setLoading(false);
      }
    };

  const activateDemo = async () => {
    setSelectedDevice(undefined);
    setSelectedSensor(undefined);
    setMeasurements([]);

    if (isDemoActive) {
      setIsDemoActive(false);
      await setApiData();
    } 
    else {
      setIsDemoActive(true);

      // Use mock data
      setDevices(mock_devices);
      setSensors(mock_sensors);
      setMeasurements(mock_measurements);
    }
  }

  const createDevice = async (request: CreateDeviceRequest) => {
    let device: Device;

    if (!isDemoActive) {
      device = await deviceService.create(request);
    }
    else {
      device = {
        ...request,
        id: crypto.randomUUID(),
        createdAtUtc: new Date().toISOString(),
      };
    }

    setDevices((prev) => [...prev, device]);
    setSelectedDevice(device)
  }

  const createSensor = async (request: CreateSensorRequest) => {
    let sensor: Sensor;

    if (!isDemoActive) {
      sensor = await sensorService.create(request);
    }
    else {
      sensor = {
        ...request,
        id: crypto.randomUUID(),
        createdAtUtc: new Date().toISOString(),
      };
    }

    setSensors(prev => [...prev, sensor]);
    setSelectedSensor(sensor)
  }

  const modifyDevice = async (request: Device) => {
    let device: Device;

    if (!isDemoActive) {
      device = await deviceService.put(request);
    }
    else {
      device = request;
    }

    setDevices(prev => prev.map(d => d.id === request.id ? device : d));
    setSelectedDevice(device);
  }

  const modifySensor = async (request: Sensor) => {
    let sensor: Sensor;

    if (!isDemoActive) {
      sensor = await sensorService.put(request);
    }
    else {
      sensor = request;
    }

    setSensors(prev => prev.map(s => s.id === request.id ? sensor : s));
    setSelectedSensor(sensor);
  }
  
  const deleteDevice = async (id: string) => {
    if (!window.confirm("Delete selected device with sensors?")) return;
    if (!isDemoActive) await deviceService.delete(id);
    
    setDevices(prev => prev.filter(d => d.id !== id));
    setSelectedDevice(undefined);
    
    setSensors(prev => prev.filter(s => s.deviceId !== id));
    setSelectedSensor(undefined);
  };

  const deleteSensor = async (id: string) => {
    if (!window.confirm("Delete selected sensor?")) return;
    if (!isDemoActive) await sensorService.delete(id);

    setSensors(prev => prev.filter(s => s.id !== id));
    setSelectedSensor(undefined);
  }

  return {
    lists: { devices: filteredDevices, sensors, measurements },
    demo: { isDemoActive, activateDemo},
    selected: {
      selectedDevice,
      setSelectedDevice,
      
      selectedSensor,
      setSelectedSensor,

      selectedMeasurements},
    modals: {
      //Device
      isAddDeviceOpen,
      setIsAddDeviceOpen,

      isModifyDeviceOpen,
      setIsModifyDeviceOpen,

      //Sensor
      isAddSensorOpen,
      setIsAddSensorOpen,

      isModifySensorOpen,
      setIsModifySensorOpen,

      //Measurement
      isAddMeasurementOpen,
      setIsAddMeasurementOpen,
    },
    actions: {
      setApiData,
      createDevice,
      modifyDevice,
      deleteDevice,
      createSensor,
      modifySensor,
      deleteSensor,
      setSelectedDevice,
      setSelectedSensor,
    },
    state: {
      loading,
      loadingError
    },
    search: {
      searchTerm,
      setSearchTerm
    }
  };
}