import { request } from "@umijs/max";

export async function apiInvoiceList(params: any) {
  return request('invoice/list', {
    params: {
        ...params,
        fromDate: params?.dateRange ? params.dateRange[0] : undefined,
        toDate: params?.dateRange ? params.dateRange[1] : undefined,
        dateRange: undefined
    }
  });
}

export async function apiInvoiceApprove(id: string) {
    return request(`invoice/approve/${id}`, {
        method: 'POST'
    });
}

export async function apiInvoiceReject(data: any) {
    return request(`invoice/reject`, {
        method: 'POST',
        data
    });
}

export async function apiInvoiceCancel(data: any) {
    return request(`invoice/cancel`, {
        method: 'POST',
        data
    });
}

export async function apiInvoiceExport(data: any) {
    return request(`invoice/export`, {
        data: {
            ...data,
            fromDate: data?.dateRange ? data.dateRange[0] : undefined,
            toDate: data?.dateRange ? data.dateRange[1] : undefined,
            dateRange: undefined
        },
        responseType: 'blob',
        method: 'POST'
    });
}

export async function apiInvoiceStatistics() {
    return request('invoice/statistics');
}

export async function apiInvoiceUpdate(data: any) {
    return request('invoice', {
        method: 'PUT',
        data
    });
}

export async function apiInvoiceDetail(id: string) {
    return request(`invoice/${id}`);
}