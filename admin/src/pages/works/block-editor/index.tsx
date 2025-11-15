import ProEditorBlock from '@/components/editorjs';
import WorkSummary from '@/components/works/summary';
import { saveBlockEditor } from '@/services/work-content';
import { ArrowLeftOutlined, SaveOutlined } from '@ant-design/icons';
import { PageContainer, ProCard } from '@ant-design/pro-components';
import { FormattedMessage, history } from '@umijs/max';
import { Button, Col, message, Row, Space } from 'antd';
import { useState } from 'react';

const BlockEditor: React.FC = () => {
  const [editorData, setEditorData] = useState<any>();

  const onFinish = async () => {
    const response = await saveBlockEditor(editorData);
    if (response.succeeded) {
      message.success('Saved');
    }
  };

  return (
    <PageContainer
      extra={
        <Button icon={<ArrowLeftOutlined />} onClick={() => history.back()}>
          Back
        </Button>
      }
    >
      <Row gutter={16}>
        <Col span={18}>
          <ProCard
            title="Start writing"
            extra={
              <Button type="primary" onClick={onFinish}>
                <Space>
                  <SaveOutlined />
                  <FormattedMessage id="general.save" />
                </Space>
              </Button>
            }
          >
            <ProEditorBlock onChange={setEditorData} />
          </ProCard>
        </Col>
        <Col span={6}>
          <WorkSummary />
        </Col>
      </Row>
    </PageContainer>
  );
};

export default BlockEditor;
