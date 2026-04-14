import { apiEventSuReport } from '@/services/event';
import { apiAttendanceOptions } from '@/services/event/attendance';
import { Column, Pie } from '@ant-design/charts';
import { ProCard, ProForm, ProFormDatePicker } from '@ant-design/pro-components';
import { useRequest } from '@umijs/max';
import { Col, Empty, Row, Skeleton, Statistic } from 'antd';
import dayjs, { Dayjs } from 'dayjs';
import { useEffect, useMemo, useState } from 'react';

type SUAttendance = {
  attendanceId: number;
  count: number;
  name: string;
};

type SUSalesReport = {
  salesName: string;
  attendances: SUAttendance[];
};

type SUReportResult = {
  salesManagerName: string;
  salesReports: SUSalesReport[];
};

const EventDashboard: React.FC = () => {
  const [fromDate, setFromDate] = useState<Dayjs>(dayjs().startOf('month'));
  const [toDate, setToDate] = useState<Dayjs>(dayjs().endOf('month'));

  const [loading, setLoading] = useState<boolean>(true);
  const [reportData, setReportData] = useState<SUReportResult[]>([]);
  const [attendanceOptions, setAttendanceOptions] = useState<{ label: string; value: string }[]>([]);

  useEffect(() => {
    const fetchData = async () => {
        try {
            setLoading(true);
            const response = await apiAttendanceOptions();
            const reportResponse = await apiEventSuReport({
                fromDate: fromDate.format('YYYY-MM-DD'),
                toDate: toDate.format('YYYY-MM-DD')
            });
            setAttendanceOptions(response || []);
            setReportData(reportResponse || []);
            setLoading(false);
        } catch (error) {
            console.error('Error fetching data:', error);
            setLoading(false);
        }
    };

    fetchData();
  }, [fromDate, toDate]);

  const metrics = useMemo(() => {
    const flattenedSales = reportData.flatMap((item: any) => item.salesReports || []);

    const salesManagers = reportData.length;
    const totalSales = flattenedSales.length;

    const attendanceMap = new Map<string, number>();
    const saleLoadData: { name: string; value: number }[] = [];
    let totalCount = 0;

    flattenedSales.forEach((sale: any) => {
      const attendances = sale.attendances || [];
      const saleTotal = attendances.reduce((sum: number, attendance: any) => {
        const isTotalRow = attendance.attendanceId === 0;
        if (isTotalRow) return sum;

        const next = (attendanceMap.get(attendance.name) || 0) + (attendance.count || 0);
        attendanceMap.set(attendance.name, next);
        return sum + (attendance.count || 0);
      }, 0);

      totalCount += saleTotal;
      saleLoadData.push({
        name: sale.salesName,
        value: saleTotal
      });
    });

    const attendanceData = Array.from(attendanceMap.entries()).map(([name, value]) => ({
      type: name,
      value
    }));

    const topSaleLoadData = saleLoadData
      .sort((a, b) => b.value - a.value)
      .slice(0, 10);

    return {
      salesManagers,
      totalSales,
      totalCount,
      averagePerSales: totalSales ? Number((totalCount / totalSales).toFixed(2)) : 0,
      attendanceData,
      topSaleLoadData
    };
  }, [reportData]);

  if (loading) {
    return (
      <ProCard className="mb-4" title="Dashboard Event" headerBordered>
        <Skeleton active />
      </ProCard>
    );
  }

  if (!reportData.length) {
    return (
      <ProCard className="mb-4" title="Dashboard Event" headerBordered>
        <Empty description="Chưa có dữ liệu" />
      </ProCard>
    );
  }

  return (
    <ProCard
      className="mb-4"
      title="Dashboard Event / Event Manager"
      headerBordered
      extra={(
        <ProForm layout="inline" submitter={false}>
          <ProFormDatePicker
            name="fromDate"
            label="Từ ngày"
            initialValue={fromDate}
            fieldProps={{
              autoFocus: false,
              onChange: (date: Dayjs | null) => {
                if (date) setFromDate(date);
              }
            }}
          />
          <ProFormDatePicker
            name="toDate"
            label="Đến ngày"
            initialValue={toDate}
            fieldProps={{
              autoFocus: false,
              onChange: (date: Dayjs | null) => {
                if (date) setToDate(date);
              }
            }}
          />
        </ProForm>
      )}
    >
      <Row gutter={[16, 16]} className="mb-4">
        <Col xs={24} sm={12} md={8} xl={6}>
          <ProCard>
            <Statistic title="Số Event Manager (SM)" value={metrics.salesManagers} />
          </ProCard>
        </Col>
        <Col xs={24} sm={12} md={8} xl={6}>
          <ProCard>
            <Statistic title="Số nhân viên Event" value={metrics.totalSales} />
          </ProCard>
        </Col>
        <Col xs={24} sm={12} md={8} xl={6}>
          <ProCard>
            <Statistic title="Tổng chỉ số attendance" value={metrics.totalCount} />
          </ProCard>
        </Col>
        <Col xs={24} sm={12} md={8} xl={6}>
          <ProCard>
            <Statistic title="Trung bình mỗi Event" value={metrics.averagePerSales} />
          </ProCard>
        </Col>
      </Row>

      <Row gutter={[16, 16]}>
        <Col xs={24} lg={14}>
          <ProCard title="Top Event theo sản lượng" headerBordered>
            {metrics.topSaleLoadData.length ? (
              <Column
                height={320}
                data={metrics.topSaleLoadData}
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
              <Empty description="Không có dữ liệu" />
            )}
          </ProCard>
        </Col>
        <Col xs={24} lg={10}>
          <ProCard title="Cơ cấu attendance" headerBordered>
            {metrics.attendanceData.length ? (
              <Pie
                height={320}
                data={metrics.attendanceData}
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
            ) : (
              <Empty description="Không có dữ liệu attendance" />
            )}
          </ProCard>
        </Col>
      </Row>
    </ProCard>
  );
};

export default EventDashboard;