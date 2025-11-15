import { request } from "@umijs/max";

export async function queryComments() {
  return request(`comment/list`);
}

export async function activeComment(commentId: string) {
  return request(`comment/active/${commentId}`, {
    method: 'POST'
  });
}

export async function removeComment(commentId: string) {
  return request(`comment/delete/${commentId}`, {
    method: 'POST'
  });
}

export const apiAddTransFeedback = (data: any) => request(`order/add-transaction-feedback`, {
  method: 'POST',
  data
})