import { request } from "@umijs/max";

type CallCenterListItem = {
    id: number;
    code: string;
    name: string;
    teamCount: number;
}

export async function apiCallCenterOptions(params?: any) {
    return request(`call-center/options`, {
        params
    });
}

export async function apiCallCenterList(params: any) {
    return request<API.PagedResult<CallCenterListItem>>(`call-center/list`, {
        params
    });
}