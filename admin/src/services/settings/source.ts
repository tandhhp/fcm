import { request } from "@umijs/max";

export async function apiSourceList(params: any) {
    return request('source/list', {
        params
    });
}

export async function apiSourceCreate(data: any) {
    return request('source', {
        method: 'POST',
        data
    });
}
 
export async function apiSourceUpdate(data: any) {
    return request('source', {
        method: 'PUT',
        data
    });
}

export async function apiSourceDelete(id: number) {
    return request(`source/${id}`, {
        method: 'DELETE'
    });
}

export async function apiSourceDetail(id: string) {
    return request(`source/${id}`);
}

export async function apiSourceOptions(params?: any) {
    return request<{
        label: string;
        value: string;
    }[]>('source/options', {
        params
    });
}

export async function apiSourceAssign(data: any) {
    return request('source/assign', {
        method: 'POST',
        data
    });
}

export async function apiAvailableSource() {
    return request(`source/availables`);
}

export async function apiTypeOfDataSources(params?: any) {
    return request(`source/type-of-data/sources`, {
        params
    });
}

export async function apiTypeOfDataOptions(params?: any) {
    return request(`source/type-of-data/options`, {
        params
    });
}

export async function apiGetTypeOfDataBySource(sourceId: number) {
    return request(`source/type-of-data-by-source-id`, {
        params: {
            sourceId
        }
    });
}

export async function apiSourceTeamOptions(params?: any) {
    return request(`source/team/options`, {
        params
    });
}

export async function apiSourceByTeamAndTypeOfData(params?: any) {
    return request(`source/options-by-type-of-data`, {
        params
    });
}