import { request } from "@umijs/max"

export async function apiVoucherOptions(params?: any) {
    return request('voucher/options', {
        params
    });
}

export async function apiVoucherList(params?: any) {
    return request('voucher/list', {
        params
    });
}

export async function apiVoucherAdd(data: any) {
    return request('voucher', {
        method: 'POST',
        data
    });
}

export async function apiVoucherUpdate(data: any) {
    return request('voucher', {
        method: 'PUT',
        data
    });
}

export async function apiVoucherDelete(id: string) {
    return request(`voucher/${id}`, {
        method: 'DELETE'
    });
}

export async function apiVoucherImport(data: FormData) {
    return request('voucher/import', {
        method: 'POST',
        data,
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}

export async function apiVoucherExport(params?: any) {
    return request('voucher/export', {
        params,
        responseType: 'blob'
    });
}