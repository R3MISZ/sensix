import { useState, useEffect } from "react";
import type { Device, Sensor } from "../../types";

interface Props {
  devices_len: number;
  sensors_len: number;
  isDemoActive: boolean;
  setIsDemoActive: () => void;
}

export const Header = (props: Props) => {
  const [now, setTime] = useState(new Date());

  useEffect(() => {
    const now_interval_id = setInterval(() => setTime(new Date()), 1000);

    return () => clearInterval(now_interval_id);
  }, []);

  const dateString = now.toLocaleDateString('de-DE', {day: '2-digit', month: '2-digit', year: 'numeric'})
  const timeString = now.toLocaleTimeString('de-DE', {hour: '2-digit', minute: '2-digit', second: '2-digit'})

  return (
    <header className="topbar">
      <div className="brand">
        <div className="brandTitle">Sensix Dashboard</div>
        <div className="dotSep">|</div>
        <div className="topbarDate">{dateString}</div>
        <div className="dotSep">|</div>
        <div className="topbarTime">{timeString}</div>
        <div className="dotSep">|</div>
        <div className="topbarTime">Devices: {props.devices_len}</div>
        <div className="dotSep">|</div>
        <div className="topbarTime">Sensors: {props.sensors_len}</div>
      </div>

      <div className="topbarControls">
        <button className="btn ghost" onClick={props.setIsDemoActive}>{props.isDemoActive ? "Disable" : "Enable"} Demo</button>
      </div>
    </header>
  );
};