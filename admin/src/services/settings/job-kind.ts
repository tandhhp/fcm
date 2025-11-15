import { request } from "@umijs/max";

export async function apiJobKindOptions(params?: any) {
  return request(`job-kind/options`, { params });
}

export async function apiJobKindCreate(data: any) {
  return request(`job-kind`, {
    method: 'POST',
    data
  });
}

export async function apiJobKindUpdate(data: any) {
  return request(`job-kind`, {
    method: 'PUT',
    data
  });
}

export async function apiJobKindDelete(id: string) {
  return request(`job-kind/${id}`, {
    method: 'DELETE'
  });
}

export async function apiJobKindList(params: any) {
  return request(`job-kind/list`, { params });
}
