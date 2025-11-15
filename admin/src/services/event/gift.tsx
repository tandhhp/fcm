import { request } from "@umijs/max";

export async function apiGiftList(params: any) {
    return request('gift/list', { params });
}

export async function apiGiftCreate(data: any) {
    return request('gift', {
        method: 'POST',
        data
    });
}

export async function apiGiftUpdate(data: any) {
    return request(`gift`, {
        method: 'PUT',
        data
    });
}

export async function apiGiftDelete(id: string) {
    return request(`gift/${id}`, {
        method: 'DELETE'
    });
}

export async function apiGiftDetail(id: string) {
    return request(`gift/${id}`);
}

export async function apiGiftOptions(params?: any) {
    return request('gift/options', { params });
}