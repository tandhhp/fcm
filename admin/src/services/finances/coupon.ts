import { request } from "@umijs/max";

export async function apiCouponCreate(data: any) {
    return request('coupon', { method: 'POST', data });
}

export async function apiCouponUpdate(data: any) {
    return request(`coupon`, { method: 'PUT', data });
}

export async function apiCouponList(params: any) {
    return request('coupon/list', { params });
}

export async function apiCouponDelete(id: string) {
    return request(`coupon/${id}`, { method: 'DELETE' });
}