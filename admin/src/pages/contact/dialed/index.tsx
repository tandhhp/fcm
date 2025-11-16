import { apiContactDialedCalls, apiExportDialedCalls } from "@/services/contact";
import { ExportOutlined } from "@ant-design/icons";
import { ModalForm, PageContainer, ProFormDateRangePicker, ProTable } from "@ant-design/pro-components"
import { Button } from "antd";
import { useState } from "react";

const Index: React.FC = () => {

    const [openExport, setOpenExport] = useState<boolean>(false);

    const onFinishExport = async (values: any) => {
        const response = await apiExportDialedCalls({
            fromDate: values.dateRange ? values.dateRange[0].format("YYYY-MM-DD") : undefined,
            toDate: values.dateRange ? values.dateRange[1].format("YYYY-MM-DD") : undefined,
        });
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `dialed_calls_${Date.now()}.xlsx`;
        a.click();
        window.URL.revokeObjectURL(url);
        return true;
    }

    return (
        <PageContainer extra={<Button onClick={() => setOpenExport(true)} type="primary" icon={<ExportOutlined />}>Xuất dữ liệu</Button>}>
            <ProTable
                request={apiContactDialedCalls}
                rowKey="id"
                scroll={{
                    x: true
                }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'SDT',
                        dataIndex: 'phoneNumber',
                        minWidth: 100,
                        width: 100
                    },
                    {
                        title: 'Tên liên hệ',
                        dataIndex: 'name',
                        minWidth: 150,
                    },
                    {
                        title: 'Ngày gọi',
                        dataIndex: 'calledAt',
                        valueType: 'dateTime',
                        search: false,
                        width: 150,
                        minWidth: 150
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'callStatusName',
                        search: false
                    },
                    {
                        title: 'Nguồn',
                        dataIndex: 'sourceName',
                        search: false
                    },
                    {
                        title: 'Người gọi',
                        dataIndex: 'teleName',
                        search: false
                    },
                    {
                        title: 'Extra',
                        dataIndex: 'extraStatus',
                        search: false
                    },
                    {
                        title: 'Tuổi',
                        dataIndex: 'age',
                        search: false
                    },
                    {
                        title: 'Công việc',
                        dataIndex: 'job',
                        search: false
                    },
                    {
                        title: 'Follow',
                        dataIndex: 'followUpDate',
                        valueType: 'date',
                        search: false,
                        width: 100,
                        minWidth: 100
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note',
                        search: false
                    }
                ]}
                search={{
                    layout: 'vertical'
                }}
            />
            <ModalForm 
                open={openExport}
                onOpenChange={setOpenExport}
                title="Xuất dữ liệu cuộc gọi đã gọi"
                width={400}
                onFinish={onFinishExport}
            >
                <ProFormDateRangePicker
                    name="dateRange"
                    label="Chọn khoảng ngày"
                    width="md"
                />
            </ModalForm>
        </PageContainer>
    )
}

export default Index;