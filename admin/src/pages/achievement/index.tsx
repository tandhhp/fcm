import { apiAchieventApprove, apiAchieventRemove, apiListApproveAchievent } from "@/services/user";
import { CheckOutlined, DeleteOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { Button, message, Popconfirm } from "antd";
import { useRef } from "react";

const AchievementPage: React.FC = () => {

    const actionRef = useRef<ActionType>();

    return (
        <PageContainer>

            <ProTable
                scroll={{
                    x: true
                }}
                actionRef={actionRef}
                request={apiListApproveAchievent}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Icon',
                        dataIndex: 'icon',
                        valueType: 'avatar',
                        width: 80
                    },
                    {
                        title: 'Tên thành tựu',
                        dataIndex: 'name'
                    },
                    {
                        title: 'Ngày tạo',
                        dataIndex: 'createdDate',
                        valueType: 'dateTime',
                        search: false
                    },
                    {
                        title: 'Chủ thẻ',
                        dataIndex: 'cardHolderName'
                    },
                    {
                        title: 'CX',
                        dataIndex: 'cxName',
                        search: false
                    },
                    {
                        title: 'Tài khoản CX',
                        dataIndex: 'cxUserName',
                        search: false
                    },
                    {
                        title: 'Ngày phê duyệt',
                        dataIndex: 'approvedDate',
                        valueType: 'dateTime',
                        search: false
                    },
                    {
                        title: 'Tác vụ',
                        render: (_, entity) => [
                            <Popconfirm key="approve" title="Xác nhận?" onConfirm={async () => {
                                await apiAchieventApprove(entity.id);
                                message.success('Thành công!');
                                actionRef.current?.reload();
                            }}>
                                <Button type="primary" size="small" icon={<CheckOutlined />} hidden={entity.isApproved}>Phê duyệt</Button>
                            </Popconfirm>,
                            <Popconfirm key="delete" title="Xác nhận?" onConfirm={async () => {
                                await apiAchieventRemove(entity.id);
                                message.success('Thành công!');
                                actionRef.current?.reload();
                            }}>
                                <Button type="primary" size="small" icon={<DeleteOutlined />} danger>Xóa</Button>
                            </Popconfirm>
                        ],
                        valueType: 'option',
                        width: 100
                    }
                ]}
                search={{
                    layout: 'vertical'
                }}
            />
        </PageContainer>
    )
}

export default AchievementPage;