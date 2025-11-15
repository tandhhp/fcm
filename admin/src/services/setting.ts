import { request } from '@umijs/max';

export async function listSetting() {
  return request(`setting/list`);
}

export async function saveLayoutHead(data: any) {
  return request(`setting/layout/head/save`, {
    method: 'POST',
    data,
  });
}

export async function saveTelegram(id: string | undefined, data: API.Telegam) {
  return request(`setting/telegram/save/${id}`, {
    method: 'POST',
    data,
  });
}

export async function getTelegram(id: string | undefined) {
  return request(`setting/telegram/${id}`);
}

export async function testTelegram(data: any) {
  return request(`setting/telegram/test`, {
    method: 'POST',
    data,
  });
}

export async function getFooter(id: string | undefined) {
  return request(`setting/footer/${id}`);
}

export async function saveSocial(data: any) {
  return request(`setting/social/save`, {
    method: 'POST',
    data,
  });
}

export async function getSocial(id: string | undefined) {
  return request(`setting/social/${id}`);
}

//#region Facebook
export async function facebookGet(id: string | undefined) {
  return request(`setting/facebook/${id}`);
}
export async function facebookSave(data: API.Facebook) {
  return request(`setting/facebook/save`, {
    method: 'POST',
    data,
  });
}
//#endregion

//#region SendGrid

export async function getSendGrid() {
  return request(`setting/sendgrid`);
}

export async function saveSendGrid(data: any) {
  return request(`setting/sendgrid/save`, {
    method: 'POST',
    data,
  });
}

//#endregion

export async function listSidebarWork(params: any) {
  return request(`setting/sidebar`, {
    params,
  });
}

export async function workAddSetting(data: any) {
  return request(`setting/work/add`, {
    method: 'POST',
    data,
  });
}

export async function getSetting(name: string | undefined) {
  return request(`setting/${name}`);
}

export async function getHeader(id: string | undefined) {
  return request(`setting/header/${id}`);
}

export async function saveSetting(data: any) {
  return request(`setting/save`, {
    method: 'POST',
    data,
  });
}

export async function querySaveSetting(normalizedName: string, data: any) {
  return request(`setting/unix/save/${normalizedName}`, {
    method: 'POST',
    data,
  });
}

export async function graphFacebook(query: string) {
  return request(`setting/graph-api-explorer?query=${query}`);
}

export async function queryThemeOptions() {
  return request(`setting/themes/options`);
}