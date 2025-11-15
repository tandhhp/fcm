import { apiQueueCardHolderStatus, apiUpdateLeadStatus, apiUsersInEvent } from "@/services/contact";
import { CheckOutlined, DeleteOutlined, EyeOutlined, FileExcelOutlined, MailOutlined, MoreOutlined, PhoneOutlined, UserAddOutlined, UsergroupAddOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProColumnType, ProFormDatePicker, ProTable } from "@ant-design/pro-components"
import { Button, Dropdown, message, Popconfirm, Tag, Tooltip } from "antd";
import { useRef, useState } from "react";
import { LeadStatus } from "@/utils/constants";
import { useAccess } from "@umijs/max";
import CardHolderForm from "../member/components/form";
import dayjs from "dayjs";
import './max-img.css';
import SubLead from "@/pages/event/components/sublead";
import { apiExportLead } from "@/services/user";
import Evidence from "./components/evidence";
import { apiEventOptions } from "@/services/event";
import { apiBranchOptions } from "@/services/settings/branch";
import { apiSourceOptions } from "@/services/settings/source";
import { apiVoucherOptions } from "@/services/event/voucher";

const CardHolderQueue: React.FC = () => {

    const actionRef = useRef<ActionType>();
    const access = useAccess();
    const [openCardHolder, setOpenCardHolder] = useState<boolean>(false);
    const [lead, setLead] = useState<any>();
    const [openSubLead, setOpenSubLead] = useState<boolean>(false);
    const [openE, setOpenE] = useState<boolean>(false);
    const [openEvidence, setOpenEvidence] = useState<boolean>(false);

    const columns: ProColumnType<any>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
            width: 30
        },
        {
            title: 'Họ và tên',
            dataIndex: 'name',
            minWidth: 150
        },
        {
            title: 'Số điện thoại',
            dataIndex: 'phoneNumber',
            render: (_, entity) => (
                <div>
                    <div><PhoneOutlined /> {entity.phoneNumber}</div>
                    {
                        entity.email && (
                            <Tooltip title={entity.email}>
                                <div className="truncate w-40"><MailOutlined /> {entity.email}</div>
                            </Tooltip>
                        )
                    }
                </div>
            ),
            minWidth: 120
        },
        {
            title: 'DDCN',
            dataIndex: 'identityNumber',
            width: 120
        },
        {
            title: 'Chi nhánh',
            dataIndex: 'branchId',
            search: false,
            minWidth: 100,
            valueType: 'select',
            request: apiBranchOptions
        },
        {
            title: 'Trợ lý',
            dataIndex: 'salesName',
            search: false,
            minWidth: 120
        },
        {
            title: 'Nguồn',
            dataIndex: 'sourceId',
            valueType: 'select',
            request: apiSourceOptions
        },
        {
            title: 'Ngày tham gia',
            dataIndex: 'eventDate',
            valueType: 'date',
            search: false,
            width: 120,
            render: (_, entity) => (
                <div>
                    <div>{entity.eventDate ? dayjs(entity.eventDate).format('DD-MM-YYYY') : '-'}</div>
                </div>
            ),
            minWidth: 100
        },
        {
            title: 'Khung giờ',
            dataIndex: 'eventId',
            valueType: 'select',
            request: apiEventOptions,
            width: 100
        },
        {
            title: 'Trạng thái',
            dataIndex: 'status',
            search: false,
            valueEnum: {
                3: <Tag color="warning" className="w-20 text-center">Chốt deal</Tag>,
                4: <Tag color="error" className="w-20 text-center">Từ chối</Tag>,
                5: <Tag color="success" className="w-20 text-center">Hoàn thành</Tag>,
                6: <Tag color="processing" className="w-20 text-center">KT xác nhận</Tag>,
                7: <Tag color="processing" className="w-20 text-center">GĐ xác nhận</Tag>
            },
            width: 70
        },
        {
            title: 'GTHĐ',
            dataIndex: 'contractAmount',
            search: false,
            valueType: 'digit'
        },
        {
            title: 'Voucher',
            dataIndex: 'voucherId',
            search: false,
            request: apiVoucherOptions
        },
        {
            title: 'Đã TT',
            dataIndex: 'amountPaid',
            search: false,
            valueType: 'digit'
        },
        {
            title: 'Tỷ lệ',
            search: false,
            render: (_, entity) => entity.contractAmount ? (entity.amountPaid / entity.contractAmount * 100).toFixed(1) + '%' : '-',
            width: 50
        },
        {
            title: 'Tác vụ',
            valueType: 'option',
            hideInTable: access.sales,
            render: (dom, entity) => [
                <Popconfirm key="approe" title="Bạn có chắc chắn muốn phê duyệt?" onConfirm={async () => {
                    await apiUpdateLeadStatus({
                        id: entity.id,
                        status: LeadStatus.DosApproved
                    });
                    message.success('Phê duyệt thành công!');
                    actionRef.current?.reload();
                }}>
                    <Tooltip title="GĐ Phê duyệt">
                        <Button type="primary" icon={<CheckOutlined />} size="small" hidden={entity.status !== LeadStatus.LeadAccept || !access.dos}></Button>
                    </Tooltip>
                </Popconfirm>,
                <Tooltip key="card-holder" title="Tạo chủ thẻ">
                    <Button size="small" type="primary"
                        onClick={() => {
                            setLead(entity);
                            setOpenCardHolder(true);
                        }}
                        icon={<UserAddOutlined />} hidden={entity.status !== LeadStatus.AccountantApproved || !access.canCX} />
                </Tooltip>,
                <Popconfirm key="account" title="Bạn có chắc chắn muốn phê duyệt?" onConfirm={async () => {
                    await apiUpdateLeadStatus({
                        id: entity.id,
                        status: LeadStatus.AccountantApproved
                    });
                    message.success('Phê duyệt thành công!');
                    actionRef.current?.reload();
                }}>
                    <Tooltip title="KT Phê duyệt">
                        <Button type="primary" icon={<CheckOutlined />} size="small" hidden={entity.status !== LeadStatus.DosApproved || !access.canAccountant}></Button>
                    </Tooltip>
                </Popconfirm>,
                <Popconfirm key="delete" title="Bạn có chắc chắn muốn phê duyệt?" onConfirm={async () => {
                    await apiQueueCardHolderStatus({
                        id: entity.id,
                        status: LeadStatus.LeadReject
                    });
                    message.success('Từ chối thành công!');
                    actionRef.current?.reload();
                }}>
                    <Tooltip title="Từ chối">
                        <Button type="primary" danger icon={<DeleteOutlined />} size="small" hidden={entity.status !== 0 || !access.canCX}></Button>
                    </Tooltip>
                </Popconfirm>,
                <Dropdown key="more" menu={{
                    items: [
                        {
                            key: 'evidence',
                            label: 'Xem chứng từ',
                            icon: <EyeOutlined />,
                            onClick: () => {
                                setLead(entity);
                                setOpenEvidence(true);
                            }
                        },
                        {
                            key: 'sublead',
                            label: 'Xem người đi cùng',
                            icon: <UsergroupAddOutlined />,
                            onClick: () => {
                                setLead(entity);
                                setOpenSubLead(true);
                            }
                        }
                    ]
                }}>
                    <Button size="small" icon={<MoreOutlined />} />
                </Dropdown>
            ],
            width: 60
        }
    ]
    return (
        <PageContainer extra={<Button type="primary" icon={<FileExcelOutlined />} onClick={() => setOpenE(true)} hidden={!access.event}>Xuất dữ liệu</Button>}>
            <ProTable
                scroll={{
                    x: true
                }}
                actionRef={actionRef}
                search={{
                    layout: 'vertical'
                }}
                request={(params: any) => apiUsersInEvent({
                    ...params,
                    inQueue: true
                })}
                columns={columns}
            />

            <CardHolderForm user={lead} open={openCardHolder} setOpen={setOpenCardHolder} actionRef={actionRef} leadConvert />
            <SubLead lead={lead} open={openSubLead} onOpenChange={setOpenSubLead} reload={() => { }} />
            <ModalForm title="Xuất dữ liệu" open={openE} onOpenChange={setOpenE} width={400} onFinish={async (values) => {
                const response = await apiExportLead(values);
                const url = window.URL.createObjectURL(
                    new Blob([response]),
                );
                const link = document.createElement('a');
                link.href = url;
                link.setAttribute(
                    'download',
                    `data-nuras.xlsx`,
                );

                // Append to html link element page
                document.body.appendChild(link);

                // Start download
                link.click();

                // Clean up and remove the link
                link.parentNode?.removeChild(link);
            }}>
                <ProFormDatePicker label="Từ ngày" name="fromDate" width="lg" rules={[
                    {
                        required: true
                    }
                ]} />
                <ProFormDatePicker label="Đến ngày" name="toDate" width="lg" rules={[
                    {
                        required: true
                    }
                ]} />
            </ModalForm>
            <Evidence open={openEvidence} onClose={() => setOpenEvidence(false)} data={lead} />
        </PageContainer>
    )
}

export default CardHolderQueue;