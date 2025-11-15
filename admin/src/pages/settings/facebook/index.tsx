import { facebookGet, facebookSave } from '@/services/setting';
import {
  PageContainer,
  ProCard,
  ProForm,
  ProFormInstance,
  ProFormText,
} from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { Col, message, Row } from 'antd';
import { useEffect, useRef } from 'react';
import GraphApiExplorer from './graph-api-explorer';

const FacebookApp: React.FC = () => {
  const { id } = useParams();
  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    facebookGet(id).then((response) => {
      if (response) {
        formRef.current?.setFields([
          {
            name: 'appId',
            value: response.appId,
          },
          {
            name: 'appSecret',
            value: response.appSecret,
          },
          {
            name: 'pageAccessToken',
            value: response.pageAccessToken,
          },
          {
            name: 'pageId',
            value: response.pageId,
          },
          {
            name: 'shortLiveToken',
            value: response.shortLiveToken,
          },
        ]);
      }
    });
  }, []);

  const onFinish = async (values: API.Facebook) => {
    const response = await facebookSave(values);
    if (response.succeeded) {
      message.success('Saved');
    }
  };

  return (
    <PageContainer>
      <Row gutter={16}>
        <Col span={16}>
          <ProCard>
            <ProForm onFinish={onFinish} formRef={formRef} grid>
              <ProFormText name="id" initialValue={id} hidden />
              <ProFormText name="appId" label="App Id" colProps={{
                md: 12
              }} />
              <ProFormText.Password name="appSecret" label="App secret" colProps={{
                md: 12
              }} />
              <ProFormText name="shortLiveToken" label="Short live token" />
              <ProFormText name="pageAccessToken" label="Page access token" />
              <ProFormText name="pageId" label="Page ID" colProps={{
                md: 8
              }} />
              <ProFormText name="pageUrl" label="Page Url" placeholder="https://" colProps={{
                md: 8
              }} />
              <ProFormText name="profileUrl" label="Profile Url" placeholder="https://" colProps={{
                md: 8
              }} />
            </ProForm>
          </ProCard>
        </Col>
        <Col span={8}>
          <GraphApiExplorer />
        </Col>
      </Row>
    </PageContainer>
  );
};

export default FacebookApp;
