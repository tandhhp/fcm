import WorkSummary from '@/components/works/summary';
import { getGoogleMap, saveArguments } from '@/services/work-content';
import {
  PageContainer,
  ProCard,
  ProForm,
  ProFormDigit,
  ProFormInstance,
  ProFormText,
} from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { Col, message, Row } from 'antd';
import { useEffect, useRef } from 'react';

const GoogleMap: React.FC = () => {
  const { id } = useParams();
  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    getGoogleMap(id).then((response) => {
      formRef.current?.setFields([
        {
          name: 'src',
          value: response.src,
        },
      ]);
    });
  }, [id]);

  const onFinish = async (values: any) => {
    const response = await saveArguments(id, values);
    if (response.succeeded) {
      message.success('Saved');
    }
  };

  return (
    <PageContainer>
      <Row gutter={16}>
        <Col span={16}>
          <ProCard>
            <ProForm formRef={formRef} onFinish={onFinish}>
              <ProFormText name="src" label="Iframe" />
              <ProFormDigit name="height" label="Height" />
              <ProFormText name="email" label="Email" />
              <ProFormText name="address" label="Address" />
            </ProForm>
          </ProCard>
        </Col>
        <Col span={8}>
          <WorkSummary />
        </Col>
      </Row>
    </PageContainer>
  );
};

export default GoogleMap;
