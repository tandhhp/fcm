import { apiCallReportTele } from "@/services/call";
import { EyeOutlined, SettingOutlined } from "@ant-design/icons";
import { PageContainer, ProCard, ProForm, ProFormDateRangePicker } from "@ant-design/pro-components";
import { Table, Spin, Empty, Button } from "antd";
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
};

const TeleReportPage: React.FC = () => {
    const [data, setData] = useState<ReportResponse[]>([]);
    const [openDetail, setOpenDetail] = useState<boolean>(false);
    const [selectedRecord, setSelectedRecord] = useState<any>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [dateRange, setDateRange] = useState<[string, string]>([
        dayjs().startOf("month").format("YYYY-MM-DD"),
        dayjs().endOf("month").format("YYYY-MM-DD"),
    ]);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            const response = await apiCallReportTele({
                startDate: dateRange[0],
                endDate: dateRange[1],
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
                    setOpenDetail(true);
                }}>Xem</Button>
            ),
            width: 30
        }
    ];

    return (
        <PageContainer>
            <ProCard title="Báo cáo Tele" headerBordered>
                <ProForm layout="inline" submitter={false}>
                    <ProFormDateRangePicker
                        name="dateRange"
                        label="Chọn khoảng thời gian"
                        initialValue={[dayjs(dateRange[0]), dayjs(dateRange[1])]
                        }
                        fieldProps={{
                            onChange: (dates, dateStrings) => {
                                setDateRange([dateStrings[0], dateStrings[1]]);
                            },
                        }}
                    />
                </ProForm>
                <div style={{ marginTop: 24 }}>
                    {loading ? (
                        <Spin />
                    ) : data.length > 0 ? (
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