import WorkSummary from '@/components/works/summary';
import { PageContainer } from '@ant-design/pro-components';
import { Row, Col } from 'antd';

const Trend: React.FC = () => {
  return (
    <PageContainer>
      <Row gutter={16}>
        <Col md={16}></Col>
        <Col md={8}>
          <WorkSummary />
        </Col>
      </Row>
    </PageContainer>
  );
};

export default Trend;
