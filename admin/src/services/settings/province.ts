import { request } from "@umijs/max";

export async function apiProvinceList(params: any) {
    return request(`province/list`, {
        params
    })
}

export async function apiProvinceOptions(params: any) {
    return request(`province/options`, { params });
}

export async function apiProvinceCreate(data: any) {
    return request(`province`, {
        method: 'POST',
        data
    })
}

export async function apiProvinceUpdate(data: any) {
    return request(`province`, {
        method: 'PUT',
        data
    })
}

export async function apiProvinceDelete(id: number) {
    return request(`province/${id}`, {
        method: 'DELETE'
    })
}
