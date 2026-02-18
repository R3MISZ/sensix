import React from "react";

export const ControlPanel: React.FC = () => {
  return (
    <section className="panel">
        <div className="simGrid">
            <div className="simField">
                <div className="simLabel">Min</div>
                <input className="input" type="number" value={200}/>
            </div>
            <div className="simField">
                <div className="simLabel">Max</div>
                <input className="input" type="number" value={1000}/>
            </div>

            <div className="panelActions">
                <div className="pill">
                    <span className="pillLabel">FROM</span>
                    <input
                    className="input inputDate"
                    type="datetime-local"
                    aria-label="FROM"
                    />
                </div>

                <div className="pill">
                    <span className="pillLabel">TO</span>
                    <input
                    className="input inputDate"
                    type="datetime-local"
                    aria-label="TO"
                    />
                </div>
            </div>
        </div>
    </section>
    );
};
