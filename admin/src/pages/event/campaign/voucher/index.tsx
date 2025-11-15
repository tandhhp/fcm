import { apiCampaignOptions } from "@/services/event/campaign";
import { apiVoucherDelete, apiVoucherExport, apiVoucherList } from "@/services/event/voucher";
import { DeleteOutlined, EditOutlined, ExportOutlined, ImportOutlined, MoreOutlined, PlusOutlined, SettingOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { useParams } from "@umijs/max";
import { Button, Dropdown, message, Popconfirm, Space } from "antd";
import { useRef, useState } from "react";
import VoucherForm from "./components/form";
import VoucherImport from "./components/import";

const Index: React.FC = () => {

    const { id } = useParams();
    const actionRef = useRef<ActionType>(null);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [selectedRow, setSelectedRow] = useState<any>(null);
    const [openImport, setOpenImport] = useState<boolean>(false);
    const [loadingExport, setLoadingExport] = useState<boolean>(false);

    const onDelete = async (id: any) => {
        await apiVoucherDelete(id);
        message.success('Đã xóa voucher');
        actionRef.current?.reload();
    }

    const onExport = async () => {
        setLoadingExport(true);
        const response = await apiVoucherExport();
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', `vouchers.xlsx`);
        document.body.appendChild(link);
        link.click();
        link.parentNode?.removeChild(link);
        window.URL.revokeObjectURL(url);
        setLoadingExport(false);
    }

    return (
        <PageContainer
            extra={<Button icon={<ExportOutlined />} type="primary" onClick={onExport} loading={loadingExport}>Xuất dữ liệu</Button>}>
            <ProTable
                headerTitle={(
                    <Space>
                        <Button type="primary" icon={<PlusOutlined />} onClick={() => {
                            setSelectedRow(null);
                            setOpenForm(true);
                        }}>Thêm voucher</Button>
                        <Button type="default" icon={<ImportOutlined />} onClick={() => setOpenImport(true)}>Nhập voucher</Button>
                    </Space>
                )}
                actionRef={actionRef}
                params={{
                    campaignId: id
                }}
                request={apiVoucherList}
                rowKey={"id"}
                search={{
                    layout: 'vertical'
                }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Mã voucher',
                        dataIndex: 'name'
                    },
                    {
                        title: 'Hạn sử dụng (ngày)',
                        dataIndex: 'expiredDays',
                        search: false
                    },
                    {
                        title: 'Loại',
                        dataIndex: 'campaignId',
                        valueType: 'select',
                        request: apiCampaignOptions,
                        search: false
                    },
                    {
                        title: 'Ngày kích hoạt',
                        dataIndex: 'activeAt',
                        valueType: 'date',
                        search: false
                    },
                    {
                        title: 'Ngày hết hạn',
                        dataIndex: 'expiredDate',
                        valueType: 'date',
                        search: false
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'status',
                        valueType: 'select',
                        valueEnum: {
                            0: { text: 'Chưa sử dụng', status: 'Default' },
                            1: { text: 'Đã sử dụng', status: 'Success' },
                            2: { text: 'Hết hạn', status: 'Error' },
                            3: { text: 'Đã kích hoạt', status: 'Processing' },
                            4: { text: 'Đã hủy', status: 'Warning' },
                            5: { text: 'Từ chối', status: 'Warning' }
                        },
                        render: (dom, record) => {
                            if (record.isExpired) {
                                return <span className="text-red-500 font-semibold">Hết hạn</span>
                            }
                            return dom;
                        }
                    },
                    {
                        title: 'Khách hàng',
                        dataIndex: 'customerName',
                        search: false
                    },
                    {
                        title: 'SDT',
                        dataIndex: 'customerPhone',
                        search: false
                    },
                    {
                        title: 'Số CCCD',
                        dataIndex: 'customerIdNumber',
                        search: false
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note',
                        search: false
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 50,
                        render: (text, record) => [
                            <Dropdown key={"more"} menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        onClick: () => {
                                            setSelectedRow(record);
                                            setOpenForm(true);
                                        },
                                        icon: <EditOutlined />
                                    }
                                ]
                            }}>
                                <Button type="dashed" icon={<MoreOutlined />} size="small" />
                            </Dropdown>,
                            <Popconfirm key={'delete'} title="Bạn có chắc chắn muốn xóa voucher này?" onConfirm={() => onDelete(record.id)}>
                                <Button type="primary" danger size="small" icon={<DeleteOutlined />}></Button>
                            </Popconfirm>,
                        ]
                    }
                ]}
            />
            <VoucherForm open={openForm} onOpenChange={setOpenForm} data={selectedRow} reload={() => actionRef.current?.reload()} />
            <VoucherImport open={openImport} onOpenChange={setOpenImport} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default Index;