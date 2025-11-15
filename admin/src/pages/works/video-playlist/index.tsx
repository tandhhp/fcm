import WorkSummary from '@/components/works/summary';
import { getArguments, saveArguments } from '@/services/work-content';
import {
  PageContainer,
  ProCard,
  ProForm,
  ProFormDigit,
  ProFormInstance,
  ProFormText,
} from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { message, Row, Col } from 'antd';
import { useRef, useEffect } from 'react';

const VideoPlaylist: React.FC = () => {
  const { id } = useParams();
  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    getArguments(id).then((response) => {
      formRef.current?.setFields([
        {
          name: 'title',
          value: response.title,
        },
        {
          name: 'pageSize',
          value: response.pageSize,
        },
        {
          name: 'className',
          value: response.className
        }
      ]);
    });
  }, []);

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
              <ProFormText name="title" label="Title" />
              <ProFormText name="className" label="Class name" />
              <ProFormDigit name="pageSize" label="Page size" />
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

export default VideoPlaylist;
