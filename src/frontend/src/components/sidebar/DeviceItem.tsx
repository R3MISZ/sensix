import { useState, type ReactNode} from "react";
import type { Device } from "../../types/Entities";

interface SideBarItemsProps {
  onSelect: () => void;
  device: Device;
  isActive: boolean;
  children: ReactNode;
}

export const DeviceItem = ({ device, isActive, onSelect, children }: SideBarItemsProps) => {
  const [isExpanded, setIsExpanded] = useState(false);

  return (
    <div className="treeNode">
      <div className={`treeDevice ${isActive ? "active" : ""}`}>
        <button className="iconBtn" onClick={() => setIsExpanded(!isExpanded)}>
          {isExpanded ? "▾" : "▸"}
        </button>
        <button className="treeDeviceMain" onClick={onSelect}>
          <span className="treeDeviceName">{device.name}</span>
          <span className={`badge ${device.status === "online" ? "ok" : ""}`}>{device.status}</span>
        </button>
      </div>
      {isExpanded && 
        <div className="treeChildren">
          {children}
        </div>
      }
    </div>
  );
}