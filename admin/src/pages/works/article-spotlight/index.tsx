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

const ArticleSpotlight: React.FC = () => {
  const formRef = useRef<ProFormInstance>();
  const { id } = useParams();

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
          name: 'tagId',
          value: response.tagId,
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
        <Col md={16}>
          <ProCard>
            <ProForm onFinish={onFinish} formRef={formRef}>
              <ProFormText label="Title" name="title" />
              <ProFormDigit label="Page size" name="pageSize" />
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

export default ArticleSpotlight;
