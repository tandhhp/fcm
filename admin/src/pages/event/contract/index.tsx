import { apiContractExport, apiContractList } from "@/services/finances/contract";
import { EditOutlined, ExportOutlined, ManOutlined, MoneyCollectOutlined, MoreOutlined, PictureOutlined, PlusOutlined, SettingOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { useRef, useState } from "react";
import { Button, Dropdown, Tag } from "antd";
import ContractPayment from "./components/payment";
import ContractInvoice from "./components/invoice";
import { useAccess } from "@umijs/max";
import ContractForm from "@/components/form/contract";
import ContractEvidence from "@/components/contract-evidence";
import dayjs from "dayjs";

const Index: React.FC = () => {

    const access = useAccess();
    const actionRef = useRef<ActionType>();
    const [contract, setContract] = useState<any>(null);
    const [openPayment, setOpenPayment] = useState<boolean>(false);
    const [openInvoice, setOpenInvoice] = useState<boolean>(false);
    const [loadingExport, setLoadingExport] = useState<boolean>(false);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [openEvidence, setOpenEvidence] = useState<boolean>(false);

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
        <PageContainer extra={<Button icon={<ExportOutlined />} type="primary"
            disabled={access.sales || access.telesale || access.telesaleManager || access.sm}
            onClick={onExport} loading={loadingExport}>Xuất dữ liệu</Button>}>
            <ProTable
                headerTitle={<Button type="primary" onClick={() => {
                    setContract(null);
                    setOpenForm(true);
                }} icon={<PlusOutlined />}
                    disabled={access.sales || access.telesale || access.telesaleManager || access.sm}
                >Tạo hợp đồng</Button>}
                actionRef={actionRef}
                request={apiContractList}
                rowKey="id"
                search={{
                    layout: 'vertical'
                }}
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
                        width: 200,
                        minWidth: 200,
                        render: (dom, record) => (
                            <div>
                                <div className="font-semibold">{dom}</div>
                                <div className="text-gray-500">Ngày tạo: {dayjs(record.createdDate).format('DD-MM-YYYY')}</div>
                                <div className="text-gray-500">Nguồn: <span className="text-red-500 font-semibold">{record.sourceName}</span></div>
                            </div>
                        )
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'customerName',
                        render: (_, record) => {
                            if (record.gender) {
                                return <><WomanOutlined className="text-pink-500" /> {record.customerName}</>
                            }
                            return <><ManOutlined className="text-blue-500" /> {record.customerName}</>
                        },
                        width: 200,
                        minWidth: 150
                    },
                    {
                        title: 'Năm sinh',
                        dataIndex: 'dateOfBirth',
                        valueType: 'dateYear',
                        search: false,
                        width: 100,
                        minWidth: 100
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
                        title: 'Ngày chốt',
                        dataIndex: 'createdDate',
                        valueType: 'date',
                        search: false,
                        width: 100,
                        minWidth: 100
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
                    },
                    {
                        title: 'Đã thanh toán',
                        dataIndex: 'paidAmount',
                        valueType: 'digit',
                        search: false,
                        render: (dom) => (
                            <Tag color="success" className="w-full text-center font-semibold border-0 text-sm">{dom}₫</Tag>
                        ),
                    },
                    {
                        title: 'Chờ duyệt',
                        dataIndex: 'pendingAmount',
                        valueType: 'digit',
                        search: false,
                        render: (dom) => (
                            <Tag color="orange" className="w-full text-center font-semibold border-0 text-sm">{dom}₫</Tag>
                        ),
                    },
                    {
                        title: 'SL phiếu',
                        dataIndex: 'invoiceCount',
                        valueType: 'digit',
                        search: false
                    },
                    {
                        title: 'Khoảng thời gian',
                        valueType: 'dateRange',
                        dataIndex: 'dateRange',
                        hideInTable: true
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        render: (_, record) => [
                            <Dropdown key="more" menu={{
                                items: [
                                    {
                                        key: 'payment',
                                        label: 'Tạo phiếu thu',
                                        onClick: () => {
                                            setContract(record);
                                            setOpenPayment(true);
                                        },
                                        icon: <MoneyCollectOutlined />
                                    },
                                    {
                                        key: 'invoice',
                                        label: 'Xem phiếu thu',
                                        onClick: () => {
                                            setContract(record);
                                            setOpenInvoice(true);
                                        },
                                        icon: <MoneyCollectOutlined />
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
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        onClick: () => {
                                            setContract(record);
                                            setOpenForm(true);
                                        },
                                        icon: <EditOutlined />
                                    }
                                ]
                            }}>
                                <Button type="dashed" size="small" icon={<MoreOutlined />} />
                            </Dropdown>
                        ],
                        width: 40,
                        align: 'center'
                    }
                ]}
            />
            <ContractPayment open={openPayment} data={contract} onOpenChange={setOpenPayment} reload={() => actionRef.current?.reload()} />
            <ContractInvoice open={openInvoice} data={contract} onOpenChange={setOpenInvoice} />
            <ContractForm open={openForm} onOpenChange={setOpenForm} reload={() => actionRef.current?.reload()} data={contract} />
            <ContractEvidence contractId={contract?.id} onOpenChange={setOpenEvidence} open={openEvidence} />
        </PageContainer>
    )
}

export default Index;