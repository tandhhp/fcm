import { request } from "@umijs/max";

export async function apiBranchOptions() {
    return request(`branch/options`);
}

export async function apiBranchList(params?: any) {
    return request(`branch/list`, {
        params
    });
}

export async function apiBranchCreate(data: any) {
    return request(`branch`, {
        method: 'POST',
        data
    });
}

export async function apiBranchUpdate(data: any) {
    return request(`branch`, {
        method: 'PUT',
        data
    });
}

export async function apiBranchDelete(id: number) {
    return request(`branch/${id}`, {
        method: 'DELETE'
    });
}