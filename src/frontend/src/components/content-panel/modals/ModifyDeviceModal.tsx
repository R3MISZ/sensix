import type { Device } from "../../../types/Entities"

interface Props {
  selectedDevice: Device | undefined;
  isOpen: boolean;
  onClose: () => void;
  onSave: (updatedDevice: Device) => void;
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
          const target = e.target as any;
          
          const updatedDevice: Device = {
              ...selectedDevice, 
              name: target.deviceName.value,
              location: target.location.value,
          };
          
          onSave(updatedDevice);
          onClose();
        }}>
            <div className="modalBody">
                <div className="formRow">
                    <label className="formLabel">Id (locked)</label>
                    <input 
                        className="input" 
                        value={selectedDevice.id} 
                        readOnly 
                        style={{ backgroundColor: '#f0f0f0', cursor: 'not-allowed' }}
                    />
                </div>
                
                <div className="formRow">
                    <label className="formLabel">Name</label>
                    <input 
                        className="input" 
                        name="deviceName" 
                        defaultValue={selectedDevice.name} 
                        required 
                    />
                </div>
                
                <div className="formRow">
                    <label className="formLabel">Location</label>
                    <input 
                        className="input" 
                        name="location" 
                        defaultValue={selectedDevice.location} 
                        required 
                    />
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