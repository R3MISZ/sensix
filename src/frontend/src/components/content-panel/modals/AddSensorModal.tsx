import type { Device, Sensor } from "../../../types"

interface Props {
    selectedDevice?: Device;
    isOpen: boolean;
    onSave: (newDevice: Sensor) => void;
    onClose: () => void;
}

export const AddSensorModal = (props: Props) => {
    if (!props.isOpen) {
        return null;
    }
    else {
        return (
            <div className="modalOverlay">
            <div className="modalCard">
            <div className="modalHeader">
                <div className="modalTitle">New Sensor</div>
            </div>
            <form onSubmit={(e) => {
                e.preventDefault();
                const target = e.target as any;
                const newSensor: Sensor = {
                    device_id: target.deviceId.value,
                    id: target.sensorId.value,
                    name: target.sensorName.value,
                    unit: target.sensorUnit.value,
                    status: target.sensorStatus.value
                };
                props.onSave(newSensor);
                props.onClose();
            }}>
                <div className="modalBody">
                    <div className="formRow">
                        <label className="formLabel">Device Id</label>
                        <input className="input" name="deviceId" value={props.selectedDevice?.id} readOnly />
                    </div>
                    <div className="formRow">
                        <label className="formLabel">Sensor Id</label>
                        <input className="input" name="sensorId" placeholder="e.g. 0123-4567-890" required />
                    </div>
                    <div className="formRow">
                        <label className="formLabel">Name</label>
                        <input className="input" name="sensorName" placeholder="e.g. Temperature Sensor" required />
                    </div>
                    <div className="formRow">
                        <label className="formLabel">Unit</label>
                        <input className="input" name="sensorUnit" placeholder="e.g. °C" required />
                    </div>
                    <div className="formRow">
                        <label className="formLabel">Status</label>
                        <input className="input" name="sensorStatus" value={"active"} readOnly />
                    </div>
                </div>
                <div className="modalFooter">
                    <button type="button" className="btn ghost" onClick={props.onClose}>Cancel</button>
                    <button type="submit" className="btn">Save Device</button>
                </div>
            </form>
            </div>
        </div>
        );
    }
};