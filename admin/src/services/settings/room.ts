import { request } from "@umijs/max";

export async function apiRoomList(params: any) {
    return request('room/list', {
        params
    });
}

export async function apiRoomCreate(data: any) {
    return request('room', {
        method: 'POST',
        data
    });
}

export async function apiRoomUpdate(data: any) {
    return request(`room`, {
        method: 'PUT',
        data
    });
}

export async function apiRoomOptions() {
    return request('room/options');
}

export async function apiRoomDelete(id: number) {
    return request(`room/${id}`, {
        method: 'DELETE'
    });
}

export async function apiRoomDetail(id?: string) {
    return request(`room/${id}`);
}