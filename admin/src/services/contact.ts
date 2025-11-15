import { request } from '@umijs/max';

export async function listContact(params: any) {
  return request(`contact/list`, { 
    params: {
      ...params,
      fromDate: params?.lastCall ? params.lastCall[0] : undefined,
      toDate: params?.lastCall ? params.lastCall[1] : undefined,
      lastCall: undefined
    }
   });
}

export async function deleteContact(id: string) {
  return request(`contact/${id}`, {
    method: 'DELETE',
  });
}

export const apiListContactActivity = (id?: string) => request(`contact/activity/list/${id}`);

export const apiContactActivityAdd = (data: any) => request(`contact/activity/add`, {
  method: 'POST',
  data
})

export const apiContactActivityUpdate = (data: any) => request(`contact/activity/update`, {
  method: 'POST',
  data
})

export const apiContactActivityDelete = (id?: string) => request(`contact/activity/delete/${id}`, {
  method: 'POST'
})

export const apiAddEvent = (data: any) => request(`contact/event/add`, {
  method: 'POST',
  data
})

export const apiUpdateEvent = (data: any) => request(`contact/event/update`, {
  method: 'POST',
  data
})

export const apiLeadOption = (params: any) => request(`contact/lead-options`, { params });

export const apiAddLead = (data: any) => request(`contact/lead/add`, {
  method: 'POST',
  data
})

export const apiListSubLead = (id: string) => request(`contact/subleads/${id}`);

export const apiAddSubLead = (data: any) => request(`contact/sublead/add`, {
  method: 'POST',
  data
});

export const apiUpdateSubLead = (data: any) => request(`contact/sublead/update`, {
  method: 'POST',
  data
});

export const apiDeleteSubLead = (id: any) => request(`contact/sublead/delete/${id}`, {
  method: 'POST'
});

export const apiAddEventLead = (data: any) => request(`contact/event/add-lead`, {
  method: 'POST',
  data
})

export const apiRemoveEventLead = (data: any) => request(`contact/event/remove-lead`, {
  method: 'POST',
  data
})

export async function listLead(params: any) {
  return request(`contact/lead/list`, { params });
}

export const apiUpdateLeadStatus = (data: any) => request(`contact/lead/status`, {
  method: 'POST',
  data
})

export const apiDeleteLead = (id: string) => request(`contact/lead/delete/${id}`, {
  method: 'POST'
})

export const apiGetEvent = (id: string) => request(`contact/event/${id}`);

export const apiUsersInEvent = (params: any) => request(`contact/users-in-event`, { params });

export const apiAddLeadFeedback = (data: any) => request(`contact/feedback/add`, {
  method: 'POST',
  data
})

export const apiUpdateLeadFeedback = (data: any) => request(`contact/feedback/update`, {
  method: 'POST',
  data
})

export const apiGetLeadFeedback = (id: string) => request(`contact/feedback/${id}`);

export const apiGetQueueCardHolder = (params: any) => request(`contact/card-holder-queue-list`, { params });

export const apiQueueCardHolderStatus = (data: any) => request(`contact/card-holder-queue-status`, {
  method: 'POST',
  data
});

export const apiGetFloorOptions = () => request(`contact/floor-options`);

export const apiGetTableOptions = (params: any) => request(`contact/table-options`, { params });

export const apiGetSmDosOptions = () => request(`user/sm-dos-options`);

export const apiMyKeyin = (params: any) => request(`contact/my-keyin-list`, { params });

export const apiUpdateEventDate = (data: any) => request(`contact/move-event-date`, {
  method: 'POST',
  data
});

export const apiCheckoutTable = (data: any) => request(`contact/checkout-table`, {
  method: 'POST',
  data
});

export const apiLeadLeadHistory = (params: any, leadId?: string) => request(`contact/list-lead-history/${leadId}`, { params });

export async function apiContractAdd(data: any) {
  return request(`user/add-sub-contract`, {
    method: 'POST',
    data
  })
}

export async function apiContractDelete(id: string) {
  return request(`user/delete-sub-contract/${id}`, {
    method: 'POST'
  })
}

export async function apiApproveDebt(data: any) {
  return request(`debt/approve`, {
    method: 'POST',
    data
  })
}

export async function apiRejectDebt(data: any) {
  return request(`debt/reject`, {
    method: 'POST',
    data
  })
}

export async function apiKeyInStatusOptions() {
  return request(`contact/status-options`);
}

export async function apiTopupStatusOptions() {
  return request(`debt/status-options`);
}

export async function apiContactBlock(data: any) {
  return request(`contact/block`, {
    method: 'POST',
    data
  })
}

export async function apiContactBlacklist(params: any) {
  return request(`contact/blacklist`, { params });
}

export async function apiContactCreate(data: any) {
  return request(`contact`, {
    method: 'POST',
    data
  })
}

export async function apiContactUpdate(data: any) {
  return request(`contact`, {
    method: 'PUT',
    data
  })
}

export async function apiContactStatistics() {
  return request(`contact/statistics`);
}

export async function apiContactDetail(id?: string) {
  return request(`contact/${id}`);
}

export async function apiContactBook(data: any) {
  return request(`contact/book`, {
    method: 'POST',
    data
  })
}

export async function apiContactImport(data: FormData) {
  return request(`contact/import`, {
    method: 'POST',
    data,
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  })
}

export async function apiContactUnassignedList(params: any) {
  return request(`contact/unassigned-list`, { params });
}

export async function apiContactAssignSource(data: any) {
  return request('contact/assign-source', {
    method: 'POST',
    data
  });
}

export async function apiContactNeedConfirm2(params: any) {
  return request(`contact/need-confirms`, { params });
}

export async function apiContactConfirm2(id: string) {
  return request(`contact/confirm2/${id}`, {
    method: 'POST',
  })
}