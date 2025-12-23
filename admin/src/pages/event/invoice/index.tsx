import { apiInvoiceDelete, apiInvoiceList } from "@/services/finances/invoice";
import { DeleteOutlined, EditOutlined, MoreOutlined, PictureOutlined, SettingOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components";
import { Button, Dropdown, message, Popconfirm } from "antd";
import { useRef, useState } from "react";
import InvoiceEvidence from "@/components/invoices/evidence";
import InvoiceExportForm from "@/components/form/invoice-export-form";
import InvoiceForm from "@/components/form/invoice";
import { useAccess } from "@umijs/max";

const Index: React.FC = () => {

    const actionRef = useRef<ActionType>(null);
    const [invoice, setInvoice] = useState<any>(null);
    const [openEvidence, setOpenEvidence] = useState<boolean>(false);
    const [filterOptions, setFilterOptions] = useState<any>({});
    const [openForm, setOpenForm] = useState<boolean>(false);
    const access = useAccess();

    const onDelete = async (id: string) => {
        await apiInvoiceDelete(id);
        message.success('Xóa phiếu thu thành công');
        actionRef.current?.reload();
    }

    return (
        <PageContainer extra={<InvoiceExportForm exportOptions={filterOptions} />}>
            <ProTable
                actionRef={actionRef}
                request={(params) => {
                    setFilterOptions(params);
                    return apiInvoiceList(params);
                }}
                rowKey="id"
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
                        title: 'Số phiếu thu',
                        dataIndex: 'invoiceNumber',
                    },
                    {
                        title: 'Số hợp đồng',
                        dataIndex: 'contractCode',
                    },
                    {
                        title: 'Số tiền',
                        dataIndex: 'amount',
                        valueType: 'digit',
                        search: false,
                        width: 120
                    },
                    {
                        title: 'Ngày thu',
                        dataIndex: 'createdAt',
                        valueType: 'date',
                        search: false,
                        width: 100
                    },
                    {
                        title: 'Thời gian',
                        dataIndex: 'dateRange',
                        valueType: 'dateRange',
                        hideInTable: true
                    },
                    {
                        title: 'Phương thức',
                        dataIndex: 'paymentMethod',
                        valueEnum: {
                            0: { text: 'Chuyển khoản' },
                            1: { text: 'Thẻ' },
                            2: { text: 'Tiền mặt' }
                        },
                        search: false
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'status',
                        valueEnum: {
                            0: { text: 'Chờ duyệt', status: 'Default' },
                            1: { text: 'Đã duyệt', status: 'Success' },
                            2: { text: 'Từ chối', status: 'Error' },
                            3: { text: 'Đã hủy', status: 'Warning' },
                            4: { text: 'SA xác nhận', status: 'Processing' }
                        }
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note',
                        search: false,
                        ellipsis: true
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 70,
                        align: 'center',
                        render: (_, record) => [
                            <Dropdown key={"more"} menu={{
                                items: [
                                    {
                                        key: 'evidence',
                                        label: 'Xem chứng từ',
                                        icon: <PictureOutlined />,
                                        onClick: () => {
                                            setInvoice(record);
                                            setOpenEvidence(true);
                                        }
                                    },
                                    {
                                        key: 'edit',
                                        label: 'Cập nhật',
                                        icon: <EditOutlined />,
                                        onClick: () => {
                                            setInvoice(record);
                                            setOpenForm(true);
                                        },
                                        disabled: !access.event && !access.accountant || record.status !== 0
                                    }
                                ]
                            }}>
                                <Button type="dashed" icon={<MoreOutlined />} size="small" />
                            </Dropdown>,
                            <Popconfirm title="Bạn có chắc chắn muốn xóa?" onConfirm={() => onDelete(record.id)}>
                                <Button key={"delete"} type="primary" icon={<DeleteOutlined />} danger size="small" disabled={!access.event || record.status !== 0}></Button>
                            </Popconfirm>
                        ]
                    }
                ]}
            />
            <InvoiceEvidence open={openEvidence} onOpenChange={setOpenEvidence} data={invoice} />
            <InvoiceForm open={openForm} onOpenChange={setOpenForm} data={invoice} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default Index;