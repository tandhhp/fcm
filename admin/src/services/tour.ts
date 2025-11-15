import { request } from '@umijs/max';

export const apiAmenities = () => request(`tour/amenities`);

export async function apiListCity(params: any) {
    return request(`tour/list-city`, { params });
}

export async function apiDeleteCity(id: string) {
    return request(`tour/delele-city/${id}`, {
        method: 'POST'
    });
}

export async function apiAddCity(data: any) {
    return request(`tour/add-city`, {
        method: 'POST',
        data
    })
}