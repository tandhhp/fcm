import {
  CalendarOutlined,
  LogoutOutlined,
  SettingOutlined,
  UserOutlined,
} from '@ant-design/icons';
import { history, useModel } from '@umijs/max';
import { Avatar, Spin } from 'antd';
import { stringify } from 'querystring';
import type { MenuInfo } from 'rc-menu/lib/interface';
import React, { useCallback } from 'react';
import { flushSync } from 'react-dom';
import HeaderDropdown from '../header-dropdown';

export type GlobalHeaderRightProps = {
  menu?: boolean;
  children?: React.ReactNode;
};

export const AvatarName = () => {
  const { initialState } = useModel('@@initialState');
  const { currentUser } = initialState || {};
  return <div className='mr-4 px-2'>{currentUser?.name}</div>;
};

const AvatarLogo = () => {
  const { initialState } = useModel('@@initialState');
  const { currentUser } = initialState || {};
  if (!currentUser?.avatar) {
    return <Avatar size="small" icon={<UserOutlined />} alt="avatar" />
  }
  return <Avatar size="small" src={currentUser?.avatar} alt="avatar" />;
};

export const AvatarDropdown: React.FC<GlobalHeaderRightProps> = ({ menu }) => {

  const loginOut = async () => {
    localStorage.removeItem('wf_token');
    localStorage.removeItem('wf_URL');
    const { search, pathname } = window.location;
    const urlParams = new URL(window.location.href).searchParams;
    const redirect = urlParams.get('redirect');
    if (window.location.pathname !== '/accounts/login' && !redirect) {
      history.replace({
        pathname: '/accounts/login',
        search: stringify({
          redirect: pathname + search,
        }),
      });
    }
  };
  const { initialState, setInitialState } = useModel('@@initialState');

  const onMenuClick = useCallback(
    (event: MenuInfo) => {
      const { key } = event;
      if (key === 'logout') {
        flushSync(() => {
          setInitialState((s) => ({ ...s, currentUser: undefined }));
        });
        loginOut();
        return;
      }
      if (key === 'profile') {
        history.push(`/user/profile`);
        return;
      } else if (key === 'settings') {
        history.push(`/user/center/${initialState?.currentUser?.id}`);
        return;
      }
      if (key === 'calendar') {
        history.push('/calendar');
        return;
      }
      history.push(`/accounts/${key}`);
    },
    [setInitialState],
  );

  const loading = (
    <Spin
      size="small"
      style={{
        marginLeft: 8,
        marginRight: 8,
      }}
    />
  );

  if (!initialState) {
    return loading;
  }

  const { currentUser } = initialState;

  if (!currentUser || !currentUser.userName) {
    return loading;
  }

  const menuItems = [
    {
      key: 'profile',
      icon: <UserOutlined />,
      label: 'Hồ sơ',
    },
    {
      key: 'calendar',
      icon: <CalendarOutlined />,
      label: 'Lịch của tôi',
    },
    {
      type: 'divider' as const,
    },
    {
      key: 'logout',
      icon: <LogoutOutlined />,
      label: 'Đăng xuất',
      danger: true
    },
  ];

  return (
    <>
      <HeaderDropdown
        menu={{
          selectedKeys: [],
          onClick: onMenuClick,
          items: menuItems,
        }}
      >
        <div className='cursor-pointer flex items-center'>
          <AvatarLogo />
          <AvatarName />
        </div>
      </HeaderDropdown>
    </>
  );
};
