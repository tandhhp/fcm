import { login } from '@/services/user';
import {
  HomeOutlined,
  LockOutlined,
  UserOutlined,
} from '@ant-design/icons';
import {
  DefaultFooter,
  LoginForm,
  ProFormCheckbox,
  ProFormText,
} from '@ant-design/pro-components';
import { FormattedHTMLMessage, Link, SelectLang, setLocale, useIntl } from '@umijs/max';
import { history, useModel } from '@umijs/max';
import { message, Tabs } from 'antd';
import React, { useState } from 'react';
import { flushSync } from 'react-dom';
import logo from '../../../assets/logo-icon.png';
import '../index.css';
import { Helmet } from '@umijs/max';
import Settings from '../../../../config/defaultSetting';

const Login: React.FC = () => {
  const [type, setType] = useState<string>('account');
  const { initialState, setInitialState } = useModel('@@initialState');

  const intl = useIntl();

  const fetchUserInfo = async () => {
    const userInfo = await initialState?.fetchUserInfo?.();
    if (userInfo) {
      flushSync(() => {
        setInitialState((s) => ({
          ...s,
          currentUser: userInfo,
        }));
      });
    }
  };

  const handleSubmit = async (values: any) => {
    try {
      values.isAdmin = true;
      const msg = await login({ ...values, type });
      setLocale('vi-VN');
      localStorage.setItem('wf_token', msg.token);
      await fetchUserInfo();
      const urlParams = new URL(window.location.href).searchParams;
      history.push(urlParams.get('redirect') || '/');
    } catch (error) {
      message.error('Đăng nhập thất bại!');
    }
  };

  return (
    <div className='h-screen'>
      <Helmet>
        <title>{intl.formatMessage({ id: 'menu.login', defaultMessage: 'Login', })} - {Settings.title}</title>
      </Helmet>
      <div className='fixed' style={{
        left: 10,
        top: 10
      }}>
        <SelectLang />
      </div>
      <div>
        <div className="flex items-center relative h-screen flex-col justify-center">
          <div className='flex flex-col items-center justify-center w-full'>
            <LoginForm
              logo={<img alt="logo" src={logo} />}
              title="First Class Membership"
              subTitle="Thân tâm an trú - Cuộc sống thượng lưu"
              initialValues={{
                autoLogin: true,
              }}
              onFinish={async (values) => {
                await handleSubmit(values as any);
              }}
            >
              <Tabs
                activeKey={type}
                onChange={setType}
                centered
                items={[
                  {
                    key: 'account',
                    label: intl.formatMessage({ id: 'pages.login.account', defaultMessage: 'Account' }),
                  }
                ]}
              />
              {type === 'account' && (
                <>
                  <ProFormText
                    name="username"
                    fieldProps={{
                      size: 'large',
                      prefix: <UserOutlined />,
                    }}
                    placeholder="Tài khoản"
                    rules={[
                      {
                        required: true,
                        message: 'Vui lòng nhập tài khoản!',
                      },
                    ]}
                  />
                  <ProFormText.Password
                    name="password"
                    fieldProps={{
                      size: 'large',
                      prefix: <LockOutlined />,
                    }}
                    placeholder={intl.formatMessage({
                      id: 'pages.login.password',
                    })}
                    rules={[
                      {
                        required: true,
                        message: 'Vui lòng nhập mật khẩu!',
                      },
                    ]}
                  />
                </>
              )}

              <div
                style={{
                  marginBottom: 24,
                }}
              >
                <ProFormCheckbox noStyle name="autoLogin">
                  <FormattedHTMLMessage id="pages.login.rememberMe" />
                </ProFormCheckbox>
                <div
                  style={{
                    float: 'right',
                  }}
                >
                  <Link to="#">
                    <FormattedHTMLMessage id="pages.login.forgotPassword" />
                  </Link>
                </div>
              </div>
            </LoginForm>
          </div>
          <DefaultFooter
            style={{
              background: 'none',
            }}
            copyright="Powered by DefZone.Net"
            links={[
              {
                key: 'github',
                title: <HomeOutlined />,
                href: 'https://1stclass.com.vn',
                blankTarget: true,
              },
              {
                key: 'home',
                title: 'Trang chủ',
                href: 'https://1stclass.com.vn',
                blankTarget: true,
              },
            ]}
          />
        </div>
      </div>
    </div>
  );
};

export default Login;