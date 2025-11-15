import { request } from '@umijs/max';

export async function apiUpdateKeyInBranch(data: any) {
  return request('/lead/update-branch', {
    method: 'POST',
    data,
  });
}