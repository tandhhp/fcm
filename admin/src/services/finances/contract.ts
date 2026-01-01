import { request } from "@umijs/max";

export async function apiContractList(params: any) {
  return request('contract/list', {
    params: {
      ...params,
      fromDate: params.dateRange ? params.dateRange[0] : undefined,
      toDate: params.dateRange ? params.dateRange[1] : undefined,
      dateRange: undefined
    }
  });
}

export async function apiContractPaymentCreate(data: any) {
  return request('contract/payment', {
    method: 'POST',
    data
  });
}

export async function apiContractInvoiceList(params: any) {
  return request('contract/invoices', {
    params
  });
}

export async function apiContractDelete(id: string) {
  return request(`contract/${id}`, {
    method: 'DELETE'
  });
}

export async function apiContractExport(params?: any) {
  return request('contract/export', {
    params,
    responseType: 'blob'
  });
}

export async function apiContractGiftAdd(data: any) {
  return request('contract/add-gift', {
    method: 'POST',
    data
  });
}

export async function apiContractGiftDelete(data: any) {
  return request(`contract/delete-gift`, {
    method: 'POST',
    data
  });
}

export async function apiContractLeadOptions(params: any) {
  return request('contract/lead-options', {
    params
  });
}

export async function apiContractDetail(id: string) {
  return request(`contract/${id}`);
}

export async function apiContractCreate(data: any) {
  return request('contract', {
    method: 'POST',
    data
  });
}

export async function apiContractUpdate(data: any) {
  return request('contract', {
    method: 'PUT',
    data
  });
}

export async function apiContractEvidenceUpload(data: FormData) {
  return request('contract/upload-evidences', {
    method: 'POST',
    data,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  });
}

export async function apiContractEvidenceList(contractId: string) {
  return request(`contract/evidences/${contractId}`);
}

export async function apiContractEvidenceDelete(id: string) {
  return request(`contract/evidence/${id}`, {
    method: 'DELETE',
  });
}

export async function apiContractEvidenceTypeOptions(params: any) {
  return request('contract/evidence-type-options', {
    params
  });
}

export async function apiContractInvoiceOptions(params: any) {
  return request('contract/invoice-options', {
    params
  });
}