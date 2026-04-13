import { ProLayoutProps } from '@ant-design/pro-components';

const { COMPANY_NAME } = process.env;

const defaultSettings: ProLayoutProps = {
  navTheme: 'light',
  contentWidth: 'Fluid',
  fixedHeader: false,
  fixSiderbar: true,
  title: COMPANY_NAME
};

export default defaultSettings;