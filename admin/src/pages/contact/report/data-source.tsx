import { apiReportDataSource, apiReportDataSourceExport } from "@/services/contact";
import { apiSourceOptions } from "@/services/settings/source";
import { apiTeamOptions } from "@/services/users/team";
import { ExportOutlined } from "@ant-design/icons";
import { PageContainer, ProCard, ProFormDateRangePicker, ProFormSelect } from "@ant-design/pro-components";
import { Button, Col, Form, Row, Space, Spin, Table, Typography, message } from "antd";
import dayjs from "dayjs";
import { useEffect, useState } from "react";

const { Title, Text } = Typography;

const Index: React.FC = () => {
    const [form] = Form.useForm();
    const selectedTeamId = Form.useWatch<number | undefined>('teamId', form);
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState<any>(null);
    const [exporting, setExporting] = useState(false);

    const [sourceOptions, setSourceOptions] = useState<{ label: string; value: string }[]>([]);

    useEffect(() => {
        const fetchSourceOptions = async () => {
            try {
                const response = await apiSourceOptions({ teamId: selectedTeamId });
                setSourceOptions(response);
            } catch (error) {
                message.error('Lỗi khi tải danh sách nguồn');
            }
        };

        fetchSourceOptions();
    }, [selectedTeamId]);

    const handleSearch = async () => {
        try {
            setLoading(true);
            const values = form.getFieldsValue();
            const response = await apiReportDataSource({
                teamId: values.teamId,
                fromDate: values.dateRange?.[0]?.format('YYYY-MM-DD'),
                toDate: values.dateRange?.[1]?.format('YYYY-MM-DD'),
                sourceId: values.sourceId,
            });
            setData(response.data);
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
            const response = await apiReportDataSourceExport({
                teamId: values.teamId,
                fromDate: values.dateRange?.[0]?.format('YYYY-MM-DD'),
                toDate: values.dateRange?.[1]?.format('YYYY-MM-DD'),
                sourceId: values.sourceId,
            });
            
            const blob = new Blob([response], { 
                type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' 
            });
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `report_data_source_${dayjs().format('YYYYMMDD_HHmmss')}.xlsx`;
            a.click();
            window.URL.revokeObjectURL(url);
            message.success('Xuất file thành công');
        } catch (error) {
            message.error('Lỗi khi xuất file');
        } finally {
            setExporting(false);
        }
    };

    const baseColumns = [
        {
            title: 'Tên nguồn',
            dataIndex: 'sourceName',
            key: 'sourceName',
            fixed: 'left' as const,
            width: 200,
        },
        {
            title: 'Nhập',
            dataIndex: 'contactImport',
            key: 'contactImport',
            align: 'right' as const,
            width: 100,
        },
        {
            title: 'Tổng liên hệ',
            dataIndex: 'total',
            key: 'total',
            align: 'right' as const,
            width: 120,
        },
        {
            title: 'CF1',
            dataIndex: 'cF1',
            key: 'cF1',
            align: 'right' as const,
            width: 100,
        },
        {
            title: 'Showup',
            dataIndex: 'showup',
            key: 'showup',
            align: 'right' as const,
            width: 100,
        },
        {
            title: 'Deal',
            dataIndex: 'deal',
            key: 'deal',
            align: 'right' as const,
            width: 100,
        },
        {
            title: '% CF/Tổng liên hệ',
            dataIndex: 'percentCFTotalContacted',
            key: 'percentCFTotalContacted',
            align: 'right' as const,
            width: 150,
            render: (val: number) => `${val?.toFixed(2)}%`,
        },
        {
            title: '% Showup/CF',
            dataIndex: 'percentShowupCF',
            key: 'percentShowupCF',
            align: 'right' as const,
            width: 120,
            render: (val: number) => `${val?.toFixed(2)}%`,
        },
        {
            title: '% Deal/Showup',
            dataIndex: 'percentDealShowup',
            key: 'percentDealShowup',
            align: 'right' as const,
            width: 140,
            render: (val: number) => `${val?.toFixed(2)}%`,
        },
    ];

    const teamColumns = [
        {
            title: 'Nhóm nguồn',
            dataIndex: 'sourceGroup',
            key: 'sourceGroup',
            fixed: 'left' as const,
            width: 200,
        },
        ...baseColumns.slice(1),
    ];

    return (
        <PageContainer>
            <ProCard style={{ marginBottom: 16 }}>
                <Form
                    form={form}
                    layout="vertical"
                    onValuesChange={(changedValues) => {
                        if (Object.prototype.hasOwnProperty.call(changedValues, 'teamId')) {
                            form.setFieldValue('sourceId', undefined);
                        }
                    }}
                >
                    <Row gutter={16}>
                        <Col span={6}>
                            <ProFormSelect
                                name="teamId"
                                label="Nhóm"
                                placeholder="Chọn nhóm"
                                request={apiTeamOptions}
                                fieldProps={{
                                    allowClear: true,
                                }}
                                showSearch
                            />
                        </Col>
                        <Col span={6}>
                            <ProFormSelect
                                name="sourceId"
                                label="Nguồn"
                                placeholder="Chọn nguồn"
                                options={sourceOptions}
                                fieldProps={{
                                    allowClear: true,
                                }}
                                showSearch
                            />
                        </Col>
                        <Col span={4}>
                            <ProFormDateRangePicker
                                name="dateRange"
                                label="Khoảng thời gian"
                                placeholder={['Từ ngày', 'Đến ngày']}
                                fieldProps={{
                                    format: 'DD/MM/YYYY',
                                }}
                                width={"xl"}
                            />
                        </Col>
                        <Col span={6} style={{ display: 'flex', alignItems: 'flex-end' }}>
                            <Space className="mb-6">
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
                    <>
                        <ProCard title="Tổng hợp" style={{ marginBottom: 16 }}>
                            <Row gutter={[16, 16]}>
                                <Col span={6}>
                                    <Text strong>Nhóm nguồn:</Text> {data.total?.sourceGroup || 'N/A'}
                                </Col>
                                <Col span={6}>
                                    <Text strong>Contact đầu Case:</Text> {data.total?.contactStartCase || 0}
                                </Col>
                                <Col span={6}>
                                    <Text strong>Tele không update:</Text> {data.total?.teleNotUpdate || 0}
                                </Col>
                            </Row>
                            <Table
                                style={{ marginTop: 16 }}
                                columns={[
                                    { title: 'Nhập', dataIndex: 'contactImport', align: 'right' },
                                    { title: 'Tổng liên hệ', dataIndex: 'total', align: 'right' },
                                    { title: 'CF1', dataIndex: 'cF1', align: 'right' },
                                    { title: 'Showup', dataIndex: 'showup', align: 'right' },
                                    { title: 'Deal', dataIndex: 'deal', align: 'right' },
                                    { 
                                        title: '% CF/Tổng liên hệ', 
                                        dataIndex: 'percentCFTotalContacted', 
                                        align: 'right',
                                        render: (val: number) => `${val?.toFixed(2)}%`,
                                    },
                                    { 
                                        title: '% Showup/CF', 
                                        dataIndex: 'percentShowupCF', 
                                        align: 'right',
                                        render: (val: number) => `${val?.toFixed(2)}%`,
                                    },
                                    { 
                                        title: '% Deal/Showup', 
                                        dataIndex: 'percentDealShowup', 
                                        align: 'right',
                                        render: (val: number) => `${val?.toFixed(2)}%`,
                                    },
                                ]}
                                dataSource={[data.total]}
                                pagination={false}
                                size="small"
                                scroll={{ x: true }}
                            />
                        </ProCard>

                        <ProCard title="Chi tiết theo đội nhóm">
                            <Table
                                columns={teamColumns}
                                dataSource={data.teams}
                                rowKey={(record: any) => `${record.sourceGroup}-${record.sourceName}`}
                                pagination={false}
                                expandable={{
                                    expandedRowRender: (record: any) => (
                                        <Table
                                            columns={baseColumns}
                                            dataSource={record.sources}
                                            rowKey="sourceName"
                                            pagination={false}
                                            size="small"
                                            scroll={{ x: true }}
                                        />
                                    ),
                                    rowExpandable: (record: any) => record.sources && record.sources.length > 0,
                                }}
                                scroll={{ x: true }}
                                size="small"
                            />
                        </ProCard>
                    </>
                )}
            </Spin>
        </PageContainer>
    );
};

export default Index;