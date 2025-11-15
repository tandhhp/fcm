import { getSetting, saveSetting } from '@/services/setting';
import {
  PageContainer,
  ProCard,
  ProForm,
  ProFormInstance,
  ProFormText,
} from '@ant-design/pro-components';
import { FormattedMessage, history, useParams } from '@umijs/max';
import { Button, Col, Row, Space, message } from 'antd';
import { useEffect, useRef, useState } from 'react';
import { ArrowLeftOutlined, InfoCircleOutlined } from '@ant-design/icons';
import Blogger from './blogger';

const GoogleApp: React.FC = () => {
  const { id } = useParams();
  const formRef = useRef<ProFormInstance>();
  const [bloggers, setBloggers] = useState<any>([]);

  useEffect(() => {
    getSetting(id).then((response) => {
      if (!response) {
        return;
      }
      formRef.current?.setFields([
        {
          name: 'bloggerApiKey',
          value: response.bloggerApiKey,
        },
        {
          name: 'clientId',
          value: response.clientId,
        },
        {
          name: 'firebaseApiKey',
          value: response.firebase?.apiKey
        },
        {
          name: 'authDomain',
          value: response.firebase?.authDomain
        },
        {
          name: 'databaseURL',
          value: response.firebase?.databaseURL
        },
        {
          name: 'storageBucket',
          value: response.firebase?.storageBucket
        },
        {
          name: 'messagingSenderId',
          value: response.firebase?.messagingSenderId
        },
        {
          name: 'appId',
          value: response.firebase?.appId
        },
        {
          name: 'measurementId',
          value: response.firebase?.measurementId
        },
        {
          name: 'gTagId',
          value: response.gTagId
        }
      ]);
      if (response.bloggers) {
        setBloggers(response.bloggers);
      }
    });
  }, [id]);

  const onFinish = async (values: any) => {
    values.bloggers = bloggers;
    const response = await saveSetting(id, values);
    if (response.succeeded) {
      message.success('Saved');
    }
  };

  return (
    <PageContainer extra={<Button icon={<ArrowLeftOutlined />} onClick={() => history.back()}><span><FormattedMessage id='general.back' /></span></Button>}>
      <ProForm formRef={formRef} onFinish={onFinish}>
        <Row gutter={16}>
          <Col span={6}>
            <Blogger dataSource={bloggers} setDataSource={setBloggers} />
          </Col>
          <Col span={6}>
            <ProCard title="Firebase" extra={<a href='https://firebase.google.com/' target='_blank'><InfoCircleOutlined /></a>}>
              <Space>
                <ProFormText name="firebaseApiKey" label="Api key" />
                <ProFormText name="authDomain" label="Auth Domain" />
              </Space>
              <Space>
                <ProFormText name="databaseURL" label="DatabaseURL" />
                <ProFormText name="projectId" label="Project Id" />
              </Space>
              <Space>
                <ProFormText name="storageBucket" label="Storage Bucket" />
                <ProFormText name="messagingSenderId" label="Messaging Sender Id" />
              </Space>
              <Space>
                <ProFormText name="appId" label="App Id" />
                <ProFormText name="measurementId" label="Measurement Id" />
              </Space>
            </ProCard>
          </Col>
          <Col md={6}>
            <ProCard title="Analytics">
              <ProFormText name="gTagId" label="Google tag Id" />
            </ProCard>
          </Col>
        </Row>
      </ProForm>
    </PageContainer>
  );
};

export default GoogleApp;
