import { apiContactConfirm2, apiContactNeedConfirm2 } from "@/services/contact";
import { ManOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProColumnType, ProForm, ProFormSelect, ProTable } from "@ant-design/pro-components"
import { useAccess } from "@umijs/max";
import { message } from "antd";
import { useRef } from "react";

const Index: React.FC = () => {

    const access = useAccess();
    const actionRef = useRef<ActionType>();

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
                    <ProForm submitter={false} readonly={!access.can_confirm2}>
                        <ProFormSelect name={"confirm2Status"} initialValue={record.confirm2Status} onChange={async (value) => {
                            await apiContactConfirm2({
                                contactId: record.id,
                                confirm2Status: value
                            });
                            message.success('Xác nhận thành công!');
                            actionRef.current?.reload();
                        }} 
                            formItemProps={{
                                className: 'mb-0'
                            }}
                            options={confirm2Options}
                            fieldProps={{
                                autoFocus: false,
                                variant: 'filled'
                            }}
                        />
                    </ProForm>
                )
            },
            valueType: 'select',
            fieldProps: {
                options: confirm2Options
            }
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
        </PageContainer>
    )
}

export default Index;