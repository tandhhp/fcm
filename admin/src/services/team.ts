import { request } from "@umijs/max";

export async function apiTeamList(params: any) {
    return request('team/list', { params });
}

export async function apiTeamCreate(data: any) {
    return request('team/create', {
        method: 'POST',
        data
    });
}

export async function apiTeamUpdate(data: any) {
    return request('team/update', {
        method: 'POST',
        data
    });
}

export async function apiTeamDelete(id: string) {
    return request(`team/delete/${id}`, {
        method: 'POST'
    });
}