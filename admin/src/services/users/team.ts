import { request } from "@umijs/max";

export async function apiTeamList(params: any) {
    return request('team/list', { params });
}

export async function apiTeamCreate(data: any) {
    return request('team', {
        method: 'POST',
        data
    });
}

export async function apiTeamUpdate(data: any) {
    return request('team', {
        method: 'PUT',
        data
    });
}

export async function apiTeamDelete(id: string) {
    return request(`team/${id}`, {
        method: 'DELETE'
    });
}

export async function apiTeamUsers(params: any) {
    return request(`team/users`, { params });
}


export async function apiTeamOptions(params?: {
    callCenterId?: number;
    keyWord?: string;
    departmentId?: number;
}) {
    return request('team/options', {
        params
    });
}

export async function apiTeamAddUser(data: any) {
    return request('team/add-user', {
        method: 'POST',
        data
    });
}

export async function apiTeamDetail(id?: string) {
    return request(`team/${id}`);
}