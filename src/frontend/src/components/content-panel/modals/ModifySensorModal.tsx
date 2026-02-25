import type { Sensor } from "../../../types/Entities"

interface Props {
  selectedSensor: Sensor | undefined;
  isOpen: boolean;
  onClose: () => void;
  onSave: (updatedSensor: Sensor) => void;
}

export const ModifySensorModal = ({ selectedSensor, isOpen, onClose, onSave }: Props) => {
  if (!isOpen || !selectedSensor) return null;

  return (
    <div className="modalOverlay">
      <div className="modalCard">
        <div className="modalHeader">
          <div className="modalTitle">Modify Device: {selectedSensor.name}</div>
        </div>
        
        <form onSubmit={(e) => {
          e.preventDefault();
          const target = e.currentTarget.elements as any;
          
          const updatedSensor: Sensor = {
              ...selectedSensor,
              name: target.sensorName.value,
              type: target.location.value,
              unit: target.unit.value,
              isActive: target.isActive.value === "true" // Convert to bool
          };
          
          onSave(updatedSensor);
          onClose();
        }}>
            <div className="modalBody">
                <div className="formRow">
                    <label className="formLabel">Id</label>
                    <input className="input" value={selectedSensor.id} readOnly />
                </div>
                <div className="formRow">
                    <label className="formLabel">Name</label>
                    <input className="input" name="sensorName" defaultValue={selectedSensor.name} required />
                </div>
                <div className="formRow">
                    <label className="formLabel">Type</label>
                    <input className="input" name="location" defaultValue={selectedSensor.type} required />
                </div>
                <div className="formRow">
                    <label className="formLabel">Unit</label>
                    <input className="input" name="unit" defaultValue={selectedSensor.unit} required />
                </div>
                <div className="formRow">
                    <label className="formLabel">Status</label>
                    <select name="isActive" defaultValue={selectedSensor.isActive.toString()}>
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