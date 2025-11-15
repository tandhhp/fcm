import { PageContainer } from '@ant-design/pro-components';
import { Button } from 'antd';

const OrderDetail: React.FC = () => {
  return (
    <PageContainer
      extra={<Button type="primary">Invoice</Button>}
    ></PageContainer>
  );
};

export default OrderDetail;
