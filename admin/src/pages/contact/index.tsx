import { deleteContact, listContact } from "@/services/contact";
import { CalendarOutlined, DeleteOutlined, EditOutlined, EyeOutlined, ManOutlined, MoreOutlined, PhoneOutlined, PlusOutlined, StopOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProColumnType, ProTable } from "@ant-design/pro-components"
import { history } from "@umijs/max";
import { Button, Dropdown, Popconfirm, message } from "antd";
import { useRef, useState } from "react";
import BlockContactModal from "./components/block-modal";
import ContactForm from "./components/form";
import CallForm from "./components/call";
import BookingForm from "./components/booking";
import dayjs from "dayjs";

const ContactPage: React.FC = () => {

    const actionRef = useRef<ActionType>();
    const [contact, setContact] = useState<any>();
    const [openBlock, setOpenBlock] = useState<boolean>(false);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [openCall, setOpenCall] = useState<boolean>(false);
    const [openBooking, setOpenBooking] = useState<boolean>(false);

    const columns: ProColumnType<any>[] = [
        {
            title: 'STT',
            valueType: 'indexBorder',
            width: 50
        },
        {
            title: 'Họ và tên',
            dataIndex: 'name',
            render: (text, record) => {
                if (record.gender === true) {
                    return <><WomanOutlined className="text-pink-500" /> {text}</>
                }
                if (record.gender === false) {
                    return <><ManOutlined className="text-blue-500" /> {text}</>
                }
                return text;
            }
        },
        {
            title: 'Số điện thoại',
            dataIndex: 'phoneNumber',
            width: 110,
            render: (text, record) => {
                return (
                    <Button type="link" size="small" icon={<PhoneOutlined />} onClick={() => {
                        setContact(record);
                        setOpenCall(true);
                    }}>{text}</Button>
                )
            }
        },
        {
            title: 'Email',
            dataIndex: 'email'
        },
        {
            title: 'Ngày tạo',
            dataIndex: 'createdDate',
            valueType: 'date',
            search: false,
            width: 100
        },
        {
            title: 'Phụ trách',
            dataIndex: 'telesalesName',
            search: false
        },
        {
            title: 'Lượt gọi',
            dataIndex: 'callCount',
            search: false
        },
        {
            title: 'Sự kiện',
            dataIndex: 'isBooked',
            valueType: 'select',
            valueEnum: {
                true: { text: 'Đã đặt lịch', status: 'Success' },
                false: { text: 'Chưa đặt lịch', status: 'Default' }
            },
            width: 120
        },
        {
            title: 'Lần gọi cuối',
            dataIndex: 'lastCall',
            hideInTable: true,
            valueType: 'dateRange'
        },
        {
            title: 'Nguồn',
            dataIndex: 'sourceName',
            search: false
        },
        {
            dataIndex: 'lastCall',
            title: 'Lần gọi cuối',
            valueType: 'dateTime',
            search: false,
            render: (dom, record) => {
                if (dayjs(record.lastCall).isAfter('2000-01-01')) {
                    return dom;
                }
                return 'Chưa gọi';
            }
        },
        {
            title: 'Ghi chú',
            dataIndex: 'note',
            search: false
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
                            key: 'booking',
                            label: 'Đặt lịch hẹn',
                            onClick: () => {
                                setContact(entity);
                                setOpenBooking(true);
                            },
                            icon: <CalendarOutlined />,
                            disabled: entity.isBooked
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
                    <Button type="primary" danger icon={<DeleteOutlined />} size="small"></Button>
                </Popconfirm>
            ],
            width: 60
        }
    ]

    return (
        <PageContainer extra={<Button type="primary" icon={<PlusOutlined />} onClick={() => setOpenForm(true)}>Tạo mới</Button>}>
            <ProTable
                scroll={{
                    x: true
                }}
                actionRef={actionRef}
                search={{
                    layout: 'vertical'
                }}
                request={listContact}
                columns={columns}
            />
            <BlockContactModal open={openBlock} contact={contact} reload={() => actionRef.current?.reload()} onOpenChange={setOpenBlock} />
            <ContactForm open={openForm} onOpenChange={setOpenForm} data={contact} reload={() => actionRef.current?.reload()} />
            <CallForm open={openCall} data={contact} onOpenChange={setOpenCall} reload={() => actionRef.current?.reload()} />
            <BookingForm open={openBooking} data={contact} onOpenChange={setOpenBooking} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default ContactPage;