import { request } from '@umijs/max';

export const apiFormList = (params: any) => request(`form/list`, { params });

export const apiFormDetail = (id: string) => request(`form/${id}`);

export const apiDeleteForm = (id: string) => request(`form/delete/${id}`, {
    method: 'POST'
});

export const apiTourUpdateStatus = (data: any) => request(`form/update-status`, {
    method: 'POST',
    data
});

export const apiCxCreateForm = (data: any) => request(`form/cx-create`, {
    method: 'POST',
    data
});

export const apiResendEmail = (id: string) => request(`form/resend-transaction/${id}`, {
    method: 'POST'
})