import { useEffect, useMemo, useState } from "react";
import { Link, useParams } from "react-router-dom";
import {
  getMeasurements,
  getSensorById,
  type MeasurementDto,
  type SensorDto,
} from "../api/baseApi";

import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
} from "recharts";



export function SensorPage() {
  const { id } = useParams<{ id: string }>();

  const [sensor, setSensor] = useState<SensorDto | null>(null);
  const [allMeasurements, setAllMeasurements] = useState<MeasurementDto[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    if (!id) return;

    setIsLoading(true);
    setError(null);

    Promise.all([getSensorById(id), getMeasurements()])
      .then(([s, m]) => {
        setSensor(s);
        setAllMeasurements(m);
      })
      .catch((e) => setError(e?.message ?? "Unknown error"))
      .finally(() => setIsLoading(false));
  }, [id]);

  const measurements = useMemo(() => {
    if (!id) return [];
    return allMeasurements
      .filter((m) => m.sensorId === id)
      .sort((a, b) => a.timestampUtc.localeCompare(b.timestampUtc));
  }, [allMeasurements, id]);

  if (!id) return <p>Invalid sensor id.</p>;
  if (isLoading) return <p>Loading sensor...</p>;
  if (error) return <p>Error: {error}</p>;
  if (!sensor) return <p>Sensor not found</p>;

  return (
    <div className="page">
      <div className="content">
        <p>
          <Link to="/">← Back</Link>
        </p>

        <h1>{sensor.name?.trim() ? sensor.name : "Unnamed sensor"}</h1>
        <p>Active: {sensor.isActive ? "yes" : "no"}</p>

        <h2>Measurements</h2>
        {measurements.length === 0 ? (
          <p>No data</p>
        ) : (
          <SensorChart data={measurements} />
        )}
      </div>
    </div>
  );
}

type SensorChartProps = {
  data: MeasurementDto[];
};

function SensorChart({ data }: SensorChartProps) {
  return (
    <ResponsiveContainer width="100%" height={300}>
      <LineChart data={data}>
        <XAxis
          dataKey="timestampUtc"
          tickFormatter={(v) => new Date(v).toLocaleTimeString()}
        />
        <YAxis />
        <Tooltip />
        <Line type="monotone" dataKey="value" />
      </LineChart>
    </ResponsiveContainer>
  );
}