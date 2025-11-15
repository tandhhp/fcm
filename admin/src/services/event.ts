import { request } from '@umijs/max';

export async function apiBackToCheckin(data: any) {
    return request(`event/back-to-checkin`, {
        method: 'POST',
        data
    });
}

export async function apiListEventSaleRevenue(params: any) {
    return request(`event/list-sale-revenue`, { params });
}

export async function apiListKeyInRevenue(params: any) {
    return request(`event/list-key-in-revenue`, { params });
}

export async function apiDebtHistoryList(params: any) {
    return request(`event/revenue-history`, { params });
}

export async function apiTopupKeyIn(data: any) {
    return request(`event/add-sale-revenue`, {
        method: 'POST',
        data,
        headers: {
            'Content-Type': 'multipart/form-data',
        },
    });
}

export async function apiEventList(params: any) {
    return request('event/list', { params });
}

export async function apiEventOptions(params: any) {
    return request('event/options', { params });
}

export async function apiEventDetail(id?: string) {
    return request(`event/${id}`);
}

export async function apiEventTableOptions(params: any) {
    return request('event/table-options', { params });
}

export async function apiCloseDeal(data: any) {
    return request('event/close-deal', {
        method: 'POST',
        data
    });
}

export async function apiEventSuReport(params: any) {
    return request('event/su-report', { params });
}

export async function apiEventReinvite(data: any) {
    return request('contact/reinvite', {
        method: 'POST',
        data
    });
}

export async function apiEventKeyInOptions(params?: any) {
    return request('event/key-in-options', { params });
}

export async function apiEventToOptions(params: any) {
    return request('event/to-options', { params });
}