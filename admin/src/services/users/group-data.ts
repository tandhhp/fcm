import { request } from "@umijs/max";

export async function apiGroupDataOptions(params?: any) {
    return request(`group-data/options`, {
        params
    });
}