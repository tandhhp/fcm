import { request } from "@umijs/max";

export async function apiLeadReject(data: any) {
  return request(`lead/reject`, {
    method: 'POST',
    data
  });
}

export async function apiLeadCheckin(data: any) {
  return request(`lead/checkin`, {
    method: 'POST',
    data
  });
}

export async function apiLeadDetail(id?: string) {
  return request(`lead/${id}`);
}

export async function apiLeadWaitingList(params?: any) {
  return request(`lead/waiting-list`, {
    params
  });
}

export async function apiLeadCheckinList(params?: any) {
  return request(`lead/checkin-list`, {
    params: {
      fromDate: params?.dateRange ? params.dateRange[0] : undefined,
      toDate: params?.dateRange ? params.dateRange[1] : undefined,
      ...params,
    }
  });
}

export async function apiLeadExportCheckin(params?: any) {
  return request(`lead/export-checkin`, {
    params,
    responseType: 'blob'
  });
}

export async function apiLeadUpdate(data: any) {
  return request(`lead`, {
    method: 'PUT',
    data
  });
}

export async function apiLeadFeedbackUpdate(data: any) {
  return request(`lead/feedback`, {
    method: 'PUT',
    data
  });
}

export async function apiLeadAllowDuplicate(id: string) {
  return request(`lead/allowed-duplicate/${id}`, {
    method: 'POST'
  });
}

export async function apiLeadUpdateFeedback(data: any) {
  return request(`lead/update-feedback`, {
    method: 'PUT',
    data
  });
}