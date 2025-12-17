import { useEffect, useState } from "react";
import { getSensors, type SensorDto } from "../../api/sensors.api";
import { Link } from "react-router-dom";

export function SensorsTable() {
  const [sensors, setSensors] = useState<SensorDto[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    getSensors()
      .then(setSensors)
      .catch(err => setError(err.message));
  }, []);

  if (error) return <p>Error: {error}</p>;

  return (
    <table>
      <thead>
        <tr>
          <th>Name</th>
          <th>Active</th>
        </tr>
      </thead>
      <tbody>
        {sensors.map((sensor) => (
          <tr key={sensor.id}>
            <td>
              <Link to={`/sensors/${sensor.id}`}>{sensor.name}</Link>
            </td>
            <td>{sensor.isActive ? "yes" : "no"}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
