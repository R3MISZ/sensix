import { api } from "../baseApi";

import type { CreateMeasurementRequest, Measurement } from "../../types";

const ENDPOINT = '/Measurements';

export const measurementService = {
  getAll: async () => {
    return (await api.get<Measurement[]>(ENDPOINT)).data;
  },
  get: async (id: string) => {
    return (await api.get(`${ENDPOINT}/${id}`)).data;
  },
  create: async (request: CreateMeasurementRequest) => {
    return (await api.post<Measurement>(ENDPOINT, request)).data
  },
  delete: async (id: string) => {
    return (await api.delete(`${ENDPOINT}/${id}`)).data
  }
}