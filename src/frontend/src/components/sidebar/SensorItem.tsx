import type { Sensor } from "../../types/Entities";

interface SideBarSensorProps {
  onSelect: () => void;
  sensor: Sensor;
  isActive: boolean;
}

export const SensorItem = ({ sensor, isActive, onSelect }: SideBarSensorProps) => {
  return (
    <button
      key={sensor.id}
      className={`treeSensor ${isActive ? "active" : ""}`}
      title={`${sensor.name} (${sensor.id})`}
      onClick={onSelect}
    >
      <span className={`miniBadge ${false ? "bad" : "good"}`}>{false ? "!" : "•"}</span>
      <span className="treeSensorName">{sensor.name}</span>
      {/*<span className="miniUnit">{sensor.unit || "—"}</span>*/}
    </button>
  );
}