import { useState, useEffect } from "react";

interface Props {
  devicesCount: number;
  sensorsCount: number;
  isDemoActive: boolean;
  onDemoClick: () => void;
}

export const Header = ({ devicesCount, sensorsCount, isDemoActive, onDemoClick }: Props) => {
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
        <div className="topbarTime">Devices: {devicesCount}</div>
        <div className="dotSep">|</div>
        <div className="topbarTime">Sensors: {sensorsCount}</div>
      </div>

      <div className="topbarControls">
        <button className="btn ghost" onClick={onDemoClick}>{isDemoActive ? "Connect Server" : "Connect Demo"}</button>
      </div>
    </header>
  );
};