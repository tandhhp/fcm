import { request } from "@umijs/max";

export async function apiAttendanceOptions(params?: any) {
    return request(`attendance/options`, {
        params
    });
}

export async function apiAttendanceList(params?: any) {
    return request(`attendance/list`, {
        params
    });
}

export async function apiAttendanceCreate(data: any) {
    return request(`attendance`, {
        method: 'POST',
        data
    });
}

export async function apiAttendanceUpdate(data: any) {
    return request(`attendance`, {
        method: 'PUT',
        data
    });
}

export async function apiAttendanceDelete(id: string) {
    return request(`attendance/${id}`, {
        method: 'DELETE'
    });
}