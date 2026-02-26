import type { CreateDeviceRequest } from "../../../types/Entities"

interface Props {
  isOpen: boolean;
  onClose: () => void;
  onSave: (request: CreateDeviceRequest) => void;
}

export const AddDeviceModal = ({isOpen, onSave, onClose}: Props) => {
    if (!isOpen) {
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
                const request: CreateDeviceRequest = {
                name: target.name.value,
                location: target.location.value,
                isActive: target.isActive.value === "true" // convert to bool
                };
                onSave(request);
                onClose();
            }}>
                <div className="modalBody">
                    <div className="formRow">
                        <label className="formLabel">Name</label>
                        <input className="input" name="name" placeholder="e.g. Raspery Pi 5" required />
                    </div>
                    <div className="formRow">
                        <label className="formLabel">Location</label>
                        <input className="input" name="location" placeholder="e.g. Lab 1" required />
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