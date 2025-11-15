import { request } from "@umijs/max";

export async function apiNotificationCount() {
    return request(`notification/count`);
}

export async function apiNotificationMyList(params: any) {
    return request(`notification/my-list`, {
        params,
    });
}

export async function apiNotificationDetail(id: string) {
    return request(`notification/${id}`);
}

export async function apiNotificationDelete(id: string) {
    return request(`notification/delete/${id}`, {
        method: "POST",
    });
}