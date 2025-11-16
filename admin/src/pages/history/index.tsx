import { apiDeleteAllLog, deleteLog, queryLogs } from "@/services/log";
import { CheckOutlined, DeleteOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProColumns, ProTable } from "@ant-design/pro-components"
import { Button, Popconfirm, message } from "antd";
import { useRef } from "react";

const LogPage: React.FC = () => {

    const actionRef = useRef<ActionType>();

    const remove = async (logId: string) => {
        const response = await deleteLog(logId);
        if (response.succeeded) {
            message.success('Deleted!');
            actionRef.current?.reload();
        } else {
            message.error(response[0].description);
        }
    }

    const columns: ProColumns<any>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
            width: 50,
            align: 'center'
        },
        {
            title: 'Nội dung',
            dataIndex: 'message'
        },
        {
            title: 'Ngày',
            dataIndex: 'createdDate',
            valueType: 'fromNow',
            width: 180
        },
        {
            title: 'Tác vụ',
            valueType: 'option',
            render: (dom, entity) => [
                <Popconfirm
                    title="Are you sure?"
                    key={2}
                    onConfirm={() => remove(entity.id)}
                >
                    <Button icon={<DeleteOutlined />} type="primary" size="small" danger />
                </Popconfirm>,
            ],
            width: 60
        },
    ];

    return (
        <PageContainer extra={(
            <Popconfirm title="Are you sure?" onConfirm={() => {
                apiDeleteAllLog().then(() => {
                    actionRef.current?.reload();
                    message.success('Xóa thành công!');
                });
            }}>
                <Button type="primary" danger icon={<DeleteOutlined />}>Xóa tất cả</Button>
            </Popconfirm>
        )}>
            <ProTable request={queryLogs} columns={columns} actionRef={actionRef}
                search={{
                    layout: 'vertical'
                }}
                scroll={{
                    x: true
                }}
            />
        </PageContainer>
    )
}

export default LogPage