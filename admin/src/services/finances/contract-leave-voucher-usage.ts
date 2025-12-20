import { request } from "@umijs/max";

export async function apiContractLeaveVoucherUsageList(contractId: string, params: any) {
    return request<API.ListResult<API.ContractLeaveVoucherUsage>>(`ContractLeaveVoucherUsage/list/${contractId}`, {
        method: 'GET',
        params,
    });
}

export async function apiContractLeaveVoucherUsageCreate(data: API.ContractLeaveVoucherUsage) {
    return request(`ContractLeaveVoucherUsage`, {
        method: 'POST',
        data,
    });
}

export async function apiContractLeaveVoucherUsageDelete(id: string) {
    return request(`ContractLeaveVoucherUsage/${id}`, {
        method: 'DELETE',
    });
}

export async function apiContractLeaveVoucherUsageUpdate(data: API.ContractLeaveVoucherUsage) {
    return request(`ContractLeaveVoucherUsage`, {
        method: 'PUT',
        data,
    });
}