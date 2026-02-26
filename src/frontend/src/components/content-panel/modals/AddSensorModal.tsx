import type { CreateSensorRequest} from "../../../types"

interface Props {
    selectedDeviceId?: string;
    isOpen: boolean;
    onSave: (request: CreateSensorRequest) => void;
    onClose: () => void;
}

export const AddSensorModal = ({isOpen, selectedDeviceId, onSave, onClose}: Props) => {
    if (!isOpen) {
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
                const request: CreateSensorRequest = {
                    deviceId: target.deviceId.value,
                    name: target.sensorName.value,
                    type: target.sensorType.value,
                    unit: target.sensorUnit.value,
                    isActive: target.isActive.value === "true" // convert to bool
                };
                onSave(request);
                onClose();
            }}>
                <div className="modalBody">
                    <div className="formRow">
                        <label className="formLabel">Device Id</label>
                        <input className="input" name="deviceId" value={selectedDeviceId} readOnly />
                    </div>
                    <div className="formRow">
                        <label className="formLabel">Name</label>
                        <input className="input" name="sensorName" placeholder="e.g. Temperature Sensor" required />
                    </div>
                     <div className="formRow">
                        <label className="formLabel">Type</label>
                        <input className="input" name="sensorType" placeholder="e.g. Temperature" required />
                    </div>
                    <div className="formRow">
                        <label className="formLabel">Unit</label>
                        <input className="input" name="sensorUnit" placeholder="e.g. °C" required />
                    </div>
                    <div className="formRow">
                        <label className="formLabel" htmlFor="isActive">Is Active</label>
                        <select name="isActive" id="isActive" defaultValue="true">
                            <option value="true">true</option>
                            <option value="false">false</option>
                        </select>
                    </div>
                </div>
                <div className="modalFooter">
                    <button type="button" className="btn ghost" onClick={onClose}>Cancel</button>
                    <button type="submit" className="btn">Save Device</button>
                </div>
            </form>
            </div>
        </div>
        );
    }
};