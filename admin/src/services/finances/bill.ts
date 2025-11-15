import { request } from "@umijs/max";

export async function apiBillList(params: any) {
    return await request('bill/list', { params });
}

export async function apiBillCreate(data: any) {
    return await request('bill', {
        method: 'POST',
        data
    });
}

export async function apiBillUpdate(data: any) {
    return await request(`bill`, {
        method: 'PUT',
        data
    });
}

export async function apiBillDelete(id: string) {
    return await request(`bill/${id}`, {
        method: 'DELETE'
    });
}

export async function apiBillApprove(id: string) {
    return await request(`bill/approve/${id}`, {
        method: 'POST'
    });
}

export async function apiBillReject(id: string) {
    return await request(`bill/reject/${id}`, {
        method: 'POST'
    });
}