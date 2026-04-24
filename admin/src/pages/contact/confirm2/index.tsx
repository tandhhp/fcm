import { apiContactConfirm2, apiContactNeedConfirm2 } from "@/services/contact";
import { ManOutlined, MoreOutlined, SettingOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProColumnType, ProForm, ProFormSelect, ProFormTextArea, ProTable } from "@ant-design/pro-components"
import { useAccess } from "@umijs/max";
import { Button, Dropdown, message, Tag } from "antd";
import { useRef, useState } from "react";

const Index: React.FC = () => {

    const access = useAccess();
    const actionRef = useRef<ActionType>();
    const [selectedRecord, setSelectedRecord] = useState<any>(null);
    const [modalVisible, setModalVisible] = useState(false);

    const confirm2Options = [
        {
            label: 'Chưa xác nhận',
            value: 0
        },
        {
            label: 'Đồng ý',
            value: 1
        },
        {
            label: 'Hủy',
            value: 2
        },
        {
            label: 'Chưa chắc chắn',
            value: 3
        },
        {
            label: 'Không nhấc máy',
            value: 4
        }
    ];

    const getConfirm2StatusTag = (status: number) => {
        const option = confirm2Options.find(o => o.value === status);
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
            width: 110
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
            dataIndex: 'telesalesName',
            search: false
        },
        {
            title: 'Lượt gọi',
            dataIndex: 'callCount',
            search: false
        },
        {
            title: 'Ngày hẹn',
            dataIndex: 'eventDate',
            valueType: 'date',
            search: false
        },
        {
            title: 'Khung giờ',
            dataIndex: 'eventName',
            search: false
        },
        {
            title: 'Ngày sự kiện',
            dataIndex: 'dateRange',
            valueType: 'dateRange',
            hideInTable: true
        },
        {
            title: 'Ghi chú',
            dataIndex: 'note',
            search: false
        },
        {
            title: 'Xác nhận 2',
            dataIndex: 'confirm2Status',
            render: (text, record) => {
                return (
                    <div>
                        {getConfirm2StatusTag(record.confirm2Status)}

                    </div>
                )
            },
            valueType: 'select',
            fieldProps: {
                options: confirm2Options
            }
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
                request={apiContactNeedConfirm2}
                columns={columns}
                size="small"
            />
            <ModalForm
                title="Cập nhật xác nhận 2"
                open={modalVisible}
                onOpenChange={setModalVisible}
                onFinish={async (values) => {
                    await apiContactConfirm2({
                        contactId: selectedRecord?.id,
                        confirm2Status: values.confirm2Status,
                        reason: values.reason
                    });
                    message.success('Xác nhận thành công!');
                    actionRef.current?.reload();
                    return true;
                }}
                initialValues={{
                    confirm2Status: selectedRecord?.confirm2Status
                }}
            >
                <ProFormSelect
                    name="confirm2Status"
                    label="Trạng thái"
                    options={confirm2Options}
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