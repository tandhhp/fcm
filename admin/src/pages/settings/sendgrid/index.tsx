import { getSendGrid, saveSendGrid } from '@/services/setting';
import {
  PageContainer,
  ProCard,
  ProForm,
  ProFormInstance,
  ProFormText,
} from '@ant-design/pro-components';
import { Divider, message, Typography } from 'antd';
import { useEffect, useRef } from 'react';

const SendGrid: React.FC = () => {
  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    getSendGrid().then((response) => {
      formRef.current?.setFields([
        {
          name: 'apiKey',
          value: response.apiKey,
        },
        {
          name: 'name',
          value: response.from?.name,
        },
        {
          name: 'email',
          value: response.from?.email,
        },
      ]);
    });
  }, []);

  const onFinish = async (values: any) => {
    const body = {
      apiKey: values.apiKey,
      from: {
        email: values.email,
        name: values.name,
      },
    };
    const response = await saveSendGrid(body);
    if (response.succeeded) {
      message.success('Saved!');
    }
  };

  return (
    <PageContainer
      title="SendGrid"
      subTitle="SendGrid là một dịch vụ gửi email"
    >
      <ProCard>
        <ProForm formRef={formRef} onFinish={onFinish}>
          <ProFormText.Password name="apiKey" label="API Key" />
          <Divider />
          <Typography.Title level={4}>From</Typography.Title>
          <ProFormText name="email" label="Email" />
          <ProFormText name="name" label="Name" />
        </ProForm>
      </ProCard>
    </PageContainer>
  );
};

export default SendGrid;
