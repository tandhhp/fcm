import { apiUserChangeApprove, apiUserChangeList } from "@/services/user";
import { CheckOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { Button, message, Popconfirm, Tag } from "antd";
import { useRef } from "react";

const UserChange: React.FC = () => {

    const actionRef = useRef<ActionType>()

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
                request={apiUserChangeList}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 40
                    },
                    {
                        title: 'Nội dung',
                        dataIndex: 'note',
                    },
                    {
                        title: 'Ngày',
                        dataIndex: 'createdDate',
                        valueType: 'fromNow',
                        search: false
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'status',
                        render: (_, entity) => {
                            if (entity.isAccept) {
                                return <Tag color="green">Đã phê duyệt</Tag>
                            }
                            return <Tag color="orange">Chờ phê duyệt</Tag>
                        },
                        width: 100,
                        search: false
                    },
                    {
                        title: 'Tác vụ',
                        valueType: 'option',
                        render: (_, entity) => [
                            <Popconfirm key="approve" title="Xác nhận thay đổi?" onConfirm={async () => {
                                await apiUserChangeApprove(entity.id);
                                message.success('Phê duyệt thành công!');
                                actionRef.current?.reload();
                            }}>
                                <Button type="primary" size="small" icon={<CheckOutlined />} hidden={entity.isAccept}>Phê duyệt</Button>
                            </Popconfirm>
                        ],
                        width: 120
                    }
                ]}
            />
        </PageContainer>
    )
}

export default UserChange;