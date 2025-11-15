import { request } from "@umijs/max";

export async function queryLogs(params: any) {
  return request(`log/list`, {
    params
  });
}

export async function deleteLog(logId: string) {
  return request(`log/delete/${logId}`, {
    method: 'POST'
  });
}

export const apiDeleteAllLog = () => request(`log/delete/all`, {
  method: 'POST'
})