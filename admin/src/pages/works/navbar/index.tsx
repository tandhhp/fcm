import WorkSummary from '@/components/works/summary';
import { PageContainer, ProCard } from '@ant-design/pro-components';
import { Col, Row } from 'antd';
import { useState } from 'react';
import NavbarContent from './content';
import NavbarSetting from './setting';

const Navbar: React.FC = () => {
  const [tab, setTab] = useState('content');

  return (
    <PageContainer title="Navbar">
      <Row gutter={16}>
        <Col span={18}>
          <ProCard
            tabs={{
              activeKey: tab,
              items: [
                {
                  label: 'Content',
                  key: 'content',
                  children: <NavbarContent />,
                },
                {
                  label: 'Setting',
                  key: 'setting',
                  children: <NavbarSetting />,
                },
              ],
              onChange: (key) => {
                setTab(key);
              },
            }}
          ></ProCard>
        </Col>
        <Col span={6}>
          <WorkSummary />
        </Col>
      </Row>
    </PageContainer>
  );
};

export default Navbar;
