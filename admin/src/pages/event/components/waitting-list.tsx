import { apiEventOptions } from "@/services/event";
import { apiLeadWaitingList } from "@/services/users/lead";
import { CheckOutlined, DeleteOutlined, EditOutlined, FileSearchOutlined, HistoryOutlined, KeyOutlined, ManOutlined, MoreOutlined, PlusOutlined, SettingOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, ProTable } from "@ant-design/pro-components";
import { useRef, useState } from "react";
import { Avatar, Button, Dropdown, message, Popconfirm, Tag } from "antd";
import { Link, useAccess } from "@umijs/max";
import { apiDeleteLead, apiUpdateLeadStatus } from "@/services/contact";
import { CONFIRM2_OPTIONS, LeadStatus } from "@/utils/constants";
import LeadForm from "@/components/form/lead-form";
import LeadStatusRender from "@/components/lead/status-render";
import { apiBranchOptions } from "@/services/settings/branch";
import ReinviteHistories from "./reinvite";
import LeadDetails from "./lead-details";
import { apiSourceOptions } from "@/services/settings/source";

const WaitingList: React.FC = () => {

    const access = useAccess();
    const [keyInOpen, setKeyInOpen] = useState<boolean>(false);
    const [historyOpen, setHistoryOpen] = useState<boolean>(false);
    const [selectedRecord, setSelectedRecord] = useState<any>(null);
    const [detailOpen, setDetailOpen] = useState<boolean>(false);
    const actionRef = useRef<ActionType>();

    const onApprove = async (id: string) => {
        await apiUpdateLeadStatus({
            id: id,
            status: LeadStatus.Approved
        });
        message.success('Thành công');
        actionRef.current?.reload();
    }

    const canDelete = (status: LeadStatus) => {
        if (status !== LeadStatus.Approved) return false;
        if (!access.canAdmin && !access.event && !access.em) return false;
        return true;
    }

    const getConfirm2StatusTag = (status: number) => {
        const option = CONFIRM2_OPTIONS.find(o => o.value === status);
        const colors = ['default', 'success', 'error', 'warning', 'default'];
        return <Tag className="w-full text-center" color={colors[status] || 'default'}>{option?.label || 'Chưa xác nhận'}</Tag>;
    };

    const canEdit = (record: any) => {
        if (record.status === LeadStatus.Approved && access.sales) return false;
        if (access.dot || access.telesaleManager || access.telesale) return false;
        if (access.adminData) return false;
        if (access.legalExcutive) return false;
        return true;
    }

    return (
        <>
            <ProTable
                rowKey={"id"}
                search={{
                    layout: 'vertical'
                }}
                scroll={{
                    x: true
                }}
                actionRef={actionRef}
                headerTitle={<Button type="primary" onClick={() => {
                    setSelectedRecord(null);
                    setKeyInOpen(true);
                }} icon={<PlusOutlined />} disabled={access.canAdmin || access.telesale || access.telesaleManager || access.dot || access.cx}>Tạo Key-In</Button>}
                request={apiLeadWaitingList}
                size="small"
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Chi nhánh',
                        dataIndex: 'branchId',
                        valueType: 'select',
                        request: apiBranchOptions,
                        width: 100,
                        minWidth: 100
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'name',
                        render: (dom, entity) => {
                            if (entity.gender === true) {
                                return <><WomanOutlined className="text-red-500 mr-1" />{dom}</>
                            }
                            if (entity.gender === false) {
                                return <><ManOutlined className="text-blue-500 mr-1" />{dom}</>
                            }
                            return <><KeyOutlined className="text-gray-500 mr-1" />{dom}</>
                        },
                        width: 180,
                        minWidth: 180
                    },
                    {
                        title: 'SDT',
                        dataIndex: 'phoneNumber',
                        width: 100
                    },
                    {
                        title: 'Khách phụ',
                        dataIndex: 'subLeads',
                        valueType: 'select',
                        search: false
                    },
                    {
                        title: 'Rep',
                        dataIndex: 'salesName',
                        search: false
                    },
                    {
                        title: 'Key-In',
                        dataIndex: 'creatorName',
                        search: false,
                        render: (dom, record) => (
                            <div className="flex gap-1 items-center">
                                <Avatar size="small" src={record.avatar} />
                                <Link to={`/user/account/${record.createdBy}`}>
                                    {dom}
                                </Link>
                            </div>
                        ),
                        width: 200,
                        minWidth: 200
                    },
                    {
                        title: 'Sự kiện',
                        dataIndex: 'eventDate',
                        valueType: 'date',
                        width: 100,
                        minWidth: 100
                    },
                    {
                        title: 'Giờ',
                        dataIndex: 'eventName',
                        search: false,
                        width: 50,
                        minWidth: 50,
                        render: (dom, record) => (
                            <Tag color={record.eventColor} className="w-full text-center">
                                {dom}
                            </Tag>
                        )
                    },
                    {
                        title: 'Nguồn',
                        dataIndex: 'sourceName',
                        search: false
                    },
                    {
                        dataIndex: 'eventId',
                        valueType: 'select',
                        request: apiEventOptions,
                        hideInTable: true,
                        title: 'Khung giờ'
                    },
                    {
                        title: 'Nguồn',
                        dataIndex: 'sourceId',
                        valueType: 'select',
                        hideInTable: true,
                        request: apiSourceOptions,
                        fieldProps: {
                            showSearch: true
                        }
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'status',
                        valueEnum: {
                            0: { text: 'Chờ duyệt', status: 'Default' },
                            1: { text: 'Đã duyệt', status: 'Success' },
                            5: { text: 'Mời lại', status: 'Warning' }
                        },
                        search: false,
                        width: 100,
                        minWidth: 100,
                        render: (_, record) => <LeadStatusRender status={record.status} />
                    },
                    {
                        title: 'Ngày tạo',
                        dataIndex: 'createdDate',
                        valueType: 'date',
                        search: false,
                        width: 100,
                        minWidth: 100
                    },
                    {
                        title: 'Xác nhận 2',
                        dataIndex: 'confirm2',
                        valueType: 'select',
                        search: false,
                        width: 120,
                        minWidth: 120,
                        fieldProps: {
                            options: CONFIRM2_OPTIONS
                        },
                        render: (text, record) => {
                            return (
                                <div>
                                    {getConfirm2StatusTag(record.confirm2)}
                                </div>
                            )
                        }
                    },
                    {
                        title: 'Lượt',
                        dataIndex: 'inviteCount',
                        search: false,
                        width: 40,
                        minWidth: 40,
                        valueType: 'digit',
                        render: (dom, entity) => (
                            <Button type="dashed" size="small" onClick={() => {
                                setSelectedRecord(entity);
                                setHistoryOpen(true);
                            }}>{dom}</Button>
                        )
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 60,
                        align: 'center',
                        render: (dom, entity) => [
                            <Popconfirm key={"approve"} title="Duyệt khách hàng này?" onConfirm={() => onApprove(entity.id)}>
                                <Button type="primary" size="small" icon={<CheckOutlined />} hidden={!access.canSm && !access.event && !access.em} disabled={entity.status === LeadStatus.Approved} />
                            </Popconfirm>,
                            <Dropdown key={"more"} menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        icon: <EditOutlined />,
                                        onClick: () => {
                                            setSelectedRecord(entity);
                                            setKeyInOpen(true);
                                        },
                                        disabled: !canEdit(entity)
                                    },
                                    {
                                        key: 'history',
                                        label: 'Lịch sử mời lại',
                                        icon: <HistoryOutlined />,
                                        onClick: () => {
                                            setSelectedRecord(entity);
                                            setHistoryOpen(true);
                                        }
                                    },
                                    {
                                        key: 'detail',
                                        label: 'Thông tin chi tiết',
                                        icon: <FileSearchOutlined />,
                                        onClick: () => {
                                            setSelectedRecord(entity);
                                            setDetailOpen(true);
                                        }
                                    }
                                ]
                            }}>
                                <Button size="small" icon={<MoreOutlined />} />
                            </Dropdown>,
                            <Popconfirm key={"delete"} title="Xóa khách hàng này?" onConfirm={async () => {
                                await apiDeleteLead(entity.id);
                                message.success('Đã xóa');
                                actionRef.current?.reload();
                            }}>
                                <Button type="primary" danger size="small" icon={<DeleteOutlined />} disabled={!canDelete(entity.status)} />
                            </Popconfirm>
                        ]
                    }
                ]}
            />
            <LeadDetails open={detailOpen} onOpenChange={setDetailOpen} lead={selectedRecord} />
            <LeadForm open={keyInOpen} onOpenChange={setKeyInOpen} reload={() => actionRef.current?.reload()} data={selectedRecord} />
            <ReinviteHistories open={historyOpen} onClose={() => setHistoryOpen(false)} leadId={selectedRecord?.id} />
        </>
    )
}

export default WaitingList;