import "./App.css";

import { useState } from "react";

import type { Device, Sensor, CreateDeviceRequest, CreateSensorRequest } from "./types"

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

import { EditSensorModal } from "./components/content-panel/modals/ModifySensorModal";
import { appState } from "./AppState";

export const FootPanel: React.FC = () => {
  return (
  <div className="footnote">
    <b>Demo notes:</b> Delete uses confirm(). Data is regenerated randomly. Simulation appends points. Backend bind later: replace series generator with API calls.
  </div>);
}

export default function App() {

const ModalEnum = {
  NONE: 0,
  ADD_DEVICE: 1,
  MODIFY_DEVICE: 2,
  ADD_SENSOR: 3,
  MODIFY_SENSOR: 4,
  ADD_MEASUREMENT: 5,
}

const closeModal = () => {
  setModal(ModalEnum.NONE)
}

const [modal, setModal] = useState<number>(ModalEnum.NONE);

const { lists, demo, actions, selected,state } = appState();

const displayFailedToLoad = () => {
  return (
    <div>
      <div className="topbarDate">Failed to Load</div>
      <button className="btn retry" onClick={actions.setApiData}>Retry</button>
    </div>
  );
}

const displayListDevices = () => {
  return (
    lists.devices.map(device => {
      
      const deviceSensors = lists.sensors.filter(s => s.deviceId === device.id);

      return (
        <DeviceItem
          key={device.id}
          device={device}
          isActive={selected.selectedDevice?.id === device.id}
          onSelect={() => {
            selected.setSelectedDevice(device);
            selected.setSelectedSensor(undefined);
          }}
        > { deviceSensors.length > 0 ? deviceSensors.map(sensor => (
            <SensorItem
              key={sensor.id}
              sensor={sensor}
              isActive={selected.selectedSensor?.id === sensor.id}
              onSelect={() => {
                selected.setSelectedSensor(sensor);
                selected.setSelectedDevice(device);
              } }
            />
            )) : <div className="emptyHint">No items added yet</div>}
        </DeviceItem>
      );
    })
  );
}

  return (
    <div className="appShell">
      <Header 
        devicesCount={lists.devices.length}
        sensorsCount={lists.sensors.length}
        isDemoActive={demo.isDemoActive}
        onDemoClick={demo.activateDemo}/>

      <div className="contentGrid">
        {/*<SideBar onAddClick={() => setIsAddDeviceOpen(true)}> */}
        <SideBar onAddClick={() => setModal(ModalEnum.ADD_DEVICE)}>
          {!demo.isDemoActive && state.loading && <div className="statusHint">Loading data...</div>}
          {!demo.isDemoActive && state.loadingError && displayFailedToLoad()}
          {(demo.isDemoActive || (!state.loading && !state.loadingError)) && displayListDevices()}
        </SideBar> 

        <main className="main">
          <DevicePanel
              device={selected.selectedDevice}
              onAddClick={() => setModal(ModalEnum.ADD_SENSOR)}
              onModifyClick={() => setModal(ModalEnum.MODIFY_DEVICE)}
              onDeleteClick={async () => {if (selected.selectedDevice?.id) actions.deleteDevice(selected.selectedDevice.id)}}
            />

          <SensorPanel 
            sensor={selected.selectedSensor}
            onAddClick={() => setModal(ModalEnum.ADD_MEASUREMENT)}
            onModifyClick={() => setModal(ModalEnum.MODIFY_SENSOR)}
            onDeleteClick={async () => {if (selected.selectedSensor?.id) actions.deleteSensor(selected.selectedSensor.id)}}
          />

          <ValuesPanel
            data={selected.selectedSensor ? selected.selectedMeasurements : []}
            unit={selected.selectedSensor?.unit}
          />

          {/*<ControlPanel />*/}

          <LineChartPanel
            data={selected.selectedMeasurements}
            unit={selected.selectedSensor?.unit}
          />

          {/*<FootPanel />*/}
        </main>
      </div>
      <AddDeviceModal
        isOpen={modal === ModalEnum.ADD_DEVICE}
        onSave={(request: CreateDeviceRequest) => actions.createDevice(request)}
        onClose={closeModal}
      />
      <AddSensorModal
        isOpen={modal === ModalEnum.ADD_SENSOR}
        selectedDeviceId={selected.selectedDevice?.id}
        onSave={async (request: CreateSensorRequest) => actions.createSensor(request)}
        onClose={closeModal}
      />
      <AddMeasurementModal
        isOpen={modal === ModalEnum.ADD_MEASUREMENT}
        onClose={closeModal}
      />
      <ModifyDeviceModal
        isOpen={modal === ModalEnum.MODIFY_DEVICE}
        selectedDevice={selected.selectedDevice}
        onSave={(request: Device) => actions.modifyDevice(request)}
        onClose={closeModal}
      />

      <EditSensorModal
        isOpen={modal === ModalEnum.MODIFY_SENSOR}
        selectedSensor={selected.selectedSensor}
        onSave={(request: Sensor) => actions.modifySensor(request)}
        onClose={closeModal}
      />
    </div>
  );
}