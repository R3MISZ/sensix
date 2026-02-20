import { type ReactNode} from "react";

export const EmptyHint = () => {
  return (
    <div className="tree">
      <div className="emptyHint">No items added yet</div>
    </div>
  );
}

interface SideBarProps {
  onAddClick: () => void;
  children: ReactNode;
}

export const SideBar = ({ onAddClick, children }: SideBarProps) => {
  return (
    <aside className="sidebar">
      <div className="sidebarHeader">
        <div className="sidebarTitle">Devices</div>
        <button className="btn small" onClick={onAddClick}>+ Add Device</button>
      </div>
      <div className="searchBox">
        <input className="input" placeholder="Search devices / sensors..." readOnly />
      </div>
      <div className="tree">
        {children || <div className="emptyHint">No items added yet</div>}
      </div>
    </aside>
  );
};

