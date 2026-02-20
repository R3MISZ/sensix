import React, { useMemo } from "react";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer
} from "recharts";
import { type Measurement } from "../../types";

interface Props {
  data: Measurement[];
  unit?: string;
}

export const LineChartPanel: React.FC<Props> = ({ data, unit }) => {
  const chartData = useMemo(() => {
    return data.map((m) => ({
      // X-Axis
      time: new Date(m.timestampUtc).toLocaleTimeString("de-DE", {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
        hour: "2-digit",
        minute: "2-digit",
        second: "2-digit",
      }),
      value: m.value,
    }));
  }, [data]);

  return (
    <section className="panel">
      <div className="chartCard">
        <div className="chartTopRow">
          <div className="chartTitle">Time Series</div>
          <div className="chartHint">{data.length > 0 && `Unit: ${unit || "°C"}`}</div>
        </div>
      </div>

        <div className="chartContainer" style={{ width: "100%", height: "300px", marginTop: "20px", background: "rgba(0, 0, 0, 0)"}}>
          <ResponsiveContainer width="100%" height="100%">
            <LineChart data={chartData} margin={{ top: 5, right: 5, left: -20, bottom: 5 }}>
              <CartesianGrid 
                strokeDasharray="3 3" 
                vertical={false}
                stroke="#333"
              />
              <XAxis
                dataKey="time"
                fontSize={11}
                tickMargin={10}
                axisLine={true}
                tickLine={true}
                stroke="#888"
              />
              <YAxis
                fontSize={11}
                axisLine={true}
                tickLine={true}
                stroke="#888"
                unit={unit}
                domain={["auto", "auto"]}
              />
              <Tooltip
                contentStyle={{
                  backgroundColor: "#1a1a1ace",
                  border: "1px solid #333333d2",
                  borderRadius: "8px",
                  fontSize: "12px"
                }}
                itemStyle={{ color: "#1b63ff" }}
                cursor={{ stroke: "#444", strokeWidth: 1 }}
                formatter={(value: number | undefined) => [`${value} ${unit}`]}
              />
              <Line
                type="linear"
                dataKey="value"
                stroke="#6aa6ff"
                strokeWidth={3}
                dot={false}
                activeDot={{ r: 6, strokeWidth: 0 }}
                animationDuration={600}
              />
            </LineChart>
          </ResponsiveContainer>

      </div>
    </section>
  );
};