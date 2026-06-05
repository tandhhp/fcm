import { deleteContact, listContact } from "@/services/contact";
import { CalendarOutlined, DeleteOutlined, EditOutlined, EyeOutlined, ManOutlined, MoreOutlined, PhoneOutlined, PlusOutlined, StopOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProColumnType, ProTable } from "@ant-design/pro-components"
import { history, Link, useAccess } from "@umijs/max";
import { Avatar, Button, Dropdown, Popconfirm, message } from "antd";
import { useRef, useState } from "react";
import BlockContactModal from "./components/block-modal";
import ContactForm from "./components/form";
import CallForm from "./components/call";
import BookingForm from "./components/booking";
import { apiSourceOptions } from "@/services/settings/source";
import AppointmentDrawer from "./components/appointment-drawer";
import LeadStatusRender from "@/components/lead/status-render";
import { apiBranchOptions } from "@/services/settings/branch";

const ContactPage: React.FC = () => {

    const access = useAccess();
    const actionRef = useRef<ActionType>();
    const [contact, setContact] = useState<any>();
    const [openBlock, setOpenBlock] = useState<boolean>(false);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [openCall, setOpenCall] = useState<boolean>(false);
    const [openBooking, setOpenBooking] = useState<boolean>(false);
    const [openAppointmentDrawer, setOpenAppointmentDrawer] = useState<boolean>(false);
    const [appointmentPhone, setAppointmentPhone] = useState<string>();

    const columns: ProColumnType<any>[] = [
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
            width: 100
        },
        {
            title: 'Họ và tên',
            dataIndex: 'name',
            render: (text, record) => (
                <>
                    <div className="font-semibold">{text}</div>
                    <div className="text-xs text-gray-500">HT2: {record.name2 || 'N/A'}</div>
                    <div className="text-xs text-gray-500">SDT2: {record.phoneNumber2 || 'N/A'}</div>
                </>
            ),
            minWidth: 150,
            width: 150
        },
        {
            title: 'Số điện thoại',
            dataIndex: 'phoneNumber',
            width: 100,
            render: (text, record) => {
                return (
                    <Button type="primary" size="small" onClick={() => {
                        setContact(record);
                        setOpenCall(true);
                    }}>{text}</Button>
                )
            }
        },
        {
            title: 'Trạng thái gọi',
            dataIndex: 'isCalled',
            valueType: 'select',
            valueEnum: {
                true: { text: 'Đã gọi' },
                false: { text: 'Chưa gọi' }
            },
            width: 100,
            minWidth: 100,
            hideInTable: true
        },
        {
            title: 'Nguồn',
            dataIndex: 'sourceId',
            hideInTable: true,
            valueType: 'select',
            request: apiSourceOptions,
            fieldProps: {
                showSearch: true
            }
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
            title: 'Nhân viên',
            dataIndex: 'telesalesName',
            search: false,
            minWidth: 180,
            width: 180,
            render: (text, record) => (
                <div>
                    <div className="mb-1">
                        <Avatar size="small" src={record.telesalesAvatar} />
                        <Link to={`/user/account/${record.telesalesId}`} className="ml-1">
                            {text}
                        </Link>
                    </div>
                    <div className="text-xs text-gray-500">QL: {record.managerName}</div>
                </div>
            )
        },
        {
            title: 'Source',
            dataIndex: 'sourceType',
            valueType: 'select',
            valueEnum: {
                1: 'Cold Data',
                2: 'Company',
                3: 'Private',
                4: 'Reference'
            },
            minWidth: 100,
            width: 100,
            render: (text, record) => (
                <div>
                    <div className="mb-1">{record.sourceName}</div>
                    <div className="text-xs text-gray-500">{text}</div>
                </div>
            )
        },
        {
            title: 'Type of Data',
            dataIndex: 'typeOfDataName',
            search: false,
            width: 140,
            minWidth: 140
        },
        {
            title: 'Sự kiện',
            dataIndex: 'leadStatus',
            width: 100,
            search: false,
            minWidth: 80,
            render: (_, record) => <LeadStatusRender status={record.leadStatus} />
        },
        {
            dataIndex: 'lastCallTime',
            title: 'Lần gọi cuối',
            valueType: 'dateTime',
            search: false,
            width: 150,
            minWidth: 150
        },
        {
            title: 'Ghi chú',
            dataIndex: 'note',
            search: false,
            minWidth: 200
        },
        {
            title: 'Tác vụ',
            valueType: 'option',
            render: (dom, entity) => [
                <Dropdown key="more" menu={{
                    items: [
                        {
                            key: 'view',
                            label: 'Chi tiết',
                            onClick: () => {
                                history.push(`/contact/center/${entity.id}`);
                            },
                            icon: <EyeOutlined />
                        },
                        {
                            key: 'edit',
                            label: 'Chỉnh sửa',
                            icon: <EditOutlined />,
                            onClick: () => {
                                setContact(entity);
                                setOpenForm(true);
                            }
                        },
                        {
                            key: 'call',
                            label: 'Cuộc gọi',
                            onClick: () => {
                                setContact(entity);
                                setOpenCall(true);
                            },
                            icon: <PhoneOutlined />
                        },
                        {
                            key: 'block',
                            label: 'Chặn liên hệ',
                            onClick: () => {
                                setContact(entity);
                                setOpenBlock(true);
                            },
                            icon: <StopOutlined />,
                            disabled: entity.isBlocked
                        }
                    ]
                }}>
                    <Button size="small" type="dashed" icon={<MoreOutlined />} />
                </Dropdown>,
                <Popconfirm key="delete" title="Bạn có chắc chắn muốn xóa?" onConfirm={async () => {
                    await deleteContact(entity.id);
                    message.success('Xóa thành công!');
                    actionRef.current?.reload();
                }}>
                    <Button type="primary" danger icon={<DeleteOutlined />} size="small" disabled={!access.telesale && !access.telesaleManager && !access.dot && !access.canAdmin}></Button>
                </Popconfirm>
            ],
            width: 80
        }
    ]

    return (
        <PageContainer extra={<Button type="primary" icon={<PlusOutlined />} onClick={() => {
            setContact(undefined);
            setOpenForm(true);
        }} disabled={!access.telesale && !access.telesaleManager && !access.dot}>
            Tạo mới
        </Button>}>
            <ProTable
                scroll={{ x: true }}
                actionRef={actionRef}
                search={{
                    layout: 'vertical'
                }}
                request={listContact}
                columns={columns}
                size="small"
            />
            <BlockContactModal open={openBlock} contact={contact} reload={() => actionRef.current?.reload()} onOpenChange={setOpenBlock} />
            <ContactForm open={openForm} onOpenChange={setOpenForm} data={contact} reload={() => actionRef.current?.reload()} />
            <CallForm open={openCall} data={contact} onOpenChange={setOpenCall} reload={() => actionRef.current?.reload()} />
            <BookingForm open={openBooking} data={contact} onOpenChange={setOpenBooking} reload={() => actionRef.current?.reload()} />
            <AppointmentDrawer open={openAppointmentDrawer} phoneNumber={appointmentPhone} onOpenChange={setOpenAppointmentDrawer} />
        </PageContainer>
    )
}

export default ContactPage;