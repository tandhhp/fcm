import { PageContainer, ProCard, ProForm, ProFormDatePicker, ProFormSelect } from '@ant-design/pro-components';
import { Col, Row } from 'antd';
import AmountReport from './components/amount';
import TopSales from './components/top-month';
import { useAccess } from '@umijs/max';
import LineBranch from './components/line-branch';
import TeleReport from './components/tele';
import { apiBranchOptions } from '@/services/settings/branch';
import { useState } from 'react';
import dayjs from 'dayjs';

const HomePage: React.FC = () => {

  const access = useAccess();
  const [branchId] = useState<number>(1);
  const [year, setYear] = useState<dayjs.Dayjs | null>(dayjs());

  return (
    <PageContainer extra={(
      <ProForm submitter={false}>
        <ProFormSelect
          name="branchId"
          disabled={!access.canAdmin}
          request={apiBranchOptions}
          initialValue={1}
          formItemProps={{
            className: 'mb-0'
          }}
          className="w-32" />
      </ProForm>
    )}>
      <AmountReport />
      <ProCard title="Doanh thu" className="mb-4" headerBordered extra={(
        <ProForm submitter={false}>
          <ProFormDatePicker.Year name="year" initialValue={dayjs()} className="w-32" fieldProps={{
            variant: 'filled',
            onChange: (date) => setYear(date),
            autoFocus: false,
            allowClear: false
          }} formItemProps={{
            className: 'mb-0'
          }} />
        </ProForm>
      )}>
        <Row>
          <LineBranch branchId={branchId} year={year} />
          <Col xs={24} md={6}>
            <TopSales />
          </Col>
        </Row>
      </ProCard>
      {
        access.telesaleManager && <TeleReport />
      }
    </PageContainer>
  );
};

export default HomePage;
