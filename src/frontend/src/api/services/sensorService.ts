import { api } from "../baseApi";

import type { CreateSensorRequest, Sensor } from "../../types";

const ENDPOINT = '/Sensors'

export const sensorService = {
  getAll: async () => (await api.get<Sensor[]>(ENDPOINT)).data,
  
  get: async (id: string) => (await api.get(`${ENDPOINT}/${id}`)).data,

  create: async (request: CreateSensorRequest) => (await api.post<Sensor>(ENDPOINT, request)).data,

  put: async (request: Sensor) => (await api.put<Sensor>(`${ENDPOINT}/${request.id}`, request)).data,

  delete: async (id: string) => (await api.delete(`${ENDPOINT}/${id}`)).data
}