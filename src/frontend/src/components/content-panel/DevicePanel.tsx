import type { Device } from "../../types/Entities";

interface Props {
    device: Device | undefined;
    onAddClick: () => void;
    onModifyClick: () => void;
    onDeleteClick: () => void;
}

export const DevicePanel = ({ device, onAddClick, onModifyClick, onDeleteClick }: Props) => {
    const empty = "N/A"
    
    return (
        <div className="mainHeader">
            <div className="mainHeaderLeft">
                <div className="hTitleRow">
                <div className="hTitle">{device?.name || "No device selected"}</div>
                <div className="hSub">
                    <span className="muted">Id:</span> <b>{device?.id || empty}</b>
                    <span className="dotSep">|</span>
                    <span className="muted">Created at:</span> <b>{device?.createdAtUtc}</b>
                    <span className="dotSep">|</span>
                    <span className="muted">IsActive:</span> <b>{device?.isActive ? "true" : "false"}</b>
                    <span className="dotSep">|</span>
                    <span className="muted">Location:</span> <b>{device?.location || empty}</b>
                </div>
                </div>
            </div>

            {device && (
                <div className="mainHeaderRight">
                    <button className="btn" onClick={onAddClick}>Add</button>
                    <button className="btn ghost" onClick={onModifyClick}>Modify</button>
                    <button className="btn danger ghost" onClick={onDeleteClick}>Delete</button>
                </div>
            )}
        </div>
    );
};