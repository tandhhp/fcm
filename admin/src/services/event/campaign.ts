import { request } from "@umijs/max";

export async function apiCampaignList(params: any) {
    return request(`campaign/list`, { params });
}

export async function apiCampaignCreate(data: any) {
    return request(`campaign`, {
        method: 'POST',
        data
    });
}

export async function apiCampaignUpdate(data: any) {
    return request(`campaign`, {
        method: 'PUT',
        data
    });
}

export async function apiCampaignDelete(id: number) {
    return request(`campaign/${id}`, {
        method: 'DELETE',
    });
}

export async function apiCampaignOptions() {
    return request(`campaign/options`);
}

export async function apiCampaignDetail(id?: string) {
    return request(`campaign/${id}`);
}