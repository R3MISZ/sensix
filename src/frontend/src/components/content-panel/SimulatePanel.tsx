import React from "react";

export const SimulatePanel: React.FC = () => {
  return (
    <section className="panel">
        <div className="panelTop">
            <div className="panelTitle">Controls</div>

            <div className="panelActions">
            <div className="pill">
                <span className="pillLabel">Custom from</span>
                <input
                className="input inputDate"
                type="datetime-local"
                aria-label="Custom from"
                />
            </div>

            <div className="pill">
                <span className="pillLabel">Custom to</span>
                <input
                className="input inputDate"
                type="datetime-local"
                aria-label="Custom to"
                />
            </div>

            <button className="btn">
                ▶ Simulate
            </button>
            </div>
        </div>

        <div className="simGrid">
            <div className="simField">
                <div className="simLabel">Min</div>
                <input className="input" type="number" value={200}/>
            </div>
            <div className="simField">
                <div className="simLabel">Max</div>
                <input className="input" type="number" value={1000}/>
            </div>
            <div className="simField">
                <div className="simLabel">Noise (0..1)</div>
                <input className="input" type="number" step={0.05}/>
            </div>
            <div className="simField">
                <div className="simLabel">Trend (-1..1)</div>
                <input className="input" type="number" step={0.05}/>
            </div>
            <div className="simField">
                <div className="simLabel">Every (ms)</div>
                <input className="input" type="number" step={100}/>
            </div>
            <div className="simField simWide">
                <div className="simLabel">Sim target</div>
                    <div className="simHint">
                        <span className="muted">Simulation runs on the selected sensor when you press “Simulate”.</span>
                    </div>
            </div>
        </div>
    </section>
    );
};
