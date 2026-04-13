import { request } from "@umijs/max";

export async function apiGroupDataList(params?: any) {
    return request('group-data/list', {
        params
    });
}

export async function apiGroupDataDetail(id: number) {
    return request(`group-data/${id}`);
}

export async function apiGroupDataCreate(data: { name: string }) {
    return request('group-data', {
        method: 'POST',
        data
    });
}

export async function apiGroupDataUpdate(data: { id: number; name: string }) {
    return request('group-data', {
        method: 'PUT',
        data
    });
}

export async function apiGroupDataDelete(id: number) {
    return request(`group-data/${id}`, {
        method: 'DELETE'
    });
}

export async function apiGroupDataOptions(params?: any) {
    return request(`group-data/options`, {
        params
    });
}