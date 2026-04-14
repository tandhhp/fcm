import { apiContactStatistics } from '@/services/contact';
import { apiCallStatistics, apiCallTMRReport } from '@/services/call';
import { apiSourceList } from '@/services/settings/source';
import { apiKeyinByTelesale } from '@/services/user';
import { apiTelesalesManagerOptions, apiTelesalesOptions } from '@/services/role';
import { ProCard } from '@ant-design/pro-components';
import { useModel, useRequest } from '@umijs/max';
import { Column, Pie } from '@ant-design/charts';
import { Col, Empty, Row, Skeleton, Statistic } from 'antd';

type TResultLike<T> = {
  succeeded?: boolean;
  data?: T;
};

type KeyinRow = {
  userName?: string;
  name?: string;
  leadCount?: number;
};

const toData = <T,>(payload: TResultLike<T> | T | undefined): T | undefined => {
  if (!payload) return undefined;
  if (typeof payload === 'object' && payload !== null && 'data' in payload) {
    return (payload as TResultLike<T>).data;
  }
  return payload as T;
};

const DotDashboard: React.FC = () => {
  const { initialState } = useModel('@@initialState');
  const currentUserId = initialState?.currentUser?.id;

  const { data, loading } = useRequest(async () => {
    const [contactResponse, sourceResponse, callResponse, tmrResponse, keyinResponse, managerOptions, telesaleOptions] = await Promise.all([
      apiContactStatistics(),
      apiSourceList({ current: 1, pageSize: 1 }),
      apiCallStatistics(),
      apiCallTMRReport(),
      apiKeyinByTelesale({ current: 1, pageSize: 200 }),
      apiTelesalesManagerOptions({ dotId: currentUserId }),
      apiTelesalesOptions({ dotId: currentUserId })
    ]);

    const contactStats = toData<any>(contactResponse) || {};
    const callStats = toData<any>(callResponse) || {};
    const tmrStats = toData<any>(tmrResponse) || {};
    const keyinRows: KeyinRow[] = keyinResponse?.data || [];

    return {
      totalContacts: contactStats.totalContacts || 0,
      totalCurrentMonthContacts: contactStats.totalCurrentMonth || 0,
      totalCurrentYearContacts: contactStats.totalCurrentYear || 0,
      totalSources: sourceResponse?.total || 0,
      totalCalls: callStats.totalCalls ?? callStats.TotalCalls ?? 0,
      currentMonthCalls: callStats.currentMonthCalls ?? callStats.CurrentMonthCalls ?? 0,
      yearlyCalls: callStats.yearlyCalls ?? callStats.YearlyCalls ?? 0,
      totalContactCalled: tmrStats.totalCalled || 0,
      totalContactNotCalled: tmrStats.TotalNotContacted || 0,
      totalManagers: managerOptions?.length || 0,
      totalTelesales: telesaleOptions?.length || keyinResponse?.total || 0,
      keyinRows
    };
  });

  if (loading) {
    return (
      <ProCard className="mb-4" title="Dashboard DOT" headerBordered>
        <Skeleton active />
      </ProCard>
    );
  }

  if (!data) {
    return (
      <ProCard className="mb-4" title="Dashboard DOT" headerBordered>
        <Empty description="Chưa có dữ liệu" />
      </ProCard>
    );
  }

  const keyinChartData = [...(data.keyinRows || [])]
    .sort((a, b) => (b.leadCount || 0) - (a.leadCount || 0))
    .slice(0, 10)
    .map((item) => ({
      name: item.name || item.userName || 'N/A',
      value: item.leadCount || 0
    }));

  const callCoverageData = [
    {
      type: 'Đã gọi',
      value: data.totalContactCalled
    },
    {
      type: 'Chưa gọi',
      value: data.totalContactNotCalled
    }
  ];

  return (
    <ProCard title="Dashboard giám đốc telesales" className="mb-4" headerBordered>
      <Row gutter={[16, 16]} className="mb-4">
        <Col xs={24} sm={12} md={8} xl={4}>
          <ProCard>
            <Statistic title="Tổng contact" value={data.totalContacts} />
          </ProCard>
        </Col>
        <Col xs={24} sm={12} md={8} xl={4}>
          <ProCard>
            <Statistic title="Tổng nguồn" value={data.totalSources} />
          </ProCard>
        </Col>
        <Col xs={24} sm={12} md={8} xl={4}>
          <ProCard>
            <Statistic title="Nhân viên telesales" value={data.totalTelesales} />
          </ProCard>
        </Col>
        <Col xs={24} sm={12} md={8} xl={4}>
          <ProCard>
            <Statistic title="Quản lý telesales" value={data.totalManagers} />
          </ProCard>
        </Col>
        <Col xs={24} sm={12} md={8} xl={4}>
          <ProCard>
            <Statistic title="Cuộc gọi tháng này" value={data.currentMonthCalls} />
          </ProCard>
        </Col>
        <Col xs={24} sm={12} md={8} xl={4}>
          <ProCard>
            <Statistic title="Contact tháng này" value={data.totalCurrentMonthContacts} />
          </ProCard>
        </Col>
      </Row>

      <Row gutter={[16, 16]}>
        <Col xs={24} lg={14}>
          <ProCard title="Top key-in theo telesales" headerBordered>
            {keyinChartData.length ? (
              <Column
                height={320}
                data={keyinChartData}
                xField="name"
                yField="value"
                label={{
                  text: 'value',
                  position: 'top'
                }}
                axis={{
                  x: {
                    labelAutoRotate: true
                  }
                }}
              />
            ) : (
              <Empty description="Chưa có dữ liệu key-in" />
            )}
          </ProCard>
        </Col>
        <Col xs={24} lg={10}>
          <ProCard title="Độ phủ gọi contact" headerBordered>
            <Pie
              height={320}
              data={callCoverageData}
              angleField="value"
              colorField="type"
              label={{
                text: (d: any) => `${d.type}: ${d.value}`
              }}
              legend={{
                color: {
                  position: 'bottom'
                }
              }}
            />
            <div className="mt-2 flex justify-between text-sm text-gray-500">
              <span>Contact năm nay: {data.totalCurrentYearContacts}</span>
              <span>Tổng cuộc gọi năm: {data.yearlyCalls}</span>
            </div>
          </ProCard>
        </Col>
      </Row>
    </ProCard>
  );
};

export default DotDashboard;