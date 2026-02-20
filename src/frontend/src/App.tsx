import { useState, useEffect, useMemo} from "react";
import "./App.css";

import { api, API_URL } from "./api/baseApi";

import type { Measurement, Device, Sensor } from "./types"
import { mock_devices, mock_measurements, mock_sensors } from "./assets/data_mock";

import { Header } from "./components/header";
import { SideBar, DeviceItem, SensorItem, AddDeviceModal } from "./components/sidebar";
import { 
  DevicePanel,
  SensorPanel,
  ValuesPanel,
  ControlPanel,
  LineChartPanel,
  AddSensorModal,
  AddMeasurementModal,
  ModifyDeviceModal
} from "./components/content-panel";

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

  const [devices, setDevices] = useState<Device[]>([]);
  const [sensors, setSensors] = useState<Sensor[]>([]);
  const [measurements, setMeasurements] = useState<Measurement[]>([]);

  const [selectedDevice, setSelectedDevice] = useState<Device | undefined>(undefined);
  const [selectedSensor, setSelectedSensor] = useState<Sensor | undefined>(undefined); 

  const [loading, setLoading] = useState<boolean>(true);
  const [loadingError, setLoadingError] = useState<boolean>(false);

  const selectedMeasurements = useMemo(() => {
    if (!selectedSensor)
      return [];
    else
      return measurements
        .filter(m => m.sensorId === selectedSensor.id)
        .sort((a, b) => a.timestampUtc - b.timestampUtc);

  }, [measurements, selectedSensor]);

  // Load backend data
  useEffect(() => {
    console.log(`Current API Url: ${API_URL}`)
    loadData();
  }, []);

  const loadData = async () => {
      try {
        setDevices([])
        setSensors([])
        setMeasurements([])

        setLoading(true);
        setLoadingError(false);

        const [api_devices, api_sensors, api_measurements] = await Promise.all([
          api.getDevices(),
          api.getSensors(),
          api.getMeasurements()
        ]);
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
        devices_len={devices.length}
        sensors_len={sensors.length}
        isDemoActive={isDemoActive}
        setIsDemoActive={async () => {
          setSelectedDevice(undefined)
          setSelectedSensor(undefined)

          if (isDemoActive) {
            setIsDemoActive(false);
            await loadData(); // Da loadData async ist, hier await nutzen
          } 
          else {
            setIsDemoActive(true);
            useMockData(); // Falls das synchron ist, kein await nötig
          }
        }}/>

      <div className="contentGrid">
        {/*<SideBar onAddClick={() => setIsAddDeviceOpen(true)}> */}
        <SideBar onAddClick={() => {}}> 
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
              onAddClick={() => {}}
              onModifyClick={() => {}}
              onDeleteClick={() => {}}
            /* TODO
              onAddClick={() => setIsAddSensorOpen(true)}
              onModifyClick={() => setIsModifyDeviceOpen(true)}
              onDeleteClick={() => {
                  if (!selectedDevice) return;

                  if (window.confirm(`Remove Device "${selectedDevice.name}" and all related Sensors?`)) {
                    const idToDelete = selectedDevice.id;

                    setSensors(prev => prev.filter(s => s.deviceId !== idToDelete));
                    setDevices(prev => prev.filter(d => d.id !== idToDelete));

                    setSelectedDevice(undefined);
                    setSelectedSensor(undefined);
                  }
                }}
              */
            />

          <SensorPanel 
            sensor={selectedSensor}
            /*onAddClick={() => setIsAddMeasurementOpen(true)}*/
            onAddClick={() => {}}
            onModifyClick={() => {}}
            onDeleteClick={() => {}}
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
        onSave={(newDevice:Device) => setDevices([...devices, newDevice])}
      />
      <AddSensorModal
        selectedDevice={selectedDevice}
        isOpen={isAddSensorOpen}
        onSave={(newSensor:Sensor) => {
          setSensors([...sensors, newSensor]);
          setSelectedSensor(newSensor);
        }}
        onClose={() => setIsAddSensorOpen(false)}
      />
      <AddMeasurementModal
          isOpen={isAddMeasurementOpen}
          onClose={() => setIsAddMeasurementOpen(false)}
      />
      {/* 
      <ModifyDeviceModal
        onSave={(updatedDevice: Device) => {
          setDevices(prev => 
            prev.map(d => d.id === updatedDevice.id ? updatedDevice : d)
          );
          
          setSelectedDevice(updatedDevice);
        }}
      />
      */}
    </div>
  );
}