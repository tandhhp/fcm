import { apiContactConfirm2, apiAttendanceSchedules, apiUpdateAttendanceSchedule } from "@/services/contact";
import { CONFIRM2_OPTIONS } from "@/utils/constants";
import { EditOutlined, ManOutlined, MoreOutlined, SettingOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProColumnType, ProFormDatePicker, ProFormSelect, ProFormText, ProFormTextArea, ProTable } from "@ant-design/pro-components"
import { useAccess } from "@umijs/max";
import { Avatar, Button, Dropdown, message, Tag } from "antd";
import { useRef, useState } from "react";
import { apiEventOptions } from "@/services/event";

const Index: React.FC = () => {

    const access = useAccess();
    const actionRef = useRef<ActionType>();
    const [selectedRecord, setSelectedRecord] = useState<any>(null);
    const [modalVisible, setModalVisible] = useState(false);
    const [editModalVisible, setEditModalVisible] = useState(false);

    const getConfirm2StatusTag = (status: number) => {
        const option = CONFIRM2_OPTIONS.find(o => o.value === status);
        const colors = ['default', 'success', 'error', 'warning', 'default'];
        return <Tag color={colors[status] || 'default'}>{option?.label || 'Chưa xác nhận'}</Tag>;
    };

    const columns: ProColumnType<any>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
            width: 30
        },
        {
            title: <SettingOutlined />,
            dataIndex: 'actions',
            valueType: 'option',
            render: (_, record) => [
                <Dropdown key="more"
                menu={{
                    items: [
                        {
                            key: 'edit',
                            label: 'Chỉnh sửa lịch hẹn',
                            icon: <EditOutlined />,
                            disabled: record.eventDate && new Date(record.eventDate) <= new Date(),
                            onClick: () => {
                                setSelectedRecord(record);
                                setEditModalVisible(true);
                            }
                        },
                        {
                            key: 'update',
                            label: 'Cập nhật trạng thái',
                            onClick: () => {
                                setSelectedRecord(record);
                                setModalVisible(true);
                            }
                        }
                    ]
                }}
                >
                    <Button
                    type="dashed"
                    size="small"
                    onClick={(e) => e.preventDefault()}
                    icon={<MoreOutlined />}
                >
                    
                </Button>
                </Dropdown>
            ],
            width: 40,
            align: 'center'
        },
        {
            title: 'Họ tên khách hàng',
            dataIndex: 'name',
            render: (text, record) => {
                if (record.gender === true) {
                    return <><WomanOutlined className="text-pink-500" /> {text}</>
                }
                if (record.gender === false) {
                    return <><ManOutlined className="text-blue-500" /> {text}</>
                }
                return text;
            },
            width: 200,
            minWidth: 200
        },
        {
            title: 'Số điện thoại',
            dataIndex: 'phoneNumber',
            width: 120,
            copyable: true,
            minWidth: 120
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
            dataIndex: 'staffName',
            search: false,
            width: 200,
            minWidth: 200,
            render: (text, record) => (
                <div className="flex items-center gap-1">
                    <Avatar size="small" src={record.staffAvatar} />{text}
                </div>
            )
        },
        {
            title: 'Sự kiện',
            dataIndex: 'eventDate',
            valueType: 'date',
            search: false,
            width: 100,
            minWidth: 100,
        },
        {
            title: 'Giờ',
            dataIndex: 'eventId',
            width: 60,
            valueType: 'select',
            request: apiEventOptions,
            minWidth: 60
        },
        {
            title: 'Ngày sự kiện',
            dataIndex: 'dateRange',
            valueType: 'dateRange',
            hideInTable: true,
            minWidth: 100,
            width: 100
        },
        {
            title: 'Xác nhận 2',
            dataIndex: 'confirm2',
            render: (text, record) => {
                return (
                    <div>
                        {getConfirm2StatusTag(record.confirm2)}

                    </div>
                )
            },
            valueType: 'select',
            fieldProps: {
                options: CONFIRM2_OPTIONS
            },
            width: 120
        },
        {
            title: 'Ghi chú',
            dataIndex: 'contactNote',
            search: false,
            minWidth: 200,
        },
        {
            title: 'Ghi chú xác nhận 2',
            dataIndex: 'note',
            search: false,
            minWidth: 200,
        }
    ]

    return (
        <PageContainer>
            <ProTable
                scroll={{
                    x: true
                }}
                actionRef={actionRef}
                search={{
                    layout: 'vertical'
                }}
                request={apiAttendanceSchedules}
                columns={columns}
                size="small"
            />
            <ModalForm
                title="Cập nhật xác nhận 2"
                open={modalVisible}
                onOpenChange={setModalVisible}
                onFinish={async (values) => {
                    await apiContactConfirm2({
                        leadId: selectedRecord?.id,
                        confirm2: values.confirm2,
                        reason: values.reason
                    });
                    message.success('Xác nhận thành công!');
                    actionRef.current?.reload();
                    return true;
                }}
                initialValues={{
                    confirm2: selectedRecord?.confirm2
                }}
            >
                <ProFormSelect
                    name="confirm2"
                    label="Trạng thái"
                    options={CONFIRM2_OPTIONS}
                    rules={[{ required: true, message: 'Vui lòng chọn trạng thái!' }]}
                />
                <ProFormTextArea
                    name="reason"
                    label="Lý do"
                    placeholder="Nhập lý do (nếu có)"
                    fieldProps={{
                        rows: 4
                    }}
                />
            </ModalForm>
            <ModalForm
                title="Chỉnh sửa lịch hẹn"
                open={editModalVisible}
                onOpenChange={setEditModalVisible}
                onFinish={async (values) => {
                    await apiUpdateAttendanceSchedule({
                        leadId: selectedRecord?.id,
                        name: values.name,
                        eventDate: values.eventDate,
                        eventId: values.eventId,
                        note: values.note,
                        confirm2: values.confirm2
                    });
                    message.success('Cập nhật lịch hẹn thành công!');
                    actionRef.current?.reload();
                    return true;
                }}
                initialValues={{
                    name: selectedRecord?.name,
                    eventDate: selectedRecord?.eventDate,
                    eventId: selectedRecord?.eventId,
                    note: selectedRecord?.note,
                    confirm2: selectedRecord?.confirm2
                }}
            >
                <ProFormText
                    name="name"
                    label="Họ tên khách hàng"
                    rules={[{ required: true, message: 'Vui lòng nhập họ tên!' }]}
                />
                <ProFormDatePicker
                    name="eventDate"
                    label="Ngày sự kiện"
                    rules={[{ required: true, message: 'Vui lòng chọn ngày sự kiện!' }]}
                    fieldProps={{ className: 'w-full', format: 'DD-MM-YYYY' }}
                />
                <ProFormSelect
                    name="eventId"
                    label="Sự kiện (Giờ)"
                    request={apiEventOptions}
                    rules={[{ required: true, message: 'Vui lòng chọn giờ sự kiện!' }]}
                />
                <ProFormSelect
                    name="confirm2"
                    label="Trạng thái xác nhận 2"
                    options={CONFIRM2_OPTIONS}
                />
                <ProFormTextArea
                    name="note"
                    label="Ghi chú"
                    placeholder="Nhập ghi chú (nếu có)"
                    fieldProps={{ rows: 3 }}
                />
            </ModalForm>
        </PageContainer>
    )
}

export default Index;