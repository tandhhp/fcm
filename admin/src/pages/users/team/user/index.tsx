import { apiAddUserToTeam, apiRemoveUserFromTeam, apiUnassignTeamUsers } from "@/services/team";
import { apiTeamDetail, apiTeamUsers } from "@/services/users/team";
import { DeleteOutlined, LeftOutlined, ManOutlined, SettingOutlined, UserOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProFormSelect, ProTable } from "@ant-design/pro-components"
import { history, useParams, useRequest } from "@umijs/max";
import { Button, message, Popconfirm } from "antd";
import { useRef, useState } from "react";

const Index: React.FC = () => {

    const { id } = useParams<{ id: string }>();
    const { data } = useRequest(() => apiTeamDetail(id));
    const actionRef = useRef<ActionType>(null);
    const [open, setOpen] = useState<boolean>(false);

    const onFinish = async (values: any) => {
        if (!id) return;
        await apiAddUserToTeam(id, values.userId);
        message.success('Đã thêm thành viên vào nhóm');
        actionRef.current?.reload();
        return true;
    }

    return (
        <PageContainer title={data?.name} extra={<Button icon={<LeftOutlined />} onClick={() => history.back()}>Quay lại</Button>}>
            <ProTable
                request={apiTeamUsers}
                headerTitle={<Button type="primary" onClick={() => setOpen(true)} icon={<UserOutlined />}>Thêm thành viên</Button>}
                params={{ teamId: id }}
                actionRef={actionRef}
                rowKey={"id"}
                search={{
                    layout: 'vertical'
                }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: <UserOutlined />,
                        dataIndex: 'avatar',
                        valueType: 'avatar',
                        width: 50,
                        search: false
                    },
                    {
                        title: 'Tài khoản',
                        dataIndex: 'userName',
                        search: false
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'name',
                        render: (dom, entity) => {
                            if (entity.gender === true) {
                                return <><WomanOutlined className="text-pink-500" /> {dom}</>
                            }
                            if (entity.gender === false) {
                                return <><ManOutlined className="text-blue-500" /> {dom}</>
                            }
                            return <>{dom}</>
                        }
                    },
                    {
                        title: 'Email',
                        dataIndex: 'email',
                    },
                    {
                        title: 'Số điện thoại',
                        dataIndex: 'phoneNumber',
                    },
                    {
                        title: 'Line Code',
                        dataIndex: 'lineCode',
                        search: false
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        render: (_, record) => [
                            <Popconfirm key={"remove"} title="Bạn có chắc chắn muốn xóa thành viên này khỏi nhóm?" onConfirm={async () => {
                                await apiRemoveUserFromTeam(record.id);
                                message.success('Đã xóa thành viên khỏi nhóm');
                                actionRef.current?.reload();
                            }}>
                                <Button type="primary" danger icon={<DeleteOutlined />} size="small" />
                            </Popconfirm>
                        ],
                        width: 50,
                        align: 'center'
                    }
                ]}
            />
            <ModalForm
                title="Thêm thành viên"
                open={open}
                onOpenChange={setOpen}
                onFinish={onFinish}
                modalProps={{
                    destroyOnHidden: true
                }}
            >
                <ProFormSelect name="userId" label="Người dùng" request={apiUnassignTeamUsers}
                    showSearch
                />
            </ModalForm>
        </PageContainer>
    )
}

export default Index;