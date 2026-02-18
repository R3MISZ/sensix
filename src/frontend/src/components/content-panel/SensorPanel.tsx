import type { Sensor } from "../../types";

interface Props {
    sensor: Sensor | undefined;
    onAddClick: () => void;
    onModifyClick: () => void;
    onDeleteClick: () => void;
}

export const SensorPanel = ({sensor, onAddClick, onModifyClick, onDeleteClick }: Props) => {
    const empty = "N/A"

    return (
        <div className="mainHeader">
            <div className="mainHeaderLeft">
                <div className="hTitleRow">
                    <div className="hTitle">{sensor ? sensor.name : "No sensor selected"}</div>
                    
                    <div className="hSub">
                        <span className="muted">Id:</span> <b>{sensor?.id || empty}</b>
                        <span className="dotSep">|</span>
                        <span className="muted">Status:</span> <b>{sensor?.status || empty}</b>
                        <span className="dotSep">|</span>
                        <span className="muted">Unit:</span> <b>{sensor?.unit || empty}</b>
                    </div>
                </div>
            </div>

            {sensor && (
                <div className="mainHeaderRight">
                    <button className="btn" onClick={onAddClick}>Add</button>
                    <button className="btn ghost" onClick={onModifyClick}>Modify</button>
                    <button className="btn danger ghost" onClick={onDeleteClick}>Delete</button>
                </div>
            )}
        </div>
    );
};