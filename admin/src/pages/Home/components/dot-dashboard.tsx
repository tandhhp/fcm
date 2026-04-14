import { apiContactStatistics } from '@/services/contact';
import { apiCallTMRReport } from '@/services/call';
import { apiKeyinByTelesale } from '@/services/user';
import { ProCard } from '@ant-design/pro-components';
import { useRequest } from '@umijs/max';
import { Column, Pie } from '@ant-design/charts';
import { Col, Empty, Row, Statistic } from 'antd';


const DotDashboard: React.FC = () => {
  const { data: contactStats } = useRequest(apiContactStatistics);
  const { data: keyinResponse } = useRequest(() => apiKeyinByTelesale({ current: 1, pageSize: 200 }));
  const { data: tmrStats } = useRequest(apiCallTMRReport);

  const keyinChartData = [...(keyinResponse?.keyinRows || [])]
    .sort((a, b) => (b.leadCount || 0) - (a.leadCount || 0))
    .slice(0, 10)
    .map((item) => ({
      name: item.name || item.userName || 'N/A',
      value: item.leadCount || 0
    }));

  const callCoverageData = [
    {
      type: 'Đã gọi',
      value: tmrStats?.totalContactCalled
    },
    {
      type: 'Chưa gọi',
      value: tmrStats?.totalContactNotCalled
    }
  ];

  return (
    <ProCard title="Dashboard giám đốc telesales" className="mb-4" headerBordered>
      <Row gutter={[16, 16]} className="mb-4">
        <Col xs={24} sm={12} md={8} xl={4}>
          <ProCard>
            <Statistic title="Tổng contact" value={contactStats?.totalContacts} />
          </ProCard>
        </Col>
        <Col xs={24} sm={12} md={8} xl={4}>
          <ProCard>
            <Statistic title="Tổng nguồn" value={contactStats?.totalSources} />
          </ProCard>
        </Col>
        <Col xs={24} sm={12} md={8} xl={4}>
          <ProCard>
            <Statistic title="Nhân viên telesales" value={contactStats?.totalTelesales} />
          </ProCard>
        </Col>
        <Col xs={24} sm={12} md={8} xl={4}>
          <ProCard>
            <Statistic title="Quản lý telesales" value={contactStats?.totalManagers} />
          </ProCard>
        </Col>
        <Col xs={24} sm={12} md={8} xl={4}>
          <ProCard>
            <Statistic title="Cuộc gọi tháng này" value={contactStats?.currentMonthCalls} />
          </ProCard>
        </Col>
        <Col xs={24} sm={12} md={8} xl={4}>
          <ProCard>
            <Statistic title="Contact tháng này" value={contactStats?.totalCurrentMonthContacts} />
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
            {
              callCoverageData && (
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
              )
            }
            <div className="mt-2 flex justify-between text-sm text-gray-500">
              <span>Contact năm nay: {tmrStats?.totalCurrentYearContacts}</span>
              <span>Tổng cuộc gọi năm: {tmrStats?.yearlyCalls}</span>
            </div>
          </ProCard>
        </Col>
      </Row>
    </ProCard>
  );
};

export default DotDashboard;