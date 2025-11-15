import { request } from '@umijs/max';

export async function getComponent(id: string | undefined) {
  return request(`component/${id}`);
}

export async function addComponent(data: API.Component) {
  return request(`component/add`, {
    method: 'POST',
    data,
  });
}

export async function updateComponent(data: API.Component) {
  return request(`component/update`, {
    method: 'POST',
    data,
  });
}

export async function listComponent(params: {
  current?: number;
  pageSize?: number;
}) {
  return request(`component/list`, {
    params,
  });
}

export async function listComponentWork(params: any, id: string | undefined) {
  return request(`component/list-work/${id}`, {
    params,
  });
}

export async function deleteComponent(id?: string) {
  return request(`component/delete/${id}`, {
    method: 'POST',
  });
}

export async function queryFormSelect(query: any) {
  const params = {
    searchTerm: query.keyWords,
  };
  return request(`component/form-select`, {
    params,
  });
}
