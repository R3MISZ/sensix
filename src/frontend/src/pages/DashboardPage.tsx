import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { getSensors, type SensorDto } from "../api/baseApi";

export function DashboardPage() {
  const [sensors, setSensors] = useState<SensorDto[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    setIsLoading(true);
    setError(null);

    getSensors()
      .then(setSensors)
      .catch((err) => setError(err?.message ?? "Unknown error"))
      .finally(() => setIsLoading(false));
  }, []);

  return (
    <div className="page">
      <div className="content">
        <h1>Sensix - Sensors</h1>

        {isLoading && <p>Loading sensors...</p>}
        {error && <p>Error: {error}</p>}

        {!isLoading && !error && <SensorsTable sensors={sensors} />}
      </div>
    </div>
  );
}

type SensorsTableProps = {
  sensors: SensorDto[];
};

function SensorsTable({ sensors }: SensorsTableProps) {
  if (sensors.length === 0) return <p>No sensors found.</p>;

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
              <Link to={`/sensors/${sensor.id}`}>
                {sensor.name?.trim() ? sensor.name : "Unnamed"}
              </Link>
            </td>
            <td>{sensor.isActive ? "yes" : "no"}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}