import { activeComment, queryComments, removeComment } from "@/services/comment"
import { CheckOutlined, DeleteOutlined, StarFilled } from "@ant-design/icons";
import { ActionType, PageContainer, ProColumns, ProTable } from "@ant-design/pro-components"
import { useAccess } from "@umijs/max";
import { Button, Popconfirm, message } from "antd";
import { useRef } from "react";

const CommentPage: React.FC = () => {

    const actionRef = useRef<ActionType>();
    const access = useAccess();

    const active = async (commentId: string) => {
        const response = await activeComment(commentId);
        if (response.succeeded) {
            message.success('Actived!');
            actionRef.current?.reload();
        } else {
            message.error(response.errors[0].description);
        }
    }

    const remove = async (commentId: string) => {
        const response = await removeComment(commentId);
        if (response.succeeded) {
            message.success('Actived!');
            actionRef.current?.reload();
        } else {
            message.error(response.errors[0].description);
        }
    }

    const columns: ProColumns<any>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
            width: 40
        },
        {
            title: 'Nội dung',
            render: (dom, entity) => (
                <div>
                    <div className="mb-1"><b>{entity.fullName}</b> <span className="text-gray-400">({entity.userName})</span></div>
                    <div>{entity.content}</div>
                </div>
            )
        },
        {
            title: 'Trang',
            render: (dom, entity) => <a href="#">{entity.catalogName}</a>,
            search: false
        },
        {
            title: 'Ngày',
            dataIndex: 'createdDate',
            valueType: 'fromNow',
            width: 130,
            search: false
        },
        {
            title: 'Điểm',
            dataIndex: 'rate',
            render: (dom) => <div>{dom} <StarFilled className="text-yellow-500" /></div>,
            width: 80,
            search: false
        },
        {
            title: 'Trạng thái',
            dataIndex: 'status',
            valueEnum: {
                0: {
                    text: 'Draft',
                    status: 'Default',
                },
                1: {
                    text: 'Active',
                    status: 'Processing',
                },
            },
            width: 90,
            search: false
        },
        {
            title: 'Tác vụ',
            valueType: 'option',
            render: (dom, entity) => [
                <Button 
                key="publish" icon={<CheckOutlined />} type="primary" size="small" hidden={entity.status !== 0 || !access.canApproveComment} onClick={() => active(entity.id)}></Button>,
                <Popconfirm
                    title="Are you sure?"
                    key={2}
                    onConfirm={() => remove(entity.id)}
                >
                    <Button icon={<DeleteOutlined />} type="primary" size="small" danger hidden={!access.canApproveComment} />
                </Popconfirm>,
            ],
            width: 100
        },
    ];

    return (
        <PageContainer>
            <ProTable request={queryComments} columns={columns} actionRef={actionRef}
                search={{
                    layout: "vertical"
                }} />
        </PageContainer>
    )
}

export default CommentPage