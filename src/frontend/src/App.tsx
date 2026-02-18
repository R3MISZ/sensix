import { useState, useEffect, useMemo} from "react";
import "./App.css";

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
  const [isAddDeviceOpen, setIsAddDeviceOpen] = useState(false);
  const [isAddSensorOpen, setIsAddSensorOpen] = useState(false);
  const [isAddMeasurementOpen, setIsAddMeasurementOpen] = useState(false);

  const [isModifyDeviceOpen, setIsModifyDeviceOpen] = useState(false);

  const [devices, setDevices] = useState<Device[]>(mock_devices);
  const [sensors, setSensors] = useState<Sensor[]>(mock_sensors);
  const [measurements, setMeasurements] = useState<Measurement[]>(mock_measurements);

  const [selectedDevice, setSelectedDevice] = useState<Device | undefined>(undefined);
  const [selectedSensor, setSelectedSensor] = useState<Sensor | undefined>(undefined); 

  const selectedMeasurements = useMemo(() => {
    if (!selectedSensor) return [];

    return measurements
      .filter(m => m.sensor_id === selectedSensor.id)
      .sort((a, b) => a.timeStamp - b.timeStamp);
  }, [measurements, selectedSensor]);

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
      <Header devices_len={devices.length} sensors_len={sensors.length}  />

      <div className="contentGrid">
        
        <SideBar onAddClick={() => setIsAddDeviceOpen(true)}> {
          devices.map(device => {

          const deviceSensors = sensors.filter((s) => s.device_id === device.id);

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
            onDeleteClick={() => {
                if (!selectedDevice) return;

                if (window.confirm(`Remove Device "${selectedDevice.name}" and all related Sensors?`)) {
                  const idToDelete = selectedDevice.id;

                  setSensors(prev => prev.filter(s => s.device_id !== idToDelete));
                  setDevices(prev => prev.filter(d => d.id !== idToDelete));

                  setSelectedDevice(undefined);
                  setSelectedSensor(undefined);
                }
              }}
            />

          <SensorPanel 
            sensor={selectedSensor}
            onAddClick={() => setIsAddMeasurementOpen(true)}
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