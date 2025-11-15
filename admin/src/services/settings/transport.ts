import { request } from "@umijs/max";

export async function apiTransportOptions(params?: any) {
    return request("transport/options", { params });
}

export async function apiTransportCreate(data: any) {
    return request("transport", {
        method: "POST",
        data
    });
}

export async function apiTransportUpdate(data: any) {
    return request(`transport`, {
        method: "PUT",
        data
    });
}

export async function apiTransportDelete(id: string) {
    return request(`transport/${id}`, {
        method: "DELETE"
    });
}

export async function apiTransportList(params: any) {
    return request("transport/list", { params });
}
