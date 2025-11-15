import { ProForm, ProFormText } from '@ant-design/pro-components';
import { FormattedMessage } from '@umijs/max';
import { Divider, Typography } from 'antd';

const ContactFormSetting: React.FC = () => {
  const onFinish = async (values: any) => {
    console.log(values);
  };

  return (
    <ProForm onFinish={onFinish}>
      <Typography.Title level={4}>Email</Typography.Title>
      <ProFormText name="received" label="Received" />
      <Typography.Title level={4}>
        <FormattedMessage id="menu.settings.sendGrid" />
      </Typography.Title>
      <ProFormText name="template" label="Template" />
      <Divider />
      <Typography.Title level={4}>
        <FormattedMessage id="menu.settings.telegram" />
      </Typography.Title>
      <ProFormText name="chatId" label="Chat ID" />
    </ProForm>
  );
};

export default ContactFormSetting;
