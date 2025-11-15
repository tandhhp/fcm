import { request } from "@umijs/max";

export async function apiTableList(params: any) {
    return request('table/list', {
        params
    });
}

export async function apiTableUpdate(data: any) {
    return request(`table`, {
        method: 'PUT',
        data
    });
}

export async function apiTableCreate(data: any) {
    return request('table', {
        method: 'POST',
        data
    });
}

export async function apiTableDelete(id: number) {
    return request(`table/${id}`, {
        method: 'DELETE'
    });
}

export async function apiTableOptions(params?: any) {
    return request('table/options', {
        params
    });
}

export async function apiTableAll(params?: any) {
    return request('table/all', {
        params
    });
}