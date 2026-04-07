import { apiReportMultipleAssign, apiReportMultipleAssignExport } from "@/services/contact";
import { apiTeamOptions } from "@/services/users/team";
import { ExportOutlined } from "@ant-design/icons";
import { PageContainer, ProCard, ProFormDateRangePicker, ProFormSelect } from "@ant-design/pro-components";
import { Button, Col, Form, Row, Space, Spin, Table, message } from "antd";
import dayjs from "dayjs";
import { useState } from "react";

const Index: React.FC = () => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState<any[]>([]);
    const [exporting, setExporting] = useState(false);

    const handleSearch = async () => {
        try {
            setLoading(true);
            const values = form.getFieldsValue();
            const response = await apiReportMultipleAssign({
                teamId: values.teamId,
                fromDate: values.dateRange?.[0]?.format('YYYY-MM-DD'),
                toDate: values.dateRange?.[1]?.format('YYYY-MM-DD'),
            });
            setData(response.data || []);
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
            const response = await apiReportMultipleAssignExport({
                teamId: values.teamId,
                fromDate: values.dateRange?.[0]?.format('YYYY-MM-DD'),
                toDate: values.dateRange?.[1]?.format('YYYY-MM-DD'),
            });
            
            const blob = new Blob([response], { 
                type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' 
            });
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `report_multiple_assign_${dayjs().format('YYYYMMDD_HHmmss')}.xlsx`;
            a.click();
            window.URL.revokeObjectURL(url);
            message.success('Xuất file thành công');
        } catch (error) {
            message.error('Lỗi khi xuất file');
        } finally {
            setExporting(false);
        }
    };

    const columns = [
        {
            title: 'Tên đội nhóm',
            dataIndex: 'teamName',
            key: 'teamName',
            width: 200,
        },
        {
            title: 'Tên nguồn',
            dataIndex: 'sourceName',
            key: 'sourceName',
            width: 200,
        },
        {
            title: 'Tổng phân công',
            dataIndex: 'totalAssigned',
            key: 'totalAssigned',
            align: 'right' as const,
            width: 150,
        },
        {
            title: 'Đang sử dụng',
            dataIndex: 'totalUsingAssigned',
            key: 'totalUsingAssigned',
            align: 'right' as const,
            width: 150,
        },
        {
            title: 'Còn lại',
            dataIndex: 'totalRemainingAssigned',
            key: 'totalRemainingAssigned',
            align: 'right' as const,
            width: 150,
        },
    ];

    return (
        <PageContainer>
            <ProCard style={{ marginBottom: 16 }}>
                <Form
                    form={form}
                    layout="vertical"
                    initialValues={{
                        dateRange: [dayjs().startOf('month'), dayjs().endOf('month')]
                    }}
                >
                    <Row gutter={16}>
                        <Col span={8}>
                            <ProFormDateRangePicker
                                name="dateRange"
                                label="Khoảng thời gian"
                                placeholder={['Từ ngày', 'Đến ngày']}
                                fieldProps={{
                                    format: 'DD/MM/YYYY'
                                }}
                            />
                        </Col>
                        <Col span={8}>
                            <ProFormSelect
                                name="teamId"
                                label="Group"
                                placeholder="Chọn đội nhóm"
                                request={apiTeamOptions}
                                showSearch
                                allowClear
                            />
                        </Col>
                        <Col span={8} style={{ display: 'flex', alignItems: 'flex-end', paddingBottom: 24 }}>
                            <Space>
                                <Button type="primary" onClick={handleSearch} loading={loading}>
                                    Tìm kiếm
                                </Button>
                                <Button 
                                    icon={<ExportOutlined />} 
                                    onClick={handleExport} 
                                    loading={exporting}
                                    disabled={data.length === 0}
                                >
                                    Xuất Excel
                                </Button>
                            </Space>
                        </Col>
                    </Row>
                </Form>
            </ProCard>

            <ProCard>
                <Spin spinning={loading}>
                    <Table
                        columns={columns}
                        dataSource={data}
                        size="small"
                        rowKey={(record, index) => `${record.teamName}_${record.sourceName}_${index}`}
                        pagination={{
                            showSizeChanger: true,
                            showTotal: (total) => `Tổng ${total} bản ghi`,
                            defaultPageSize: 20,
                            pageSizeOptions: ['10', '20', '50', '100'],
                        }}
                        scroll={{ x: 'max-content' }}
                        bordered
                    />
                </Spin>
            </ProCard>
        </PageContainer>
    )
}

export default Index;