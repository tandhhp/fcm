const getBaseUrl = () => localStorage.getItem('wf_URL');

const BASE_URL = getBaseUrl();

export { BASE_URL }