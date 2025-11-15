import { request } from '@umijs/max';

export async function listLocalization(params: any) {
  return request(`localization/list`, {
    params,
  });
}

export async function addLocalization(data: any) {
  return request(`localization/add`, {
    method: 'POST',
    data,
  });
}

export async function saveLocalization(data: any) {
  return request(`localization/save`, {
    method: 'POST',
    data,
  });
}

export async function deleteLocalization(id: string) {
  return request(`localization/delete/${id}`, {
    method: 'POST',
  });
}
