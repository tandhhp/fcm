import WorkSummary from '@/components/works/summary';
import { getArguments, saveArguments } from '@/services/work-content';
import {
  PageContainer,
  ProCard,
  ProForm,
  ProFormInstance,
  ProFormSelect,
  ProFormText,
} from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { Col, message, Row } from 'antd';
import { useEffect, useRef } from 'react';

const AffiliateLink: React.FC = () => {
  const { id } = useParams();
  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    getArguments(id).then((response) => {
      formRef.current?.setFields([
        {
          name: 'name',
          value: response.name,
        },
        {
          name: 'href',
          value: response.href,
        },
        {
          name: 'target',
          value: response.target,
        },
      ]);
    });
  }, [id]);

  const onFinish = async (values: CPN.Link) => {
    const response = await saveArguments(id, values);
    if (response.succeeded) {
      message.success('Saved!');
    } else {
      message.error(response.errors[0].description);
    }
  };

  return (
    <PageContainer>
      <Row gutter={16}>
        <Col span={16}>
          <ProCard>
            <ProForm onFinish={onFinish} formRef={formRef}>
              <ProFormText name="name" label="Name" />
              <ProFormText
                name="href"
                label="URL"
                rules={[
                  {
                    required: true,
                  },
                ]}
              />
              <ProFormSelect
                name="target"
                label="Target"
                allowClear
                options={[
                  {
                    value: '_blank',
                    label: 'Open in new tab',
                  },
                ]}
              />
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

export default AffiliateLink;
