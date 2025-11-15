import { request } from '@umijs/max';

export async function login(data: any) {
  return request(`user/password-sign-in`, {
    method: 'POST',
    data,
  });
}

export async function queryCurrentUser() {
  return request(`user`);
}

export async function apiUserDetail(id: string | undefined) {
  return request(`user/${id}`);
}

export async function apiChangeAvatar(data: any) {
  return request(`user/change-avatar`, {
    data,
    method: 'POST',
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  })
}

export const apiGetStatisticsUser = (id: string) => request(`user/card-holder/statistics/${id}`);

export const apiGetUserInfo = (id: string) => request(`user/info/${id}`);

export const apiUpdateUserInfo = (data: any) => request(`user/info/save`, {
  method: 'POST',
  data
})

export async function createEmployee(data: any) {
  return request(`user/create-member`, {
    method: 'POST',
    data,
  });
}


export async function apiUserUpdate(data: any) {
  return request(`user/update`, {
    method: 'POST',
    data,
  });
}

export async function listUser(params: any) {
  return request(`user/list`, { params });
}

export async function listTrainerUser(params: any) {
  return request(`user/trainer/list-user`, { params });
}

export async function changePassword(data: any) {
  return request(`user/change-password`, {
    method: 'POST',
    data,
  });
}

export async function deleteUser(id?: string) {
  return request(`user/delete/${id}`, {
    method: 'POST'
  })
}

export const apiLockUser = (id: string) => request(`user/leave/${id}`, {
  method: 'POST'
})

export const apiUnLockUser = (id: string) => request(`user/unlock/${id}`, {
  method: 'POST'
})


export async function addToRole(data: any) {
  return request(`user/add-to-role`, {
    method: 'POST',
    data
  });
}

export async function getUserInRoles(params: any, roleName?: string | string[]) {
  return request(`user/users-in-role/${roleName}`, { params });
}

export async function apiConfirmEmail(id?: string | string[]) {
  return request(`user/confirm-email/${id}`, {
    method: 'POST'
  });
}

export const apiUpdateLoyalty = (data: any) => request(`user/loyalty/save`, {
  method: 'POST',
  data
})

export async function apiGetMyTransactions(userId?: string) {
  return request(`user/transactions/${userId}`);
}

export const apiCardOptions = () => request(`user/card/options`);

export const apiDeleteCard = (id?: string) => request(`user/card/delete/${id}`, { method: 'POST' });

export const apiSubUserList = (id?: string) => request(`user/sub-user/list/${id}`);

export const apiSubUserAdd = (data: any) => request(`user/sub-user/add`, {
  method: 'POST',
  data
});

export const apiSubUserUpdate = (data: any) => request(`user/sub-user/update`, {
  method: 'POST',
  data
});

export const apiSubUserDelete = (id: string) => request(`user/sub-user/delete/${id}`, {
  method: 'POST'
})

export const apiRoleOptions = () => request(`user/role/options`);

export const apiDosOptions = () => request(`user/dos/options`);

export const apiSmOptions = (id?: string) => request(`user/sm/options/${id}`);

export const apiUserByRoleOptions = (name: string) => request(`role/user-options-in-role/${name}`);

export const apiLotaltyApproveList = (params: any) => request(`user/loyalty/list-approve`, { params });

export const apiApproveDeposit = (id: string) => request(`user/loyalty/approve/${id}`, {
  method: 'POST'
})

export const apiRejectDeposit = (id: string) => request(`user/loyalty/reject/${id}`, {
  method: 'POST'
})

export const apiGetSalesOptions = () => request(`user/sales/options`);

export const apiAchievementList = () => request(`user/achievements`);

export const apiAchievementListById = (id: string) => request(`user/achievements-by-user/${id}`);

export const apiExportLead = (data: any) => request(`contact/export-lead`, {
  responseType: 'blob',
  method: 'POST',
  data
});

export const apiSalesOptions = (params?: any) => request(`user/sales/options`, { params });

export const apiSalesWithSmOptions = () => request(`user/sales-with-sm-options`);

export const apiTeleWithTmOptions = () => request(`user/tele-with-tm-options`);

export const apiSendEmails = (data: any) => request(`user/send-emails`, {
  method: 'POST',
  data
})

export const apiCardHolderOptions = () => request(`user/card-holder/options`);

export const apiListTopSales = (params: any) => request(`user/list-top-sales`, { params });

export const apiListTopup = (params: any) => request(`user/list-topup`, { params });

export const apiApproveTopup = (data: any) => request(`user/approve-topup`, {
  method: 'POST',
  data
});

export const apiAmountReport = () => request(`report/amounts`);

export const apiSaleChartData = () => request(`user/sale-chart`);

export const apiSmChartData = () => request(`user/sm-chart`);

export const apiSalesListReport = (year: number) => request(`report/sales`, {
  params: { year }
});

export const apiUserChangeList = (params: any) => request(`user/changes-list`, { params });

export const apiUserChangeApprove = (id: string) => request(`user/approve-changes/${id}`, {
  method: 'POST'
});

export const apiAchievementAdd = (data: any) => request(`user/achievement/add`, {
  method: 'POST',
  data
});

export const apiListApproveAchievent = (params: any) => request(`user/achievement/list-approve`, { params });

export const apiAchieventApprove = (id: string) => request(`user/achievement/approve/${id}`, {
  method: 'POST'
});

export const apiAchieventRemove = (id: string) => request(`user/achievement/delete/${id}`, {
  method: 'POST'
});

export const apiLeadFeedback = (params: any) => request(`contact/feedback/list`, { params });

export const apiKeyinByTelesale = (params: any) => request(`report/keyin-by-telesale`, { params });

export async function apiSetPassword(data: any) {
  return request(`user/set-password`, {
    method: 'POST',
    data
  })
}

export async function apiUpdateContractCode(data: any) {
  return request(`user/change-contract-code`, {
    method: 'POST',
    data
  })
}

export async function apiMyTeam(params: any) {
  return request(`user/team`, { params })
}

export async function apiUserPointByCardHolder(params: any) {
  return request(`user/points`, { params });
}

export async function apiUserProfileUpdate(data: any) {
  return request(`user/update-profile`, {
    method: 'POST',
    data
  });
}

export async function apiLoanPoint(data: any) {
  return request(`user/loan-point`, {
    method: 'POST',
    data
  });
}

export async function apiUserOptions(params?: any) {
  return request(`user/options`, { params });
}

export async function apiUserStatistics() {
  return request(`user/statistics`);
}

export async function apiUserProfile() {
  return request(`user/profile`);
}

export async function apiUserTelesalesOptions(params?: any) {
  return request(`user/telesales/options`, { params });
}

export async function apiUserTelesalesManagerOptions(params?: any) {
  return request(`user/telesales-manager/options`, { params });
}

export async function apiUserImport(data: FormData) {
  return request(`user/import`, {
    method: 'POST',
    data,
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  });
}

export async function apiUserExport(params?: any) {
  return request(`user/export`, {
    method: 'GET',
    params,
    responseType: 'blob'
  });
}

export async function apiUserClaims(params: any) {
  return request(`user/claims`, { params });
}

export async function apiUserClaimUpdate(data: any) {
  return request(`user/claim`, {
    method: 'POST',
    data
  });
}

export async function apiUserListDos(params: any) {
  return request(`user/list-dos`, { params });
}

export async function apiUserSetDos(data: any) {
  return request(`user/set-dos`, {
    method: 'POST',
    data
  });
}

export async function apiUserListTele(params: any) {
  return request(`user/list-tele`, { params });
}