import { apiUserOptions } from "@/services/user";
import { apiCallCenterOptions } from "@/services/users/call-center";
import { apiGroupDataOptions } from "@/services/users/group-data";
import { apiTeamCreate, apiTeamDelete, apiTeamList, apiTeamUpdate } from "@/services/users/team";
import { DeleteOutlined, EditOutlined, LeftOutlined, MoreOutlined, PlusOutlined, UsergroupAddOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProFormInstance, ProFormSelect, ProFormText, ProTable } from "@ant-design/pro-components"
import { history, useParams } from "@umijs/max";
import { Button, Dropdown, message, Popconfirm } from "antd";
import { useEffect, useRef, useState } from "react";

const Index: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [open, setOpen] = useState<boolean>(false);
    const actionRef = useRef<ActionType>();
    const formRef = useRef<ProFormInstance>();
    const [team, setTeam] = useState<any>();

    useEffect(() => {
        if (open && team) {
            formRef.current?.setFields([
                {
                    name: 'name',
                    value: team.name
                },
                {
                    name: 'leaderId',
                    value: team.leaderId
                },
                {
                    name: 'id',
                    value: team.id
                },
                {
                    name: 'callCenterId',
                    value: team.callCenterId
                },
                {
                    name: 'groupDataId',
                    value: team.groupDataId
                }
            ]);
        }
    }, [open, team]);

    const onFinish = async (values: any) => {
        values.departmentId = id;
        if (values.id) {
            await apiTeamUpdate(values);
        } else {
            await apiTeamCreate(values);
        }
        message.success('Thành công');
        actionRef.current?.reload();
        formRef.current?.resetFields();
        setTeam(undefined);
        return true;
    }

    return (
        <PageContainer extra={<Button icon={<LeftOutlined />} onClick={() => history.back()}>Quay lại</Button>}>
            <ProTable
                headerTitle={<Button type="primary" onClick={() => setOpen(true)} icon={<PlusOutlined />}>Thêm nhóm</Button>}
                request={apiTeamList} params={{ departmentId: id }}
                actionRef={actionRef}
                search={{
                    layout: 'vertical'
                }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Tên nhóm',
                        dataIndex: 'name'
                    },
                    {
                        title: 'Trưởng nhóm',
                        dataIndex: 'leaderName',
                        search: false
                    },
                    {
                        title: 'Call Center',
                        dataIndex: 'callCenterName',
                        search: false
                    },
                    {
                        title: 'Group Data',
                        dataIndex: 'groupDataName',
                        search: false
                    },
                    {
                        title: 'Thành viên',
                        valueType: 'digit',
                        search: false,
                        dataIndex: 'userCount'
                    },
                    {
                        title: 'Tác vụ',
                        valueType: 'option',
                        render: (_, record) => [
                            <Dropdown key={`action`} menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        icon: <EditOutlined />,
                                        onClick: () => {
                                            setTeam(record);
                                            setOpen(true);
                                        }
                                    },
                                    {
                                        key: 'user',
                                        label: 'Quản lý thành viên',
                                        icon: <UsergroupAddOutlined />,
                                        onClick: () => {
                                            history.push(`/user/department/team/user/${record.id}`);
                                        }
                                    }
                                ]
                            }}>
                                <Button type="dashed" icon={<MoreOutlined />} size="small" />
                            </Dropdown>,
                            <Popconfirm key={`delete`} title="Bạn có chắc chắn muốn xóa nhóm này?" onConfirm={async () => {
                                await apiTeamDelete(record.id);
                                message.success('Xóa nhóm thành công');
                                actionRef.current?.reload();
                            }}>
                                <Button type="primary" danger icon={<DeleteOutlined />} size="small" />
                            </Popconfirm>
                        ],
                        width: 60
                    }
                ]}
            />
            <ModalForm open={open} onOpenChange={setOpen} title="Nhóm" formRef={formRef} onFinish={onFinish}>
                <ProFormText name="id" hidden />
                <ProFormText name="name" label="Tên nhóm" rules={[{ required: true }]} />
                <div className="grid grid-cols-2 gap-4">
                    <ProFormSelect name={"callCenterId"} label="Call Center" request={apiCallCenterOptions} />
                    <ProFormSelect name="groupDataId" label="Group Data" request={apiGroupDataOptions} />
                </div>
                <ProFormSelect name="leaderId" label="Trưởng nhóm" request={apiUserOptions} showSearch rules={[{ required: true }]} />
            </ModalForm>
        </PageContainer>
    )
}

export default Index;