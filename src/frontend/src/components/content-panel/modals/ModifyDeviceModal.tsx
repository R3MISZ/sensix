import type { Device } from "../../../types/Entities"

interface Props {
  selectedDevice: Device | undefined;
  isOpen: boolean;
  onClose: () => void;
  onSave: (request: Device) => void;
}

export const ModifyDeviceModal = ({ selectedDevice, isOpen, onClose, onSave }: Props) => {
  if (!isOpen || !selectedDevice) return null;

  return (
    <div className="modalOverlay">
      <div className="modalCard">
        <div className="modalHeader">
          <div className="modalTitle">Modify Device: {selectedDevice.name}</div>
        </div>
        
        <form onSubmit={(e) => {
          e.preventDefault();
          const target = e.currentTarget.elements as any;
          
          const request: Device = {
            ...selectedDevice,
            name: target.deviceName.value,
            location: target.location.value,
            isActive: target.isActive.value === "true" // Convert to bool
          };
          
          onSave(request);
          onClose();
        }}>
          <div className="modalBody">
            <div className="formRow">
              <label className="formLabel">Id</label>
              <input className="input" value={selectedDevice.id} readOnly />
            </div>
            <div className="formRow">
              <label className="formLabel">Name</label>
              <input className="input" name="deviceName" defaultValue={selectedDevice.name} required />
            </div>
            <div className="formRow">
              <label className="formLabel">Location</label>
              <input className="input" name="location" defaultValue={selectedDevice.location} required />
            </div>
            <div className="formRow">
              <label className="formLabel">Status</label>
              <select className="input" name="isActive" defaultValue={selectedDevice.isActive.toString()}>
                  <option value="true">true</option>
                  <option value="false">false</option>
              </select>
            </div>
          </div>
          
          <div className="modalFooter">
            <button type="button" className="btn ghost" onClick={onClose}>Cancel</button>
            <button type="submit" className="btn">Update Device</button>
          </div>
        </form>
      </div>
    </div>
  );
};