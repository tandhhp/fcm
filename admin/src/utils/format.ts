import dayjs from 'dayjs';

export function trim(str: string) {
  return str.trim();
}

export function absolutePath(str?: string) {
  return new URL(str || '', localStorage.getItem('wf_URL') || '').href;
}

export function formatDate(str?: Date) {
  if (!str) {
    return '-';
  }
  return dayjs(str).format('DD/MM/YYYY hh:mm:ss');
}

export const waitTime = (time: number = 100) => {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve(true);
    }, time);
  });
};