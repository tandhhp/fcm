import { apiUpdateLeadStatus, apiUsersInEvent } from "@/services/contact";
import { LeadStatus } from "@/utils/constants";
import { CheckOutlined, CommentOutlined, CopyOutlined, EditOutlined, LeftOutlined, ManOutlined, MoreOutlined, PlusOutlined, SettingOutlined, UsergroupAddOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProForm, ProFormDatePicker, ProFormSelect, ProTable } from "@ant-design/pro-components"
import { Button, Dropdown, message, Popconfirm, Space, Tag } from "antd";
import { useRef, useState } from "react";
import LeadFeedback from "./components/feedback";
import dayjs from "dayjs";
import { history, useAccess, useParams, useRequest } from "@umijs/max";
import SubLead from "./components/sublead";
import TableComponent from "./components/table";
import ReinviteHistories from "./components/reinvite";
import BackToCheckinModal from "./components/back-to-checkin";
import { apiEventDetail, apiEventOptions } from "@/services/event";
import LeadCheckin from "./components/checkin";
import LeadForm from "@/components/form/lead-form";

const EventUserPage: React.FC = () => {

    const actionRef = useRef<ActionType>();
    const [open, setOpen] = useState<boolean>(false);
    const { id } = useParams<{ id: string }>();
    const [openFB, setOpenFB] = useState<boolean>(false);
    const [lead, setLead] = useState<any>();
    const [eventDate, setEventDate] = useState<string>(dayjs().format('YYYY-MM-DD'));
    const access = useAccess();
    const [subLeadOpen, setSubLeadOpen] = useState<boolean>(false);
    const [openHistory, setOpenHistory] = useState<boolean>(false);
    const [openBTC, setOpenBTC] = useState<boolean>(false);
    const { data } = useRequest(() => apiEventDetail(id));
    const [openCheckin, setOpenCheckin] = useState<boolean>(false);

    const onApprove = async (id: string) => {
        await apiUpdateLeadStatus({
            id: id,
            status: LeadStatus.Approved
        });
        message.success('Thành công');
        actionRef.current?.reload();
    }

    return (
        <PageContainer title={`Khung giờ: ${data?.name}`}
            extra={(
                <Space size="small">
                    <ProForm submitter={false} layout="inline">
                        <ProFormDatePicker initialValue={dayjs()}
                            name="eventDate"
                            fieldProps={{
                                format: 'DD-MM-YYYY',
                                onChange: (date: any) => {
                                    setEventDate(date.format('YYYY-MM-DD'));
                                    actionRef.current?.reload();
                                }
                            }}
                            formItemProps={{
                                className: 'mb-0'
                            }}
                        />
                        <ProFormSelect name="eventId"
                            request={apiEventOptions}
                            initialValue={id}
                            fieldProps={{
                                onChange: (value: string) => {
                                    history.push(`/event/time-slot/center/${value}`);
                                    window.location.reload();
                                }
                            }}
                            formItemProps={{
                                className: 'mb-0'
                            }}
                        />
                    </ProForm>
                    <Button icon={<LeftOutlined />} onClick={() => history.push('/event/time-slot')}>Quay lại</Button>
                </Space>
            )}>
            <div className="overflow-auto">
                <ProTable
                    headerTitle={(
                        <Space>
                            <Button icon={<PlusOutlined />} type="primary" onClick={() => {
                                setLead(null);
                                setOpen(true);
                            }} hidden={!access.event && !access.telesale && !access.sales}>Tạo khách hàng</Button>
                            <TableComponent eventDate={eventDate} eventId={id} />
                        </Space>
                    )}
                    actionRef={actionRef}
                    request={apiUsersInEvent}
                    params={{ eventId: id, eventDate: eventDate }}
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
                            width: 20
                        },
                        {
                            title: 'Họ và tên',
                            dataIndex: 'name',
                            render: (dom, entity) => {
                                if (entity.gender === true) {
                                    return <div><WomanOutlined className="text-pink-500 mr-2" />{dom}</div>
                                }
                                if (entity.gender === false) {
                                    return <div><ManOutlined className="text-blue-500 mr-2" />{dom}</div>
                                }
                                return <div>{dom}</div>;
                            },
                            minWidth: 250,
                            width: 250
                        },
                        {
                            title: 'Năm sinh',
                            dataIndex: 'dateOfBirth',
                            width: 100,
                            valueType: 'dateYear',
                            search: false,
                            minWidth: 100
                        },
                        {
                            title: 'Key-In',
                            dataIndex: 'createdByName',
                            search: false,
                            minWidth: 150,
                            width: 150
                        },
                        {
                            title: 'SĐT',
                            dataIndex: 'phoneNumber',
                            width: 80
                        },
                        {
                            title: 'Trạng thái',
                            dataIndex: 'status',
                            valueEnum: {
                                0: <Tag color="warning" className="w-full text-center">Check-In</Tag>,
                                1: <Tag color="success" className="w-full text-center">Đã duyệt</Tag>
                            },
                            width: 80,
                            minWidth: 80
                        },
                        {
                            title: 'Nguồn',
                            dataIndex: 'sourceName',
                            minWidth: 100,
                            width: 100,
                            search: false
                        },
                        {
                            title: 'Ghi chú',
                            dataIndex: 'note',
                            search: false,
                            minWidth: 200
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
                                    type="primary" disabled={entity.inviteCount === 1}>{entity.inviteCount}</Button>
                            },
                            width: 50
                        },
                        {
                            title: <SettingOutlined />,
                            valueType: 'option',
                            render: (_, entity) => [
                                <Popconfirm key={"approve"} title="Duyệt khách hàng này?" onConfirm={() => onApprove(entity.id)}>
                                    <Button type="primary" size="small" icon={<CheckOutlined />} hidden={!access.canSm && !access.event} disabled={entity.status === LeadStatus.Approved} />
                                </Popconfirm>,
                                <Dropdown key="more" menu={{
                                    items: [
                                        {
                                            key: 'copy',
                                            label: 'Sao chép',
                                            icon: <CopyOutlined />,
                                            onClick: () => {
                                                const text = `Họ tên: ${entity.name}.\nTelesale: ${entity.teleName || ''}.\nNguồn: ${entity.source || ''}.\nTrạng thái: ${entity.tableStatus || ''}.\nBàn: ${entity.table || ''}.`
                                                navigator.clipboard.writeText(text);
                                                message.success('Sao chép thông tin thành công!');
                                            }
                                        },
                                        {
                                            key: 'sublead',
                                            label: 'Đi cùng',
                                            icon: <UsergroupAddOutlined />,
                                            onClick: () => {
                                                setLead(entity);
                                                setSubLeadOpen(true);
                                            }
                                        },
                                        {
                                            key: 'edit',
                                            label: 'Chỉnh sửa',
                                            icon: <EditOutlined />,
                                            onClick: () => {
                                                setLead(entity);
                                                setOpen(true);
                                            }
                                        },
                                        {
                                            key: 'checkin',
                                            label: 'Checkin',
                                            icon: <CheckOutlined />,
                                            onClick: () => {
                                                setLead(entity);
                                                setOpenCheckin(true);
                                            },
                                            disabled: entity.status !== LeadStatus.Approved || !access.event
                                        },
                                        {
                                            key: 'feedback',
                                            label: 'Phản hồi',
                                            icon: <CommentOutlined />,
                                            onClick: () => {
                                                setLead(entity);
                                                setOpenFB(true);
                                            }
                                        }
                                    ]
                                }}>
                                    <Button type="dashed" size="small" icon={<MoreOutlined />} />
                                </Dropdown>
                            ],
                            width: 60,
                            align: 'center'
                        }
                    ]}
                />
            </div>

            <LeadForm open={open} onOpenChange={setOpen} data={lead} reload={() => {
                actionRef.current?.reload();
            }} />
            <LeadFeedback open={openFB} onOpenChange={setOpenFB}
                eventId={id}
                id={lead?.id} eventDate={eventDate} reload={() => {
                    actionRef.current?.reload();
                }} />
            <SubLead lead={lead} open={subLeadOpen} onOpenChange={setSubLeadOpen} reload={() => {
                actionRef.current?.reload();
            }} />
            <ReinviteHistories open={openHistory} onClose={() => setOpenHistory(false)} leadId={lead?.id} />
            <BackToCheckinModal open={openBTC} onOpenChange={setOpenBTC} id={lead?.id} reload={() => {
                actionRef.current?.reload();
                setOpenBTC(false);
            }} />
            <LeadCheckin open={openCheckin} onOpenChange={setOpenCheckin} data={lead} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default EventUserPage;