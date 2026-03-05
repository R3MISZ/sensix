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
  onSearchChange: (value: string) => void;
  searchValue: string;
  children: ReactNode;
}

export const SideBar = ({ onAddClick, onSearchChange, searchValue, children }: SideBarProps) => {

  return (
    <aside className="sidebar">
      <div className="sidebarHeader">
        <div className="sidebarTitle">Devices</div>
        <button className="btn small" onClick={onAddClick}>+ Add Device</button>
      </div>
      <div className="searchBox">
        <div className="searchWrapper">
          <input className="input" placeholder="Search devices / sensors..." value={searchValue} onChange={(e) => onSearchChange(e.target.value)}/>
          {searchValue && (<button className="clearButton" onClick={() => onSearchChange("")} aria-label="Clear search">✕</button>)}
        </div>
      </div>
      <div className="tree">
        {children || <div className="emptyHint">No items added yet</div>}
      </div>
    </aside>
  );
};