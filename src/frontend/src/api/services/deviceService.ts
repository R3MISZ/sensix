import { api } from "../baseApi";

import type { CreateDeviceRequest, Device } from "../../types";

const ENDPOINT = '/Devices'

export const deviceService = {
  getAll: async () => (await api.get<Device[]>(ENDPOINT)).data,

  get: async (id: string) => (await api.get<Device>(`${ENDPOINT}/${id}`)).data,

  create: async (request: CreateDeviceRequest) => (await api.post<Device>(ENDPOINT, request)).data,

  put: async (request: Device) => (await api.put<Device>(`${ENDPOINT}/${request.id}`, request)).data,

  delete: async (id: string) => (await api.delete(`${ENDPOINT}/${id}`)).data
}