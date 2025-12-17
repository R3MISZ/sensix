import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { getSensorById, type SensorDto } from "../api/sensors.api";

import { getMeasurements, type MeasurementDto } from "../api/measurements.api";
import { SensorChart } from "../features/sensors/SensorChart";

export function SensorPage() {
  const { id } = useParams<{ id: string }>();
  const [sensor, setSensor] = useState<SensorDto | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [measurements, setMeasurements] = useState<MeasurementDto[]>([]);

  useEffect(() => {
  if (!id) return;

  getSensorById(id).then(setSensor).catch(e => setError(e.message));

  getMeasurements()
    .then((all) => {
      const filtered = all
        .filter(m => m.sensorId === id)
        .sort((a, b) => a.timestampUtc.localeCompare(b.timestampUtc)); // asc
      setMeasurements(filtered);
    })
    .catch(e => setError(e.message));
}, [id]);

  useEffect(() => {
    if (!id) return;

    getSensorById(id).then(setSensor);

    getSensorById(id)
      .then(setSensor)
      .catch((e) => setError(e.message));
  }, [id]);

  if (error) return <p>Error: {error}</p>;
  if (!sensor) return <p>Loading...</p>;

  return (
    <div>
      <p><Link to="/">← Back</Link></p>

      <h1>{sensor.name}</h1>
      <p>Active: {sensor.isActive ? "yes" : "no"}</p>

      <h2>Measurements</h2>
      {measurements.length === 0 ? (<p>No data</p>) : (<SensorChart data={measurements} />)}

      {/* Measurements kommen als nächstes */}
    </div>
  );
}