import { request } from "@umijs/max";

export async function apiCallCenterOptions(params?: any) {
    return request(`call-center/options`, {
        params
    });
}