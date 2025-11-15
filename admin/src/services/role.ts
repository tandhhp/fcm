import { request } from "@umijs/max";

export async function listRole(params: any) {
    return request(`/role/list`, {
        params
    })
}

export async function apiSalesManagerOptions(params?: any) {
    return request('role/sales-manager-options', {
        params
    });
}

export async function apiRoleOptions(params?: any) {
    return request('role/options', {
        params
    });
}

export async function apiDotOptions(params?: any) {
    return request('role/dot-options', {
        params
    });
}

export async function apiTelesalesManagerOptions(params?: any) {
    return request('role/telesales-manager-options', {
        params
    });
}

export async function apiTelesalesOptions(params?: any) {
    return request('role/telesales-options', {
        params
    });
}

export async function apiRoleInit() {
    return request('role/init');
}

export async function apiRoleUpdate(data: any) {
    return request('role', {
        method: 'PUT',
        data
    });
}

export async function apiManagerOptions(params?: any) {
    return request('role/manager-options', {
        params
    });
}

export async function apiKeyInOptions(params?: any) {
    return request('role/key-in-options', {
        params
    });
}