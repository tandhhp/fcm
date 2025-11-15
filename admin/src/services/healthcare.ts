import { request } from '@umijs/max';

export const apiHealthcareContent = (catalogId: string) => {
    return request(`healthcare/${catalogId}`);
}

export const apiHealthcareContentSave = (data: any) => {
  return request(`healthcare/save`, {
    method: 'POST',
    data
  });
}

export const apiHealthcareCreate = (data: any) => request(`healthcare/create-combo`, {
  method: 'POST',
  data
})

export const apiGetCombo = (id?: string) => request(`healthcare/${id}`);

export const apiHealthcareList = (id?: string) => request(`healthcare/list-healthcare/${id}`);

export const apiSaveComboHealthcare = (data: any) => request(`healthcare/combo/update`, {
  method: 'POST',
  data
})

export const apiDeleteCombo = (id: string) => request(`healthcare/combo/delete/${id}`, { method: 'POST' })