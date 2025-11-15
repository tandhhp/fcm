import WorkSummary from '@/components/works/summary';
import { ArrowLeftOutlined } from '@ant-design/icons';
import { PageContainer, ProCard } from '@ant-design/pro-components';
import { FormattedMessage, history } from '@umijs/max';
import { Button, Col, Row } from 'antd';
import { useState } from 'react';
import RowContent from './components/content';
import RowSetting from './components/setting';

const RowComponent: React.FC = () => {
  const [tab, setTab] = useState('content');

  return (
    <PageContainer
      extra={
        <Button icon={<ArrowLeftOutlined />} onClick={() => history.back()}>
          <span>
            <FormattedMessage id='general.back' />
          </span>
        </Button>
      }
    >
      <Row gutter={16}>
        <Col span={18}>
          <ProCard
            tabs={{
              activeKey: tab,
              items: [
                {
                  label: 'Content',
                  key: 'content',
                  children: <RowContent />,
                },
                {
                  label: <FormattedMessage id='menu.settings' />,
                  key: 'setting',
                  children: <RowSetting />,
                },
              ],
              onChange: (key) => {
                setTab(key);
              },
            }}
          />
        </Col>
        <Col span={6}>
          <WorkSummary />
        </Col>
      </Row>
    </PageContainer>
  );
};

export default RowComponent;
