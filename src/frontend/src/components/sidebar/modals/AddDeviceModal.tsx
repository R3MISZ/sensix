import type { Device } from "../../../types/Entities"

interface Props {
  isOpen: boolean;
  onClose: () => void;
  onSave: (newDevice: Device) => void;
}

export const AddDeviceModal = (PARAM: Props) => {
    if (!PARAM.isOpen) {
        return null;
    }
    else {
        return (
            <div className="modalOverlay">
            <div className="modalCard">
            <div className="modalHeader">
                <div className="modalTitle">New Device</div>
            </div>
            <form onSubmit={(e) => {
                e.preventDefault();
                const target = e.target as any;
                const newDevice: Device = {
                    id: target.deviceId.value,
                    name: target.deviceName.value,
                    location: target.location.value,
                    status: "active"
                };
                PARAM.onSave(newDevice);
                PARAM.onClose();
            }}>
                <div className="modalBody">
                    <div className="formRow">
                        <label className="formLabel">Id</label>
                        <input className="input" name="deviceId" placeholder="e.g. 0123-4567-890" required />
                    </div>
                    <div className="formRow">
                        <label className="formLabel">Name</label>
                        <input className="input" name="deviceName" placeholder="e.g. Raspery Pi 5" required />
                    </div>
                    <div className="formRow">
                        <label className="formLabel">Location</label>
                        <input className="input" name="location" placeholder="e.g. Lab 1" required />
                    </div>
                </div>
                <div className="modalFooter">
                    <button type="button" className="btn ghost" onClick={PARAM.onClose}>Cancel</button>
                    <button type="submit" className="btn">Save Device</button>
                </div>
            </form>
            </div>
        </div>
        );
    }
};