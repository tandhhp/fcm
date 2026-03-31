import { apiCallReportTele } from "@/services/call";
import { EyeOutlined, SettingOutlined, TableOutlined, AppstoreOutlined } from "@ant-design/icons";
import { PageContainer, ProCard, ProColumnType, ProForm, ProFormDateRangePicker } from "@ant-design/pro-components";
import { Table, Spin, Empty, Button, Segmented } from "antd";
import dayjs from "dayjs";
import { useEffect, useState } from "react";
import ReportDetail from "./components/detail";

type ReportResponse = {
    teleName: string;
    managerName: string;
    totalCalls: number;
    callStatusCounts: CallStatusCount[];
};

type CallStatusCount = {
    callStatus: string;
    count: number;
    callStatusId: number;
    teleId: string;
};

const TeleReportPage: React.FC = () => {
    const [data, setData] = useState<ReportResponse[]>([]);
    const [openDetail, setOpenDetail] = useState<boolean>(false);
    const [selectedRecord, setSelectedRecord] = useState<any>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [viewMode, setViewMode] = useState<'card' | 'table'>('table');
    const [dateRange, setDateRange] = useState<[string, string]>([
        dayjs().startOf("month").format("YYYY-MM-DD"),
        dayjs().endOf("month").format("YYYY-MM-DD"),
    ]);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            const response = await apiCallReportTele({
                fromDate: dateRange[0],
                toDate: dateRange[1],
            });
            setData(response.data || []);
            setLoading(false);
        };
        fetchData();
    }, [dateRange]);

    const columns = [
        {
            title: "Trạng thái cuộc gọi",
            dataIndex: "callStatus",
            key: "callStatus",
        },
        {
            title: "Số lượng",
            dataIndex: "count",
            key: "count",
        },
        {
            title: <SettingOutlined />,
            key: "action",
            render: (record: any) => (
                <Button type="primary" icon={<EyeOutlined />} size="small" onClick={() => {
                    setSelectedRecord(record);
                    console.log(record);
                    setOpenDetail(true);
                }}>Xem</Button>
            ),
            width: 30
        }
    ];

    // Columns for table view
    const tableViewColumns = [
        {
            title: "Tên Tele",
            dataIndex: "teleName",
            key: "teleName",
            fixed: 'left' as const,
            width: 150,
        },
        {
            title: "Quản lý",
            dataIndex: "managerName",
            key: "managerName",
            width: 150,
        },
        {
            title: "Tổng cuộc gọi",
            dataIndex: "totalCalls",
            key: "totalCalls",
            width: 120,
            align: 'center' as const,
        },
        ...(data.length > 0 && data[0].callStatusCounts
            ? Array.from(new Set(data.flatMap(d => d.callStatusCounts.map(c => c.callStatus)))).map(status => ({
                title: status,
                key: status,
                width: 100,
                align: 'center' as const,
                render: (record: ReportResponse) => {
                    const statusCount = record.callStatusCounts.find(c => c.callStatus === status);
                    if (statusCount?.count === 0 || !statusCount) {
                        return <Button type="dashed" size="small" disabled>0</Button>;
                    }
                    
                    return (
                        <Button 
                            type="dashed"
                            size="small" 
                            onClick={() => {
                                setSelectedRecord({
                                    teleId: statusCount?.teleId,
                                    callStatusId: statusCount?.callStatusId
                                });
                                setOpenDetail(true);
                            }}
                        >{statusCount?.count}</Button>
                    );
                }
            }))
            : []),
    ];

    return (
        <PageContainer>
            <ProCard title="Báo cáo Tele" headerBordered>
                
                
                <div style={{ marginTop: 16, marginBottom: 16 }} className="flex justify-between">
                    <ProForm layout="inline" submitter={false}>
                    <ProFormDateRangePicker
                        name="dateRange"
                        label="Khoảng thời gian"
                        initialValue={[dayjs(dateRange[0]), dayjs(dateRange[1])]
                        }
                        fieldProps={{
                            onChange: (dates, dateStrings) => {
                                setDateRange([dateStrings[0], dateStrings[1]]);
                            },
                            autoFocus: false
                        }}
                    />
                </ProForm>
                    <Segmented
                        value={viewMode}
                        onChange={(value) => setViewMode(value as 'card' | 'table')}
                        options={[
                            { label: 'Dạng thẻ', value: 'card', icon: <AppstoreOutlined /> },
                            { label: 'Dạng bảng', value: 'table', icon: <TableOutlined /> },
                        ]}
                    />
                </div>

                <div style={{ marginTop: 24 }}>
                    {loading ? (
                        <Spin />
                    ) : data.length > 0 ? (
                        viewMode === 'card' ? (
                            data.map((tele, idx) => (
                                <ProCard
                                    key={tele.teleName + idx}
                                    title={tele.teleName}
                                    style={{ marginBottom: 24 }}
                                    bordered
                                >
                                    <div style={{ marginBottom: 8 }}>
                                        <b>Quản lý:</b> {tele.managerName} &nbsp; | &nbsp;
                                        <b>Tổng số cuộc gọi:</b> {tele.totalCalls}
                                    </div>
                                    <Table
                                        columns={columns}
                                        dataSource={tele.callStatusCounts}
                                        rowKey="callStatus"
                                        pagination={false}
                                        size="small"
                                        bordered
                                    />
                                </ProCard>
                            ))
                        ) : (
                            <Table
                                columns={tableViewColumns}
                                dataSource={data}
                                rowKey="teleName"
                                pagination={{ pageSize: 20 }}
                                scroll={{ x: 'max-content' }}
                                bordered
                            />
                        )
                    ) : (
                        <Empty description="Không có dữ liệu báo cáo" />
                    )}
                </div>
            </ProCard>
            <ReportDetail
                open={openDetail}
                onOpenChange={setOpenDetail}
                data={selectedRecord}
            />
        </PageContainer>
    );
};

export default TeleReportPage;