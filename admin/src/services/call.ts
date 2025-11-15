import { request } from "@umijs/max";

export async function apiCallOptions(params: any) {
    return await request('call/status/options', { params });
}

export async function apiCallComplete(data: any) {
    return await request('call/complete', {
        method: 'POST',
        data
    });
}

export async function apiCallHistories(params: any) {
    return await request('call/histories', { params });
}

export async function apiCallStatistics() {
    return await request('call/statistics');
}

export async function apiCallStatusList(params: any) {
    return await request('call/status/list', { params });
}

export async function apiCallStatusCreate(data: any) {
    return await request('call/status', {
        method: 'POST',
        data
    });
}

export async function apiCallStatusUpdate(data: any) {
    return await request('call/status', {
        method: 'PUT',
        data
    });
}

export async function apiCallStatusDelete(id: number) {
    return await request(`call/status/${id}`, {
        method: 'DELETE'
    });
}

export async function apiCallReportTele(params: any) {
    return await request('call/tele-report', { params });
}

export async function apiCallReportCdr(params: any) {
    return await request('call/cdr', { 
        params: {
            ...params,
            startDate: params.time_range ? params.time_range[0] : undefined,
            endDate: params.time_range ? params.time_range[1] : undefined,
            time_range: undefined
        }
     });
}

export async function apiCallStatusDetails(params: any) {
    return request('call/status-details', { params });
}

export async function apiCallTMRReport() {
    return request('contact/tmr-report');
}