import React, { useMemo } from "react";
import { type Measurement } from "../../types";

interface Props {
  data: Measurement[];
  unit: string | undefined;
}

export const ValuesPanel: React.FC<Props> = ({ data, unit }) => {
  const stats = data.length > 0 ? {
    current: data[data.length - 1].value,
    min: Math.min(...data.map(m => m.value)),
    max: Math.max(...data.map(m => m.value)),
    avg: Number((data.reduce((a, b) => a + b.value, 0) / data.length).toFixed(2)),
    count: data.length
  } : null;


const renderValue = (val: number | undefined): string => {
  if (val === undefined) return "-";
  return `${val} ${unit ?? ""}`; 
};;

  return (
    <section className="cardsRow">
      <div className="kpiCard">
        <div className="kpiLabel">Latest</div>
        <div className="kpiValue">{renderValue(stats?.current)}</div>
        <div className="kpiFoot">{stats ? "Live" : "No Data"}</div>
      </div>

      <div className="kpiCard">
        <div className="kpiLabel">Min</div>
        <div className="kpiValue">{renderValue(stats?.min)}</div>
        <div className="kpiFoot">Period Min</div>
      </div>

      <div className="kpiCard">
        <div className="kpiLabel">Max</div>
        <div className="kpiValue">{renderValue(stats?.max)}</div>
        <div className="kpiFoot">Period Max</div>
      </div>

      <div className="kpiCard">
        <div className="kpiLabel">Average</div>
        <div className="kpiValue">{renderValue(stats?.avg)}</div>
        <div className="kpiFoot">
          {stats ? `Based on ${stats.count} signals` : "-"}
        </div>
      </div>
    </section>
  );
};