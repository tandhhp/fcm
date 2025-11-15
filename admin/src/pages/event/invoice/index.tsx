import { apiInvoiceList } from "@/services/finances/invoice";
import { EditOutlined, MoreOutlined, PictureOutlined, SettingOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components";
import { Button, Dropdown } from "antd";
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
                        search: false
                    },
                    {
                        title: 'Ngày thu',
                        dataIndex: 'createdAt',
                        valueType: 'date',
                        search: false
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
                            2: { text: 'Từ chối', status: 'Error' }
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
                        width: 60,
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
                            </Dropdown>
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