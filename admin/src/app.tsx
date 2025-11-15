import { RequestConfig, RequestOptions, setLocale } from '@umijs/max';
import '../style.less';
import logo from './assets/logo-icon.png';
import { queryCurrentUser } from './services/user';
import { history } from '@umijs/max';
import { RunTimeLayoutConfig } from '@umijs/max';
import { AvatarDropdown, SelectLang } from './components';
import { message } from 'antd';
import NotificationBadge from './components/right-content/notification-badge';
import Footer from './components/layout/footer';

const loginPath = '/accounts/login';

export async function getInitialState(): Promise<{
  avatar?: string;
  name?: string;
  currentUser?: API.User;
  fetchUserInfo?: () => Promise<API.User | undefined>;
}> {
  const fetchUserInfo = async () => {
    try {
      const response = await queryCurrentUser();
      return response.data;
    } catch (error) {
      history.push(loginPath);
    }
    return undefined;
  };
  const { location } = history;
  if (location.pathname !== loginPath) {
    const currentUser = await fetchUserInfo();
    return {
      fetchUserInfo,
      name: currentUser?.userName,
      avatar: currentUser?.avatar,
      currentUser: currentUser,
    };
  }
  return {
    fetchUserInfo,
    name: '@umijs/max',
  };
}
export const layout: RunTimeLayoutConfig = ({ initialState }) => {
  return {
    logo: logo,
    menu: {
      locale: false,
    },
    layout: 'mix',
    token: {
      sider: {
        colorMenuBackground: '#000305ff',
        colorBgMenuItemHover: '#ca8a04',
        colorTextMenu: '#FFF',
        colorTextMenuSelected: '#FFF',
        colorTextMenuItemHover: '#FFF',
        colorTextMenuActive: '#FFFFFF',
        colorBgMenuItemSelected: '#d89a15ff',
        colorTextMenuSecondary: '#FFFFFF',
        colorTextMenuTitle: '#FFFFFF',
        colorTextSubMenuSelected: '#FFFFFF'
      }
    },
    footerRender: () => <Footer />,
    onPageChange: () => {
      const { location } = history;
      if (!initialState?.currentUser && location.pathname !== loginPath) {
        setLocale('vi-VN');
        history.push(loginPath);
      }
    },
    rightContentRender: () => (
      <div className='flex gap-4 items-center'>
        <NotificationBadge />
        <SelectLang />
        <AvatarDropdown menu />
      </div>
    )
  };
};

export const request: RequestConfig = {
  requestInterceptors: [
    (config: RequestOptions) => {
      const token = localStorage.getItem('wf_token');
      config.baseURL = new URL(`api/`, 'https://api.1stclass.com.vn/').href;
      //config.baseURL = new URL(`api/`, 'https://localhost:52588/').href;
      config.headers = {
        authorization: `Bearer ${token}`,
      };
      return config;
    },
  ],
  responseInterceptors: [
    (response: any) => {
      if (response.status === 200) {
        if (response.data.succeeded === false) {
          message.error(response.data.message);
          throw new Error(response.data.message);
        }
      }
      return response;
    },
  ],
  errorConfig: {
    errorHandler: (error: any) => {
      if (error.response.data) {
        message.error(error.response.data)
      }
    }
  },
};
