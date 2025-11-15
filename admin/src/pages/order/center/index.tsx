import { queryOrder } from '@/services/order';
import { formatDate } from '@/utils/format';
import { PrinterOutlined } from '@ant-design/icons';
import { PageContainer, ProCard } from '@ant-design/pro-components';
import { Link, useParams } from '@umijs/max';
import { Button, Col, Descriptions, Row, Table } from 'antd';
import { useEffect, useState } from 'react';

const OrderCenter: React.FC = () => {
  const { id } = useParams();
  const [order, setOrder] = useState<API.Order>();
  useEffect(() => {
    queryOrder(id).then(response => {
      setOrder(response);
    })
  }, []);

  return (
    <PageContainer title={order?.number} extra={<Button icon={<PrinterOutlined />}>Print</Button>}>
      <Row gutter={16} className='mb-4'>
        <Col md={12}>
          <ProCard>
            <Descriptions title="Customer Info">
              <Descriptions.Item label="Name">{order?.customerName}</Descriptions.Item>
              <Descriptions.Item label="Note">{order?.note}</Descriptions.Item>
            </Descriptions>
          </ProCard>
        </Col>
        <Col md={12}>
          <ProCard>
          <Descriptions title="Order Info">
              <Descriptions.Item label="Date">{formatDate(order?.createdDate)}</Descriptions.Item>
              <Descriptions.Item label="Note">{order?.note}</Descriptions.Item>
            </Descriptions>
          </ProCard>
        </Col>
      </Row>
      <Table
        dataSource={order?.orderDetails}
        rowKey="id"
        columns={[
          {
            title: 'Product name',
            dataIndex: 'productName',
            render: (dom, record) => <Link to={`/catalog/${record.productId}`}>{dom}</Link>
          },
          {
            title: 'Price',
            dataIndex: 'price'
          },
          {
            title: 'Quantity',
            dataIndex: 'quantity'
          },
          {
            title: 'Thành tiền',
            render: (dom, record) => record.price * record.quantity
          }
        ]}
      />
    </PageContainer>
  );
};

export default OrderCenter;
