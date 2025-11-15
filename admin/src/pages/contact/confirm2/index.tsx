import { apiContactConfirm2, apiContactNeedConfirm2 } from "@/services/contact";
import { ManOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProColumnType, ProTable } from "@ant-design/pro-components"
import { useAccess } from "@umijs/max";
import { message, Switch } from "antd";
import { useRef } from "react";

const Index: React.FC = () => {

    const access = useAccess();
    const actionRef = useRef<ActionType>();

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
            title: 'Email',
            dataIndex: 'email',
            search: false
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
            title: 'Ghi chú',
            dataIndex: 'note',
            search: false
        },
        {
            title: 'Xác nhận 2',
            dataIndex: 'confirm2',
            render: (text, record) => {
                return <Switch checked={record.confirm2} size="small" onChange={async () => {
                    await apiContactConfirm2(record.id);
                    message.success('Xác nhận thành công!');
                    actionRef.current?.reload();
                }} disabled={!access.can_confirm2} />
            },
            search: false
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
            />
        </PageContainer>
    )
}

export default Index;