import WorkSummary from '@/components/works/summary';
import { getArguments, saveArguments } from '@/services/work-content';
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

const Swiper: React.FC = () => {
  const { id } = useParams();
  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    getArguments(id).then((response) => {
      formRef.current?.setFields([
        {
          name: 'mode',
          value: response.mode,
        },
      ]);
    });
  }, []);

  const onFinish = async (values: any) => {
    values.format = 1;
    const response = await saveArguments(id, values);
    if (response.succeeded) {
      message.success('Saved');
    }
  };

  return (
    <PageContainer>
      <Row gutter={16}>
        <Col md={16}>
          <ProCard>
            <ProForm formRef={formRef} onFinish={onFinish}>
              <ProFormText name="title" label="Title" />
              <ProFormText name="mode" label="Display mode" />
            </ProForm>
          </ProCard>
        </Col>
        <Col md={8}>
          <WorkSummary />
        </Col>
      </Row>
    </PageContainer>
  );
};

export default Swiper;
