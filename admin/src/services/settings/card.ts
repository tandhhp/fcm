import { request } from "@umijs/max";

export async function apiCardCreate(data: any) {
    return request(`card`, {
        method: 'POST',
        data
    });
}

export async function apiCardList(params: any) {
    return request(`card/list`, {
        params
    });
}

export async function apiCardUpdate(data: any) {
    return request(`card`, {
        method: 'PUT',
        data
    });
}

export async function apiCardDelete(id: number) {
    return request(`card/${id}`, {
        method: 'DELETE'
    });
}

export async function apiCardDetail(id: number) {
    return request(`card/${id}`);
}

export async function apiCardOptions(params: any) {
    return request(`card/options`, {
        params
    });
}