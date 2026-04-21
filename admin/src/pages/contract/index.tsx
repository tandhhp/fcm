import { apiContractExport, apiContractList } from "@/services/finances/contract";
import { ArrowLeftOutlined, DeleteOutlined, ExportOutlined, GiftOutlined, ManOutlined, MoreOutlined, PhoneOutlined, PictureOutlined, PlusOutlined, SettingOutlined, TableOutlined, TagOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { useRef, useState } from "react";
import ContractInvoice from "./components/invoice";
import { Button, Dropdown, message, Popconfirm, Tag } from "antd";
import { FormattedNumber, useAccess } from "@umijs/max";
import { apiContractDelete } from "@/services/contact";
import GiftList from "./components/gift-list";
import ContractForm from "@/components/form/contract";
import BillForm from "./components/bill-form";
import CouponForm from "./components/coupon-form";
import ContractEvidence from "@/components/contract-evidence";
import ServiceUsageList from "./components/service-usage-list";
import LeaveVoucherUsageList from "./components/leave-voucher-usage-list";
import Coupon from "./components/coupon";
import dayjs from "dayjs";

const Index: React.FC = () => {

    const access = useAccess();
    const actionRef = useRef<ActionType>();
    const [openInvoice, setOpenInvoice] = useState<boolean>(false);
    const [openBillForm, setOpenBillForm] = useState<boolean>(false);
    const [contract, setContract] = useState<any>(null);
    const [loadingExport, setLoadingExport] = useState<boolean>(false);
    const [giftListOpen, setGiftListOpen] = useState<boolean>(false);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [couponFormOpen, setCouponFormOpen] = useState<boolean>(false);
    const [openEvidence, setOpenEvidence] = useState<boolean>(false);
    const [serviceUsageOpen, setServiceUsageOpen] = useState<boolean>(false);
    const [leaveVoucherUsageOpen, setLeaveVoucherUsageOpen] = useState<boolean>(false);
    const [couponOpen, setCouponOpen] = useState<boolean>(false);

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
            disabled={!access.em && !access.canAdmin && !access.dos}
            type="primary" onClick={onExport} loading={loadingExport}>Xuất dữ liệu</Button>}>
            <ProTable
                headerTitle={<Button type="primary" onClick={() => setOpenForm(true)} icon={<PlusOutlined />}
                    disabled={access.sales || access.telesale || access.telesaleManager || access.sm || access.legalExcutive}>Tạo hợp đồng</Button>}
                actionRef={actionRef}
                request={apiContractList}
                rowKey="id"
                search={{
                    layout: 'vertical'
                }}
                size="small"
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
                        title: 'Số hợp đồng',
                        dataIndex: 'contractCode',
                        render: (dom, record) => (
                            <div>
                                <div className="font-semibold">{dom}</div>
                                <div className="text-gray-500">Ngày tạo: {dayjs(record.createdDate).format('DD-MM-YYYY')}</div>
                                <div className="text-gray-500">Nguồn: {record.sourceName}</div>
                            </div>
                        )
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'customerName',
                        render: (_, record) => (
                            <div>
                                <div className="font-semibold">{record.gender ? <WomanOutlined className="text-pink-500" /> : <ManOutlined className="text-blue-500" />} {record.customerName}</div>
                                <div className="text-gray-500"><PhoneOutlined /> {record.phoneNumber}</div>
                                <div className="text-gray-500">CCCD: {record.identityNumber}</div>
                            </div>
                        )
                    },
                    {
                        title: 'SDT',
                        dataIndex: 'phoneNumber',
                        hideInTable: true
                    },
                    {
                        title: 'Số CCCD',
                        dataIndex: 'identityNumber',
                        hideInTable: true
                    },
                    {
                        title: 'Nhân sự',
                        dataIndex: 'salesName',
                        render: (_, record) => (
                            <div>
                                <div>Sales: {record.salesName}</div>
                                <div>SM: {record.smName}</div>
                                <div>DOS: {record.dos}</div>
                            </div>
                        ),
                        minWidth: 180
                    },
                    {
                        title: 'GTHĐ',
                        dataIndex: 'amount',
                        valueType: 'digit',
                        search: false,
                        tip: 'Giá trị hợp đồng',
                        render: (dom) => (
                            <Tag color="blue" className="w-full text-center font-semibold border-0 text-sm">{dom}₫</Tag>
                        ),
                        width: 100
                    },
                    {
                        title: 'GTTT',
                        valueType: 'digit',
                        tip: 'Giá trị thực thu',
                        search: false,
                        render: (_, record) => (
                            <Tag color="green" className="w-full text-center font-semibold border-0 text-sm">{<FormattedNumber value={record.amount - record.discount} />}₫</Tag>
                        ),
                        width: 100
                    },
                    {
                        title: 'Đã TT',
                        dataIndex: 'paidAmount',
                        valueType: 'digit',
                        search: false,
                        render: (dom) => (
                            <Tag color="orange" className="w-full text-center font-semibold border-0 text-sm">{dom}₫</Tag>
                        ),
                        width: 100
                    },
                    {
                        title: 'GTQD',
                        dataIndex: 'discount',
                        valueType: 'digit',
                        search: false,
                        tip: 'Giá trị quy đổi',
                        render: (dom, record) => (
                            <Tag color="red"
                                onClick={() => {
                                    setContract(record);
                                    setCouponOpen(true);
                                }}
                                className="w-full text-center font-semibold border-0 cursor-pointer hover:bg-red-100 text-sm">{dom}₫</Tag>
                        ),
                        width: 100
                    },
                    {
                        title: 'Chờ duyệt',
                        dataIndex: 'pendingAmount',
                        valueType: 'digit',
                        search: false,
                        render: (dom) => (
                            <Tag color="yellow" className="w-full text-center font-semibold border-0 cursor-pointer hover:bg-yellow-100 text-sm">{dom}₫</Tag>
                        ),
                        width: 100
                    },
                    {
                        title: 'Tỷ lệ TT',
                        dataIndex: 'paymentRate',
                        valueType: 'percent',
                        search: false,
                        render: (dom, record) => (
                            <Tag color="purple" className="w-full text-center font-semibold border-0 text-sm">{(record.paidAmount / record.amount * 100).toFixed(1)}%</Tag>
                        ),
                        width: 80
                    },
                    {
                        title: 'Cần TT',
                        dataIndex: 'remainingAmount',
                        valueType: 'digit',
                        search: false,
                        render: (dom, record) => (
                            <Tag color="pink" className="w-full text-center font-semibold border-0 text-sm">{<FormattedNumber value={(record.amount - record.discount - record.paidAmount)} />}₫</Tag>
                        ),
                        width: 100
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
                                        icon: <ArrowLeftOutlined />,
                                        disabled: access.legalExcutive || access.salesAdmin
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
                                        label: 'Quà tặng',
                                        onClick: () => {
                                            setContract(record);
                                            setGiftListOpen(true);
                                        },
                                        icon: <GiftOutlined />,
                                        disabled: !access.salesAdmin && !access.cx
                                    },
                                    {
                                        title: 'Thư viện ảnh',
                                        key: 'evidence',
                                        label: 'Thư viện ảnh',
                                        onClick: () => {
                                            setContract(record);
                                            setOpenEvidence(true);
                                        },
                                        icon: <PictureOutlined />
                                    },
                                    {
                                        key: 'coupon',
                                        label: 'Tạo phiếu quy đổi',
                                        onClick: () => {
                                            setContract(record);
                                            setCouponFormOpen(true);
                                        },
                                        icon: <TagOutlined />,
                                        disabled: access.legalExcutive || access.cx
                                    },
                                    {
                                        key: 'service-usage',
                                        label: 'Phiếu sử dụng dịch vụ',
                                        onClick: () => {
                                            setContract(record);
                                            setServiceUsageOpen(true);
                                        },
                                        icon: <TagOutlined />,
                                        disabled: !access.cx
                                    },
                                    {
                                        key: 'leave-voucher-usage',
                                        label: 'Phiếu sử dụng quyền nghỉ',
                                        onClick: () => {
                                            setContract(record);
                                            setLeaveVoucherUsageOpen(true);
                                        },
                                        icon: <TagOutlined />,
                                        disabled: !access.cx
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
            <ContractEvidence contractId={contract?.id} onOpenChange={setOpenEvidence} open={openEvidence} />
            <ContractInvoice open={openInvoice} onOpenChange={setOpenInvoice} data={contract} />
            <GiftList open={giftListOpen} onOpenChange={setGiftListOpen} data={contract} />
            <ContractForm open={openForm} onOpenChange={setOpenForm} reload={() => actionRef.current?.reload()} />
            <BillForm open={openBillForm} onOpenChange={setOpenBillForm} data={contract} reload={() => actionRef.current?.reload()} />
            <CouponForm open={couponFormOpen} onOpenChange={setCouponFormOpen} data={contract} reload={() => actionRef.current?.reload()} />
            <ServiceUsageList open={serviceUsageOpen} onOpenChange={setServiceUsageOpen} data={contract} />
            <LeaveVoucherUsageList open={leaveVoucherUsageOpen} onOpenChange={setLeaveVoucherUsageOpen} data={contract} />
            <Coupon open={couponOpen} onClose={() => setCouponOpen(false)} data={contract} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default Index;