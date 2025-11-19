import { request } from "@umijs/max";

export async function apiSourceList(params: any) {
    return request('source/list', {
        params
    });
}

export async function apiSourceCreate(data: any) {
    return request('source', {
        method: 'POST',
        data
    });
}

export async function apiSourceUpdate(data: any) {
    return request('source', {
        method: 'PUT',
        data
    });
}

export async function apiSourceDelete(id: number) {
    return request(`source/${id}`, {
        method: 'DELETE'
    });
}

export async function apiSourceDetail(id: number) {
    return request(`source/${id}`);
}

export async function apiSourceOptions(params?: any) {
    return request<{
        label: string;
        value: string;
    }[]>('source/options', {
        params
    });
}

export async function apiSourceAssign(data: any) {
    return request('source/assign', {
        method: 'POST',
        data
    });
}

export async function apiAvailableSource() {
    return request(`source/availables`);
}