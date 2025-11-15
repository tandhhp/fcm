import { apiDeleteLead } from "@/services/contact";
import { CheckOutlined, CloseOutlined, DeleteOutlined, EditOutlined, ExportOutlined, ManOutlined, MoreOutlined, ReloadOutlined, SettingOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { useAccess } from "@umijs/max";
import { Button, Dropdown, message, Popconfirm, Tag } from "antd";
import dayjs from "dayjs";
import { useRef, useState } from "react";
import { apiBranchOptions } from "@/services/settings/branch";
import { apiEventOptions } from "@/services/event";
import { apiLeadCheckinList, apiLeadExportCheckin } from "@/services/users/lead";
import { apiTableOptions } from "@/services/settings/table";
import { apiSourceOptions } from "@/services/settings/source";
import ReinviteHistories from "@/pages/event/components/reinvite";
import LeadRejectForm from "./components/reject-form";
import ReinviteForm from "./components/reinvite-form";
import LeadFeedbackUpdateForm from "./components/update-form";
import { apiAttendanceOptions } from "@/services/event/attendance";
import CloseDealForm from "@/components/form/close-deal";

const LeadPage: React.FC = () => {

    const actionRef = useRef<ActionType>();
    const [lead, setLead] = useState<any>();
    const access = useAccess();
    const [loadingExport, setLoadingExport] = useState<boolean>(false);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [openHistory, setOpenHistory] = useState<boolean>(false);
    const [openCloseDeal, setOpenCloseDeal] = useState<boolean>(false);
    const [openReject, setOpenReject] = useState<boolean>(false);
    const [openReinvite, setOpenReinvite] = useState<boolean>(false);
    const [filterOptions, setFilterOptions] = useState<any>({});

    const onExport = async () => {
        setLoadingExport(true);
        const response = await apiLeadExportCheckin({
            ...filterOptions,
            fromDate: filterOptions?.dateRange ? filterOptions.dateRange[0] : undefined,
            toDate: filterOptions?.dateRange ? filterOptions.dateRange[1] : undefined,
            dateRange: undefined
        });
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', `Danh_sach_checkin_${dayjs().format('YYYYMMDD_HHmmss')}.xlsx`);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
        setLoadingExport(false);
    }

    return (
        <PageContainer extra={<Button type="primary" icon={<ExportOutlined />} onClick={onExport}
            disabled={access.sales || access.telesale || access.telesaleManager || access.sm}
            loading={loadingExport}>Xuất dữ liệu</Button>}>
            <ProTable
                actionRef={actionRef}
                search={{
                    layout: 'vertical',
                    defaultCollapsed: false
                }}
                scroll={{
                    x: true
                }}
                request={(params) => {
                    setFilterOptions(params);
                    return apiLeadCheckinList(params);
                }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Bàn',
                        dataIndex: 'tableId',
                        search: false,
                        valueType: 'select',
                        request: apiTableOptions,
                        width: 60,
                        minWidth: 60
                    },
                    {
                        title: 'Nguồn',
                        dataIndex: 'sourceId',
                        valueType: 'select',
                        search: false,
                        request: apiSourceOptions,
                        width: 120,
                        minWidth: 120
                    },
                    {
                        title: 'Trạng thái tham dự',
                        dataIndex: 'attendanceId',
                        valueType: 'select',
                        search: false,
                        request: apiAttendanceOptions,
                        width: 150,
                        minWidth: 150
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'name',
                        render: (dom, entity) => (
                            <div>{entity.gender === false && (<ManOutlined className='text-blue-500' />)}{entity.gender === true && (<WomanOutlined className='text-red-500' />)} {dom}</div>
                        ),
                        minWidth: 180,
                        width: 180
                    },
                    {
                        title: 'Điện thoại',
                        dataIndex: 'phoneNumber',
                        width: 100,
                        minWidth: 100
                    },
                    {
                        title: 'Năm sinh',
                        dataIndex: 'dateOfBirth',
                        valueType: 'dateYear',
                        search: false,
                        width: 90,
                        minWidth: 90
                    },
                    {
                        title: 'Số CCCD',
                        dataIndex: 'identityNumber',
                        search: false,
                        width: 100,
                        minWidth: 100
                    },
                    {
                        title: 'Khách phụ',
                        dataIndex: 'subLeads',
                        valueType: 'select',
                        search: false,
                        width: 200,
                        minWidth: 200,
                    },
                    {
                        title: 'Team Key-In',
                        dataIndex: 'teamKeyIn',
                        search: false,
                        minWidth: 160,
                        width: 160
                    },
                    {
                        title: 'Key-In',
                        dataIndex: 'creatorName',
                        search: false,
                        minWidth: 160,
                        width: 160
                    },
                    {
                        title: 'Rep',
                        dataIndex: 'salesName',
                        search: false,
                        minWidth: 160
                    },
                    {
                        title: 'Người T.O',
                        dataIndex: 'toName',
                        search: false,
                        minWidth: 160,
                        width: 160
                    },
                    {
                        title: 'Ngày sự kiện',
                        dataIndex: 'eventDate',
                        valueType: 'date',
                        width: 110,
                        render: (_, entity) => entity.eventDate ? dayjs(entity.eventDate).format('DD-MM-YYYY') : '-',
                        minWidth: 120
                    },
                    {
                        title: 'Khung giờ',
                        dataIndex: 'eventId',
                        width: 100,
                        valueType: 'select',
                        request: apiEventOptions,
                        minWidth: 100
                    },
                    {
                        title: 'Chi nhánh',
                        dataIndex: 'branchId',
                        minWidth: 100,
                        width: 100,
                        valueType: 'select',
                        request: apiBranchOptions
                    },
                    {
                        title: 'Khoảng thời gian',
                        dataIndex: 'dateRange',
                        hideInTable: true,
                        valueType: 'dateRange'
                    },
                    {
                        title: 'Sales Manager',
                        dataIndex: 'salesManagerName',
                        search: false,
                        minWidth: 160,
                        width: 160
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'status',
                        valueEnum: {
                            2: <Tag color="red" className="w-20 text-center">Check-In</Tag>,
                            3: <Tag color="blue" className="w-20 text-center">Chốt deal</Tag>,
                            4: <Tag color="black" className="w-20 text-center">Từ chối</Tag>,
                            5: <Tag color="tomato">Mời lại</Tag>
                        },
                        width: 100,
                        minWidth: 100
                    },
                    {
                        title: 'Lượt',
                        dataIndex: 'inviteCount',
                        search: false,
                        render: (_, entity) => {
                            return <Button size="small"
                                onClick={() => {
                                    setLead(entity);
                                    setOpenHistory(true);
                                }}
                                type="dashed" disabled={entity.inviteCount === 1}>{entity.inviteCount}</Button>
                        },
                        width: 60
                    },
                    {
                        title: 'Voucher 1',
                        dataIndex: 'voucher1',
                        search: false,
                        minWidth: 100,
                    },
                    {
                        title: 'Voucher 2',
                        dataIndex: 'voucher2',
                        search: false,
                        minWidth: 100,
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note',
                        search: false,
                        minWidth: 200
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        render: (_, entity) => [
                            <Popconfirm title="Xác nhận xóa?" key="delete" onConfirm={async () => {
                                await apiDeleteLead(entity.id);
                                message.success('Thành công!');
                                actionRef.current?.reload();
                            }}>
                                <Button type="primary" danger size="small" icon={<DeleteOutlined />} hidden={!access.canAdmin} />
                            </Popconfirm>,
                            <Dropdown key="event" menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        icon: <EditOutlined />,
                                        onClick: () => {
                                            setLead(entity);
                                            setOpenForm(true);
                                        },
                                        disabled: access.cx || access.sales
                                    },
                                    {
                                        key: 'close-deal',
                                        label: 'Chốt deal',
                                        icon: <CheckOutlined />,
                                        onClick: () => {
                                            setLead(entity);
                                            setOpenCloseDeal(true);
                                        },
                                        disabled: access.cx || access.sales
                                    },
                                    {
                                        key: 'reject',
                                        label: 'Từ chối',
                                        icon: <CloseOutlined />,
                                        onClick: () => {
                                            setLead(entity);
                                            setOpenReject(true);
                                        },
                                        disabled: true
                                    },
                                    {
                                        key: 'reinvite',
                                        label: 'Mời lại',
                                        icon: <ReloadOutlined />,
                                        onClick: () => {
                                            setLead(entity);
                                            setOpenReinvite(true);
                                        },
                                        disabled: true
                                    }
                                ]
                            }}>
                                <Button icon={<MoreOutlined />} size="small" type="dashed"></Button>
                            </Dropdown>
                        ],
                        width: 50,
                        align: 'center'
                    }
                ]}
            />
            <LeadFeedbackUpdateForm open={openForm} onOpenChange={setOpenForm} reload={() => actionRef.current?.reload()} data={lead} />
            <ReinviteHistories leadId={lead?.id} open={openHistory} onClose={() => setOpenHistory(false)} />
            <CloseDealForm open={openCloseDeal} onOpenChange={setOpenCloseDeal} reload={() => actionRef.current?.reload()} data={lead} />
            <LeadRejectForm open={openReject} onOpenChange={setOpenReject} data={lead} reload={() => actionRef.current?.reload()} />
            <ReinviteForm open={openReinvite} onOpenChange={setOpenReinvite} data={lead} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default LeadPage;