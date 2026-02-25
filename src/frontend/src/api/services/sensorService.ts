import { api } from "../baseApi";

import type { CreateSensorRequest, Sensor } from "../../types";

const ENDPOINT = '/Sensors'

export const deviceService = {
  getAll: async () => {
    return (await api.get<Sensor[]>(ENDPOINT)).data
  },
  get: async (id: string) => {
    return (await api.get(`${ENDPOINT}/${id}`)).data
  },
  create: async (request: CreateSensorRequest) => {
    return (await api.post<Sensor>(ENDPOINT, request)).data;
  },
  put: async (request: Sensor) => {
    return (await api.put<Sensor>(`${ENDPOINT}/${request.id}`, request)).data;
  },
  delete: async (id: string) => {
    return (await api.delete(`${ENDPOINT}/${id}`)).data
  }
}