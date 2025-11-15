import { apiContractExport, apiContractList } from "@/services/finances/contract";
import { ArrowLeftOutlined, DeleteOutlined, ExportOutlined, GiftOutlined, ManOutlined, MoreOutlined, PlusOutlined, SettingOutlined, TableOutlined, TagOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { useRef, useState } from "react";
import ContractInvoice from "./components/invoice";
import { Button, Dropdown, message, Popconfirm } from "antd";
import { FormattedNumber, useAccess } from "@umijs/max";
import { apiContractDelete } from "@/services/contact";
import GiftForm from "./components/gift-form";
import GiftList from "./components/gift-list";
import ContractForm from "@/components/form/contract";
import BillForm from "./components/bill-form";
import CouponForm from "./components/coupon-form";

const Index: React.FC = () => {

    const access = useAccess();
    const actionRef = useRef<ActionType>();
    const [openInvoice, setOpenInvoice] = useState<boolean>(false);
    const [openBillForm, setOpenBillForm] = useState<boolean>(false);
    const [contract, setContract] = useState<any>(null);
    const [loadingExport, setLoadingExport] = useState<boolean>(false);
    const [giftFormOpen, setGiftFormOpen] = useState<boolean>(false);
    const [giftListOpen, setGiftListOpen] = useState<boolean>(false);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [couponFormOpen, setCouponFormOpen] = useState<boolean>(false);

    const onDelete = async (id: string) => {
        await apiContractDelete(id);
        actionRef.current?.reload();
        message.success('Xoá hợp đồng thành công');
    }

    const onExport = async () => {
        setLoadingExport(true);
        const response = await apiContractExport();
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', `contracts.xlsx`);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
        setLoadingExport(false);
    }

    return (
        <PageContainer extra={<Button icon={<ExportOutlined />}
            disabled={access.sales || access.telesale || access.telesaleManager || access.sm}
            type="primary" onClick={onExport} loading={loadingExport}>Xuất dữ liệu</Button>}>
            <ProTable
                headerTitle={<Button type="primary" onClick={() => setOpenForm(true)} icon={<PlusOutlined />}
                    disabled={access.sales || access.telesale || access.telesaleManager || access.sm}>Tạo hợp đồng</Button>}
                actionRef={actionRef}
                request={apiContractList}
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
                        title: 'Số hợp đồng',
                        dataIndex: 'contractCode',
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'customerName',
                        render: (_, record) => {
                            if (record.gender) {
                                return <><WomanOutlined className="text-pink-500" /> {record.customerName}</>
                            }
                            return <><ManOutlined className="text-blue-500" /> {record.customerName}</>
                        }
                    },
                    {
                        title: 'SDT',
                        dataIndex: 'phoneNumber',
                    },
                    {
                        title: 'Số CCCD',
                        dataIndex: 'identityNumber',
                    },
                    {
                        title: 'Ngày tạo',
                        dataIndex: 'createdDate',
                        valueType: 'date',
                        search: false
                    },
                    {
                        title: 'GTHĐ',
                        dataIndex: 'amount',
                        valueType: 'digit',
                        search: false,
                        tip: 'Giá trị hợp đồng',
                    },
                    {
                        title: 'GTQD',
                        dataIndex: 'discount',
                        valueType: 'digit',
                        search: false,
                        tip: 'Giá trị quy đổi',
                    },
                    {
                        title: 'GTTT',
                        valueType: 'digit',
                        tip: 'Giá trị thực thu',
                        search: false,
                        render: (_, record) => <FormattedNumber value={record.amount - record.discount} />
                    },
                    {
                        title: 'Đã TT',
                        dataIndex: 'paidAmount',
                        valueType: 'digit',
                        search: false
                    },
                    {
                        title: 'Chờ duyệt',
                        dataIndex: 'pendingAmount',
                        valueType: 'digit',
                        search: false
                    },
                    {
                        title: 'Quà tặng',
                        dataIndex: 'giftCount',
                        valueType: 'digit',
                        search: false,
                        width: 80,
                        render: (dom, record) => (
                            <Button type="primary" size="small" onClick={() => {
                                setContract(record);
                                setGiftListOpen(true);
                            }}>{dom}</Button>
                        )
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        render: (_, record) => [
                            <Dropdown key="more" menu={{
                                items: [
                                    {
                                        key: 'Tạo phiếu chi',
                                        label: 'Tạo phiếu chi',
                                        onClick: () => {
                                            setContract(record);
                                            setOpenBillForm(true);
                                        },
                                        icon: <ArrowLeftOutlined />
                                    },
                                    {
                                        key: 'invoice',
                                        label: 'Danh sách phiếu thu',
                                        onClick: () => {
                                            setContract(record);
                                            setOpenInvoice(true);
                                        },
                                        icon: <TableOutlined />
                                    },
                                    {
                                        key: 'gift',
                                        label: 'Tặng quà',
                                        onClick: () => {
                                            setContract(record);
                                            setGiftFormOpen(true);
                                        },
                                        icon: <GiftOutlined />,
                                        disabled: !access.canCX
                                    },
                                    {
                                        key: 'gift-list',
                                        label: 'Danh sách quà tặng',
                                        onClick: () => {
                                            setContract(record);
                                            setGiftListOpen(true);
                                        },
                                        icon: <TableOutlined />
                                    },
                                    {
                                        key: 'coupon',
                                        label: 'Tạo phiếu quy đổi',
                                        onClick: () => {
                                            setContract(record);
                                            setCouponFormOpen(true);
                                        },
                                        icon: <TagOutlined />
                                    }
                                ]
                            }}>
                                <Button type="dashed" size="small" icon={<MoreOutlined />} />
                            </Dropdown>,
                            <Popconfirm key={"delete"} title="Xoá hợp đồng?" onConfirm={() => onDelete(record.id)}>
                                <Button type="primary" danger size="small" disabled={!access.canAdmin} icon={<DeleteOutlined />} />
                            </Popconfirm>
                        ],
                        width: 40,
                        align: 'center'
                    }
                ]}
            />
            <ContractInvoice open={openInvoice} onOpenChange={setOpenInvoice} data={contract} />
            <GiftForm open={giftFormOpen} onOpenChange={setGiftFormOpen} data={contract} reload={() => actionRef.current?.reload()} />
            <GiftList open={giftListOpen} onOpenChange={setGiftListOpen} data={contract} />
            <ContractForm open={openForm} onOpenChange={setOpenForm} reload={() => actionRef.current?.reload()} />
            <BillForm open={openBillForm} onOpenChange={setOpenBillForm} data={contract} reload={() => actionRef.current?.reload()} />
            <CouponForm open={couponFormOpen} onOpenChange={setCouponFormOpen} data={contract} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default Index;