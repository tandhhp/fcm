import { apiContactConfirm2, apiAttendanceSchedules } from "@/services/contact";
import { CONFIRM2_OPTIONS } from "@/utils/constants";
import { ManOutlined, MoreOutlined, SettingOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProColumnType, ProForm, ProFormSelect, ProFormTextArea, ProTable } from "@ant-design/pro-components"
import { useAccess } from "@umijs/max";
import { Avatar, Button, Dropdown, message, Tag } from "antd";
import { useRef, useState } from "react";

const Index: React.FC = () => {

    const access = useAccess();
    const actionRef = useRef<ActionType>();
    const [selectedRecord, setSelectedRecord] = useState<any>(null);
    const [modalVisible, setModalVisible] = useState(false);

    const getConfirm2StatusTag = (status: number) => {
        const option = CONFIRM2_OPTIONS.find(o => o.value === status);
        const colors = ['default', 'success', 'error', 'warning', 'default'];
        return <Tag color={colors[status] || 'default'}>{option?.label || 'Chưa xác nhận'}</Tag>;
    };

    const columns: ProColumnType<any>[] = [
        {
            title: 'STT',
            valueType: 'indexBorder',
            width: 50
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
            width: 200
        },
        {
            title: 'Số điện thoại',
            dataIndex: 'phoneNumber',
            width: 120,
            copyable: true
        },
        {
            title: 'Ngày tạo',
            dataIndex: 'createdDate',
            valueType: 'date',
            search: false,
            width: 100
        },
        {
            title: 'Nhân viên',
            dataIndex: 'staffName',
            search: false,
            width: 200,
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
            width: 100
        },
        {
            title: 'Giờ',
            dataIndex: 'eventName',
            search: false,
            width: 100
        },
        {
            title: 'Ngày sự kiện',
            dataIndex: 'dateRange',
            valueType: 'dateRange',
            hideInTable: true
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
            dataIndex: 'note',
            search: false
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
        </PageContainer>
    )
}

export default Index;