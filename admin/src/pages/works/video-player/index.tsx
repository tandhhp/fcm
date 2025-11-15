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
import { Col, Row, message } from 'antd';
import { useEffect, useRef } from 'react';

const VideoPlayer: React.FC = () => {
  const { id } = useParams();
  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    getArguments(id).then((response) => {
      formRef.current?.setFields([
        {
          name: 'embedId',
          value: response.embedId,
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
        <Col span={16}>
          <ProCard>
            <ProForm formRef={formRef} onFinish={onFinish}>
              <ProFormText name="embedId" label="Embed Id" />
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

export default VideoPlayer;
