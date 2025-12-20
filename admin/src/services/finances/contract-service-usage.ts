import { request } from "@umijs/max";

export async function apiContractServiceUsageList(contractId: string, params: any) {
    return request<API.ListResult<API.ContractServiceUsage>>(`ContractServiceUsage/list/${contractId}`, {
        method: 'GET',
        params,
    });
}

export async function apiContractServiceUsageCreate(data: API.ContractServiceUsage) {
    return request(`ContractServiceUsage`, {
        method: 'POST',
        data,
    });
}

export async function apiContractServiceUsageDelete(id: string) {
    return request(`ContractServiceUsage/${id}`, {
        method: 'DELETE',
    });
}

export async function apiContractServiceUsageUpdate(data: API.ContractServiceUsage) {
    return request(`ContractServiceUsage`, {
        method: 'PUT',
        data,
    });
}