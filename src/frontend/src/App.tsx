import { useState, useEffect, useMemo} from "react";
import "./App.css";

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
import { useAppLogic } from "./AppContext";

export const FootPanel: React.FC = () => {
  return (
  <div className="footnote">
    <b>Demo notes:</b> Delete uses confirm(). Data is regenerated randomly. Simulation appends points. Backend bind later: replace series generator with API calls.
  </div>);
}

export default function App() {

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
    loadData();
  }, []);

  const loadData = async () => {
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

  const useMockData = () => {
    setDevices(mock_devices)
    setSensors(mock_sensors)
    setMeasurements(mock_measurements)
  }

  /*
  useEffect(() => {
    console.log("--- Devices State ---");
    console.table(devices);
    console.log("--- Sensors State ---");
    console.table(sensors);
  }, [devices, sensors]);
  */

  return (
    <div className="appShell">
      <Header 
        devicesCount={devices.length}
        sensorsCount={sensors.length}
        isDemoActive={isDemoActive}
        onDemoClick={async () => {
          setSelectedDevice(undefined)
          setSelectedSensor(undefined)

          if (isDemoActive) {
            setIsDemoActive(false);
            await loadData();
          } 
          else {
            setIsDemoActive(true);
            useMockData();
          }
        }}/>

      <div className="contentGrid">
        {/*<SideBar onAddClick={() => setIsAddDeviceOpen(true)}> */}
        <SideBar onAddClick={() => setIsAddDeviceOpen(true)}>
          {/* Demo NOT active and using api*/}
          {!isDemoActive && loading && <div className="statusHint">Loading data...</div>}
          {!isDemoActive && loadingError && 
          <div>
            <div className="topbarDate">Failed to Load</div>
            <button className="btn retry" onClick={loadData}>Retry</button>
          </div>}

          {/* Demo ACTIVE */}
          {(isDemoActive || (!loading && !loadingError)) &&
            devices.map(device => {

            const deviceSensors = sensors.filter(s => s.deviceId === device.id);

            return (
              <DeviceItem
                key={device.id}
                device={device}
                isActive={selectedDevice?.id === device.id}
                onSelect={() => {
                  setSelectedDevice(device);
                  setSelectedSensor(undefined);
                }}
              > { deviceSensors.length > 0 ? deviceSensors.map(sensor => (
                  <SensorItem
                    key={sensor.id}
                    sensor={sensor}
                    isActive={selectedSensor?.id === sensor.id}
                    onSelect={() => {
                      setSelectedSensor(sensor);
                      setSelectedDevice(device);
                    } }
                  />
                  )) : <div className="emptyHint">No items added yet</div>}
              </DeviceItem>
            );
        })}
        </SideBar> 

        <main className="main">
          <DevicePanel
              device={selectedDevice}
              onAddClick={() => setIsAddSensorOpen(true)}
              onModifyClick={() => setIsModifyDeviceOpen(true)}
              onDeleteClick={async () => {
                if (!selectedDevice) return;

                const confirmMessage = `Remove Device "${selectedDevice.name}" and all related Sensors?`;
                if (!window.confirm(confirmMessage)) return;

                const idToDelete = selectedDevice.id;

                try {
                  if (!isDemoActive) await deviceService.delete(idToDelete);

                  setSensors(prev => prev.filter(s => s.deviceId !== idToDelete));
                  setDevices(prev => prev.filter(d => d.id !== idToDelete));

                  setSelectedDevice(undefined);
                  setSelectedSensor(undefined);
                }
                catch (error) {
                  alert(`Error: ${error}`);
                }
              }}
            />

          <SensorPanel 
            sensor={selectedSensor}
            onAddClick={() => setIsAddMeasurementOpen(true)}
            onModifyClick={() => setIsModifySensorOpen(true)}
            onDeleteClick={async () => {
              if (!selectedSensor) return;

              const confirmMessage = `Remove Sensor "${selectedSensor.name}" ?`
              if (!window.confirm(confirmMessage)) return;

              const idToDelete = selectedSensor.id;

              try {
                if (!isDemoActive) {
                  await sensorService.delete(idToDelete);
                }

                setSensors(prev => prev.filter(s => s.id !== idToDelete));
                setSelectedSensor(undefined);
              }
              catch (error) {
                alert(`Error: ${error}`);
              }
            }}
          />

          <ValuesPanel
            data={selectedMeasurements}
            unit={selectedSensor?.unit}
          />

          {/*<ControlPanel />*/}

          <LineChartPanel
            data={selectedMeasurements}
            unit={selectedSensor?.unit}
          />

          {/*<FootPanel />*/}
        </main>
      </div>
      <AddDeviceModal
        isOpen={isAddDeviceOpen}
        onClose={() => setIsAddDeviceOpen(false)}
        onSave={async (newDevice: CreateDeviceRequest) => {
          try {
            let savedDevice: Device;

            if (!isDemoActive) {
              savedDevice = await deviceService.create(newDevice);
            }
            else {
              savedDevice = {
                ...newDevice,
                id: crypto.randomUUID(),
                createdAtUtc: new Date().toISOString(),
              };
            }

            setDevices((prev) => [...prev, savedDevice]);
            setSelectedDevice(savedDevice)
          }
          catch (error) {
            window.alert(`Error: ${error}`);
          }
        }}
      />
      <AddSensorModal
        selectedDeviceId={selectedDevice?.id}
        isOpen={isAddSensorOpen}
        onSave={async (newSensor:CreateSensorRequest) => {
          try {
            let savedSensor: Sensor;

            if (!isDemoActive) {
              savedSensor = await sensorService.create(newSensor);
            }
            else {
              savedSensor = {
                ...newSensor,
                id: crypto.randomUUID(),
                createdAtUtc: new Date().toISOString(),
              };
            }

            setSensors((prev) => [...prev, savedSensor]);
            setSelectedSensor(savedSensor)
          }
          catch (error) {
            window.alert(`Error: ${error}`);
          }
        }}
        onClose={() => setIsAddSensorOpen(false)}
      />
      <AddMeasurementModal
        isOpen={isAddMeasurementOpen}
        onClose={() => setIsAddMeasurementOpen(false)}
      />
      <ModifyDeviceModal
        selectedDevice={selectedDevice}
        isOpen={isModifyDeviceOpen}
        onSave={(updatedDevice: Device) => {

          setDevices(prev => prev.map(d => d.id === updatedDevice.id ? updatedDevice : d));
          setSelectedDevice(updatedDevice);
        }}
        onClose={() => setIsModifyDeviceOpen(false)}
      />

      <ModifySensorModal
        selectedSensor={selectedSensor}
        isOpen={isModifySensorOpen}
        onSave={async (updatedSensor: Sensor) => {
          try {
            let savedSensor: Sensor;

            if (!isDemoActive) {
              savedSensor = await sensorService.put(updatedSensor);
            }
            else {
              savedSensor = updatedSensor;
            }

            setSensors(prev => prev.map(s => s.id === updatedSensor.id ? updatedSensor : s));
            setSelectedSensor(savedSensor);
          }
          catch (error) {
            window.alert(`Error: ${error}`);
          }
        }}
        onClose={() => setIsModifySensorOpen(false)}
      />
    </div>
  );
}