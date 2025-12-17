const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;
const API_PREFIX = "/api";

export async function http<T>(url: string): Promise<T> {
  const res = await fetch(`${API_BASE_URL}${API_PREFIX}${url}`);

  if (!res.ok) {
    throw new Error(await res.text());
  }

  return res.json();
}
