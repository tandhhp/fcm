import access from "@/access";
import InvoiceEvidence from "@/components/invoices/evidence";
import { apiInvoiceList, apiInvoiceApprove } from "@/services/finances/invoice";
import { InvoiceStatus } from "@/utils/enum";
import { SettingOutlined, CheckOutlined, PictureOutlined, EditOutlined, CloseOutlined, MoreOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { useAccess } from "@umijs/max";
import { Popconfirm, message, Button, Dropdown } from "antd";
import { useRef, useState } from "react";
import InvoiceRejectForm from "../invoice/components/reject";

const Index: React.FC = () => {

    const access = useAccess();
    const actionRef = useRef<ActionType>(null);
    const [invoice, setInvoice] = useState<any>(null);
    const [openReject, setOpenReject] = useState<boolean>(false);
    const [openEvidence, setOpenEvidence] = useState<boolean>(false);

    return (
        <PageContainer>
            <ProTable
                actionRef={actionRef}
                request={apiInvoiceList}
                params={{
                    status: 0
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
                        title: 'Từ ngày',
                        dataIndex: 'fromDate',
                        valueType: 'date',
                        hideInTable: true
                    },
                    {
                        title: 'Đến ngày',
                        dataIndex: 'toDate',
                        valueType: 'date',
                        hideInTable: true
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
                        width: 80,
                        align: 'center',
                        render: (_, record) => [
                            <Popconfirm key={`approve`} title="Xác nhận duyệt hóa đơn?" onConfirm={async () => {
                                await apiInvoiceApprove(record.id);
                                message.success('Duyệt hóa đơn thành công');
                                actionRef.current?.reload();
                            }} okText="Duyệt" cancelText="Hủy">
                                <Button type="primary" icon={<CheckOutlined />} size="small" disabled={record.status !== InvoiceStatus.Pending || !access.accountant}></Button>
                            </Popconfirm>,
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
                                        key: 'reject',
                                        label: 'Từ chối',
                                        icon: <CloseOutlined />,
                                        disabled: record.status !== InvoiceStatus.Pending,
                                        onClick: () => {
                                            setInvoice(record);
                                            setOpenReject(true);
                                        }
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
            <InvoiceRejectForm open={openReject} onOpenChange={setOpenReject} data={invoice} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default Index;