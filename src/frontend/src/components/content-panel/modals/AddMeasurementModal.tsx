
interface Props {
		isOpen: boolean;
		onClose: () => void;
}

export const AddMeasurementModal = ({isOpen, onClose}: Props) => {
		if (!isOpen) {
				return null;
		}
		else {
				return (
				<div className="modalOverlay">
						<div className="modalCard">
								<div className="modalHeader">
										<div className="modalTitle">New Measurement</div>
										<div className="mainHeaderRight">
												<button className="btn ghost" onClick={onClose}>x</button>
										</div>
								</div>
								<div className="modalBody">
										<label className="formLabel">To add new measurement use the api</label>
								</div>
						</div>
				</div>
				);
		}
}