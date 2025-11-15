import { request } from '@umijs/max';

export async function listWork() {
  return request(`work/list`);
}

export async function addWorkContent(data: any) {
  return request(`work/add`, {
    method: 'POST',
    data,
  });
}

export async function listWorkContent(id: string | undefined) {
  return request(`work/list/${id}`);
}

export async function activeWork(id: string | undefined) {
  return request(`work/active/${id}`, {
    method: 'POST',
  });
}

export async function listChildWorkContent(id: string | undefined) {
  return request(`work/list-child/${id}`);
}

export async function addChildWorkContent(data: API.WorkContent) {
  return request(`work/child/add`, {
    method: 'POST',
    data,
  });
}

export async function updateTitle(data: any) {
  return request(`work/save/title`, {
    method: 'POST',
    data,
  });
}

export async function saveArguments(id: string | undefined, data: any) {
  return request(`work/save/${id}`, {
    method: 'POST',
    data,
  });
}

export async function getArguments(id?: string | string[]) {
  return request(`work/arguments/${id}`);
}

export async function addColumn(data: any) {
  return request(`work/column/add`, {
    method: 'POST',
    data,
  });
}

export async function getWorkContent(id: string | undefined) {
  return request(`work/${id}`);
}

export async function getWorkSummary(id: string | undefined) {
  return request(`work/summary/${id}`);
}

export async function updateWorkSummary(data: API.WorkContent) {
  return request(`work/summary/update`, {
    method: 'POST',
    data,
  });
}

export async function deleteWorkContent(
  workContentId?: string,
  catalogId?: string | undefined,
) {
  return request(`work/delete`, {
    method: 'POST',
    data: {
      workContentId,
      catalogId,
    },
  });
}

export async function deleteWork(id: string | undefined) {
  return request(`work/delete/${id}`, {
    method: 'POST',
  });
}

export async function listCss() {
  return request(`style/list`);
}

export async function addCss(data: API.WorkItem) {
  data.sortOrder = Number(data.sortOrder);
  return request(`style/add`, {
    method: 'POST',
    data,
  });
}

export async function getCss(id: string | undefined) {
  if (!id) {
    return request(`style`);
  }
  return request(`style/${id}`);
}

export async function saveCss(data: API.WorkItem) {
  return request(`style/save`, {
    method: 'POST',
    data,
  });
}

export async function deleteCss(id: string) {
  return request(`style/delete/${id}`, {
    method: 'POST',
  });
}

export async function uploadImage(id: string) {
  return request(`image/upload/${id}`, {
    method: 'POST',
  });
}

export async function getNavbar(id: string | undefined) {
  return request<API.Navbar>(`work/navbar/${id}`);
}

export async function saveNavbar(data: API.Navbar) {
  return request(`work/navbar/save`, {
    method: 'POST',
    data,
  });
}

export async function addNavbarItem(data: API.NavItem) {
  return request(`work/navbar/item/add`, {
    method: 'POST',
    data,
  });
}

export async function listNavItem(id: string | undefined) {
  return request(`work/navbar/item/list/${id}`);
}

export async function saveNavItem(id: string | undefined, data: CPN.Link) {
  return request(`work/navbar/item/save/${id}`, {
    method: 'POST',
    data,
  });
}

export async function deleteNavItem(
  linkId: string,
  workId: string | undefined,
) {
  return request(`work/navbar/item/delete`, {
    method: 'POST',
    data: {
      linkId,
      workId,
    },
  });
}

export async function listUnuse(params: any) {
  return request(`work/list-unuse`, {
    params
  })
}

export async function getBlockEditor(id: string | undefined) {
  return request(`work/block-editor/${id}`);
}

export async function saveBlockEditor(data: any) {
  return request(`work/block-editor/save`, {
    method: 'POST',
    data,
  });
}

export async function getCard(id: string | undefined) {
  return request(`work/card/${id}`);
}

export async function saveCard(data: any) {
  return request(`work/card/save`, {
    method: 'POST',
    data,
  });
}

export async function getRow(id: string | undefined) {
  return request(`work/row/${id}`);
}

export async function saveRow(data: any) {
  return request(`work/row/save`, {
    method: 'POST',
    data,
  });
}

export async function getListColumn(id: string | undefined) {
  return request(`work/column/list/${id}`);
}

export async function getChildList(
  params: {
    current?: number;
    pageSize?: number;
  },
  id: string | undefined,
) {
  return request(`work/child/list/${id}`);
}

//#region Look book
export async function addLookBook(data: API.WorkContent) {
  return request(`work/lookbook/add`, {
    method: 'POST',
    data,
  });
}
//#endregion

export async function listTag(params: any) {
  return request(`work/tag/list`, {
    params,
  });
}

export async function addItem(data: any) {
  return request(`work/item/add`, {
    method: 'POST',
    data,
  });
}

export async function deleteItem(data: Entity.WorkItem) {
  return request(`work/item/delete`, {
    method: 'POST',
    data,
  });
}

export async function saveLink(id: string | undefined, data: CPN.Link) {
  return request(`work/link/save/${id}`, {
    method: 'POST',
    data,
  });
}

export async function getLink(id: string | undefined) {
  return request(`work/link/${id}`);
}

export async function getGoogleMap(id: string | undefined) {
  return request(`work/google-map/${id}`);
}

export async function sortWork(workIds: string[]) {
  return request(`/work/sort`, {
    method: 'POST',
    data: workIds
  })
}

export async function sortChild(workIds: string[]) {
  return request(`/work/child/sort`, {
    method: 'POST',
    data: workIds
  })
}

export async function apiUnuseWorks(params: any) {
  return request(`/work/unuse-works`, {
    params
  })
}