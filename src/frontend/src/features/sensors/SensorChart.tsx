import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
} from "recharts";
import type { MeasurementDto } from "../../api/measurements.api";

type Props = {
  data: MeasurementDto[];
};

export function SensorChart({ data }: Props) {
  return (
    <ResponsiveContainer width="80%" height={300}>
      <LineChart data={data}>
        <XAxis dataKey="timestampUtc" tickFormatter={(v) => new Date(v).toLocaleTimeString()}/>
        <YAxis />
        <Tooltip />
        <Line type="monotone" dataKey="value" />
      </LineChart>
    </ResponsiveContainer>
  );
}