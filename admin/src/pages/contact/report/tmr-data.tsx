import { apiReportTmrData, apiReportTmrDataExport } from "@/services/contact";
import { apiTeamOptions } from "@/services/users/team";
import { ExportOutlined } from "@ant-design/icons";
import { PageContainer, ProCard, ProFormSelect } from "@ant-design/pro-components";
import { Button, Col, Form, Radio, Row, Space, Spin, Table, message } from "antd";
import dayjs from "dayjs";
import { useState } from "react";

const Index: React.FC = () => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState<any>(null);
    const [exporting, setExporting] = useState(false);
    const [viewType, setViewType] = useState<0 | 1>(0);

    const handleSearch = async () => {
        try {
            setLoading(true);
            const values = form.getFieldsValue();
            const response = await apiReportTmrData({
                TeamId: values.teamId,
                viewType: values.viewType,
            });
            setData(response.data);
            setViewType(values.viewType);
        } catch (error) {
            message.error('Lỗi khi tải dữ liệu');
        } finally {
            setLoading(false);
        }
    };

    const handleExport = async () => {
        try {
            setExporting(true);
            const values = form.getFieldsValue();
            const response = await apiReportTmrDataExport({
                TeamId: values.teamId,
                viewType: values.viewType,
            });
            
            const blob = new Blob([response], { 
                type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' 
            });
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `report_tmr_data_${dayjs().format('YYYYMMDD_HHmmss')}.xlsx`;
            a.click();
            window.URL.revokeObjectURL(url);
            message.success('Xuất file thành công');
        } catch (error) {
            message.error('Lỗi khi xuất file');
        } finally {
            setExporting(false);
        }
    };

    // Cột cho User table (bên trong expand)
    const userColumns = viewType === 0 ? [
        {
            title: 'Username',
            dataIndex: 'userName',
            key: 'userName',
            width: 120,
        },
        {
            title: 'Họ và tên',
            dataIndex: 'fullName',
            key: 'fullName',
            width: 200,
        },
        {
            title: 'Tổng phân công',
            dataIndex: 'totalAssign',
            key: 'totalAssign',
            align: 'right' as const,
            width: 130,
        },
        {
            title: 'Tổng khả dụng',
            dataIndex: 'totalAvailableContact',
            key: 'totalAvailableContact',
            align: 'right' as const,
            width: 130,
        },
    ] : [
        {
            title: 'Username',
            dataIndex: 'userName',
            key: 'userName',
            width: 120,
        },
        {
            title: 'Họ và tên',
            dataIndex: 'fullName',
            key: 'fullName',
            width: 200,
        },
        {
            title: 'Call Status Counts',
            children: [
                {
                    title: 'Tele',
                    dataIndex: ['callStatusCounts', 'tele'],
                    key: 'tele',
                    align: 'right' as const,
                    width: 80,
                    render: (val: any) => val || 0,
                },
                {
                    title: 'Temp',
                    dataIndex: ['callStatusCounts', 'temp'],
                    key: 'temp',
                    align: 'right' as const,
                    width: 80,
                    render: (val: any) => val || 0,
                },
                {
                    title: 'Not Enough',
                    dataIndex: ['callStatusCounts', 'notEnough'],
                    key: 'notEnough',
                    align: 'right' as const,
                    width: 100,
                    render: (val: any) => val || 0,
                },
                {
                    title: 'Meet',
                    dataIndex: ['callStatusCounts', 'meet'],
                    key: 'meet',
                    align: 'right' as const,
                    width: 80,
                    render: (val: any) => val || 0,
                },
                {
                    title: 'Refuse',
                    dataIndex: ['callStatusCounts', 'refuse'],
                    key: 'refuse',
                    align: 'right' as const,
                    width: 80,
                    render: (val: any) => val || 0,
                },
                {
                    title: 'Location',
                    dataIndex: ['callStatusCounts', 'location'],
                    key: 'location',
                    align: 'right' as const,
                    width: 80,
                    render: (val: any) => val || 0,
                },
            ],
        },
    ];

    // Cột cho Team table
    const teamColumns = viewType === 0 ? [
        {
            title: 'Leader',
            dataIndex: 'leaderName',
            key: 'leaderName',
            width: 200,
        },
        {
            title: 'Tổng phân công',
            dataIndex: 'totalAssign',
            key: 'totalAssign',
            align: 'right' as const,
            width: 130,
        },
        {
            title: 'Tổng khả dụng',
            dataIndex: 'totalAvailableContact',
            key: 'totalAvailableContact',
            align: 'right' as const,
            width: 130,
        },
    ] : [
        {
            title: 'Leader',
            dataIndex: 'leaderName',
            key: 'leaderName',
            width: 200,
        },
        {
            title: 'Call Status Counts',
            children: [
                {
                    title: 'Tele',
                    dataIndex: ['callStatusCounts', 'tele'],
                    key: 'tele',
                    align: 'right' as const,
                    width: 80,
                },
                {
                    title: 'Temp',
                    dataIndex: ['callStatusCounts', 'temp'],
                    key: 'temp',
                    align: 'right' as const,
                    width: 80,
                },
                {
                    title: 'Not Enough',
                    dataIndex: ['callStatusCounts', 'notEnough'],
                    key: 'notEnough',
                    align: 'right' as const,
                    width: 100,
                },
                {
                    title: 'Meet',
                    dataIndex: ['callStatusCounts', 'meet'],
                    key: 'meet',
                    align: 'right' as const,
                    width: 80,
                },
                {
                    title: 'Refuse',
                    dataIndex: ['callStatusCounts', 'refuse'],
                    key: 'refuse',
                    align: 'right' as const,
                    width: 80,
                },
                {
                    title: 'Location',
                    dataIndex: ['callStatusCounts', 'location'],
                    key: 'location',
                    align: 'right' as const,
                    width: 80,
                },
            ],
        },
    ];

    return (
        <PageContainer>
            <ProCard style={{ marginBottom: 16 }}>
                <Form form={form} layout="vertical" initialValues={{ viewType: 0 }}>
                    <Row gutter={16}>
                        <Col span={8}>
                            <ProFormSelect
                                name="teamId"
                                label="Đội nhóm"
                                placeholder="Chọn đội nhóm"
                                request={apiTeamOptions}
                                fieldProps={{
                                    allowClear: true,
                                }}
                            />
                        </Col>
                        <Col span={8}>
                            <Form.Item name="viewType" label="Loại xem">
                                <Radio.Group>
                                    <Radio value={0}>Assigned</Radio>
                                    <Radio value={1}>Call Status</Radio>
                                </Radio.Group>
                            </Form.Item>
                        </Col>
                        <Col span={8} style={{ display: 'flex', alignItems: 'flex-end' }}>
                            <Space>
                                <Button type="primary" onClick={handleSearch} loading={loading}>
                                    Tìm kiếm
                                </Button>
                                <Button 
                                    icon={<ExportOutlined />} 
                                    onClick={handleExport}
                                    loading={exporting}
                                    disabled={!data}
                                >
                                    Xuất Excel
                                </Button>
                            </Space>
                        </Col>
                    </Row>
                </Form>
            </ProCard>

            <Spin spinning={loading}>
                {data && (
                    <ProCard title={`Báo cáo TMR Data - ${viewType === 0 ? 'Assigned' : 'Call Status'}`}>
                        <Table
                            columns={teamColumns}
                            dataSource={data.teams}
                            rowKey="teamId"
                            pagination={false}
                            expandable={{
                                expandedRowRender: (record: any) => (
                                    <Table
                                        columns={userColumns}
                                        dataSource={record.users}
                                        rowKey="userId"
                                        pagination={false}
                                        size="small"
                                        scroll={{ x: true }}
                                    />
                                ),
                                rowExpandable: (record: any) => record.users && record.users.length > 0,
                            }}
                            scroll={{ x: true }}
                            size="small"
                        />
                    </ProCard>
                )}
            </Spin>
        </PageContainer>
    );
};

export default Index;