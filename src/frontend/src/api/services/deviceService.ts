import { api } from "../baseApi";

import type { CreateDeviceRequest, Device } from "../../types";

const ENDPOINT = '/Devices'

export const deviceService = {
  getAll: async () => {
    return (await api.get<Device[]>(ENDPOINT)).data
  },
  get: async (id: string) => {
    return (await api.get<Device>(`${ENDPOINT}/${id}`)).data;
  },
  create: async (request: CreateDeviceRequest) => {
    return (await api.post<Device>(ENDPOINT, request)).data;
  },
  put: async (request: Device) => {
    return (await api.put<Device>(`${ENDPOINT}/${request.id}`, request)).data;
  },
  delete: async (id: string) => {
    return (await api.delete(`${ENDPOINT}/${id}`)).data
  }
}