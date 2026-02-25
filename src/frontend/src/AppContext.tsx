import { useState, useEffect, useMemo} from "react";

import type { Measurement, Device, Sensor, CreateDeviceRequest, CreateSensorRequest } from "./types"
import { mock_devices, mock_measurements, mock_sensors } from "./assets/data_mock";

import { Header } from "./components/header";
import { SideBar, DeviceItem, SensorItem, AddDeviceModal } from "./components/sidebar";
import { 
  DevicePanel,
  SensorPanel,
  ValuesPanel,
  //ControlPanel,
  LineChartPanel,
  AddSensorModal,
  AddMeasurementModal,
  ModifyDeviceModal
} from "./components/content-panel";

import { ModifySensorModal } from "./components/content-panel/modals/ModifySensorModal";
import { deviceService, sensorService, measurementService } from "./api/services";

export function useAppLogic() {
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
    setSelectedDevice(undefined)
    setSelectedSensor(undefined)

    if (isDemoActive) {
      setIsDemoActive(false);
      await setApiData();
    } 
    else {
      setIsDemoActive(true);

      // Use mock data
      setDevices(mock_devices)
      setSensors(mock_sensors)
      setMeasurements(mock_measurements)
    }
  }

  const deleteDevice = async (id: string) => {
    if (!window.confirm("Wirklich löschen?")) return;
    if (!isDemoActive) await deviceService.delete(id);
    setDevices(prev => prev.filter(d => d.id !== id));
    setSelectedDevice(undefined);
  };

  return {
    state: { devices, sensors, selectedDevice, selectedSensor, isDemoActive },
    actions: { setApiData, deleteDevice, setSelectedDevice, setSelectedSensor, activateDemo }
  };
}