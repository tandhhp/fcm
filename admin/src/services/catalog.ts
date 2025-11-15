import { CatalogType, ESortOrder } from '@/constants';
import { request } from '@umijs/max';
import { SortOrder } from 'antd/lib/table/interface';
import { DataNode } from 'antd/lib/tree';

export async function getCatalog(id: string | undefined) {
  return request<API.Catalog>(`catalog/${id}`);
}

export async function addCatalog(data: API.Catalog) {
  return request('catalog/add', {
    method: 'POST',
    data,
  });
}

export async function listCatalog(params: {
  current?: number;
  pageSize?: number;
  type?: CatalogType;
  parentId?: string;
  locale?: string;
}, sort: Record<string, SortOrder>) {
  return request('catalog/list', {
    method: 'GET',
    params: {
      viewCount: sort.viewCount ? (sort.viewCount === 'ascend' ? ESortOrder.Ascending : ESortOrder.Descending) : ESortOrder.Unspecified,
      modifiedDate: sort.modifiedDate ? (sort.modifiedDate === 'ascend' ? ESortOrder.Ascending : ESortOrder.Descending) : ESortOrder.Unspecified,
      ...params,
    },
  });
}

export async function listTree() {
  return request<DataNode[]>('catalog/tree');
}

export async function deleteCatalog(id: string | undefined) {
  return request(`catalog/delete/${id}`, {
    method: 'POST',
  });
}

export async function saveCatalog(data: API.Catalog) {
  return request(`catalog/save`, {
    method: 'POST',
    data,
  });
}

export async function treeDrop(data: any) {
  return request(`catalog/tree-drop`, {
    method: 'POST',
    data,
  });
}

export async function queryViewCount() {
  return request(`catalog/view-count`);
}

export async function activeCatalog(id: string | undefined) {
  return request(`catalog/active/${id}`, {
    method: 'POST',
  });
}

export async function updateThumbnail(data: API.Catalog) {
  return request(`catalog/update-thumbnail`, {
    method: 'POST',
    data,
  });
}

export async function listTypes() {
  return request(`catalog/types`);
}

export async function listTagById(id: string | undefined) {
  return request(`catalog/list-tag/${id}`);
}

export async function listTagSelect(params: any) {
  return request(`catalog/list-tag-select`, {
    params,
  });
}

export async function listByTag(
  id: string | undefined,
  params: {
    current?: number;
    pageSize?: number;
  },
) {
  return request(`catalog/list-by-tag/${id}`, {
    params,
  });
}

export async function dataPieChart() {
  return request(`catalog/pie-chart`)
}

export async function saveProductImage(urls: string[], catalogId?: string | string[]) {
  return request(`/product/image/save`, {
    method: 'POST',
    data: {
      urls,
      catalogId
    }
  })
}

export async function queryProductImage(catalogId?: string | string[]) {
  return request(`/product/image/${catalogId}`)
}

export async function queryCatalogSelect(params: any) {
  return request(`/catalog/form-select`, {
    params
  });
}

export async function saveProduct(data: any) {
  return request(`/product/save`, {
    method: 'POST',
    data
  })
}

export async function queryProduct(catalogId?: string | string[]) {
  return request(`/product/${catalogId}`);
}

export async function saveBrand(data: any) {
  return request(`/product/brand/save`, {
    method: 'POST',
    data
  });
}

export async function apiTopView(type: string) {
  return request(`/catalog/top-view`, {
    params: {
      type
    }
  })
}

export const apiTourContent = (catalogId: string) => {
  return request(`tour/${catalogId}`);
}

export const apiTourContentSave = (data: any) => {
  return request(`tour/save`, {
    method: 'POST',
    data
  });
}

export const apiItinernaryAdd = (data: any) => {
  return request(`tour/itinerary/add`, {
    method: 'POST',
    data
  });
}

export const apiItinernaryUpadte = (data: any) => request(`tour/itinerary/update`, {
  method: 'POST',
  data
});

export const apiItineraryList = (id?: string) => request(`tour/itinerary/${id}`);

export const apiItineraryDelete = (id?: string) => request(`tour/itinerary/delete/${id}`, {
  method: 'POST'
});

export const apiLocations = () => request(`tour/cities`);

export const apiSaveTourPoint = (data: any) => request(`tour/save-point`, {
  method: 'POST',
  data
});