import { apiRoleInit, listRole } from "@/services/role"
import { EditOutlined, ExportOutlined, MoreOutlined, ReloadOutlined, UserAddOutlined } from "@ant-design/icons"
import { ActionType, PageContainer, ProColumns, ProTable } from "@ant-design/pro-components"
import { history } from "@umijs/max"
import { Button, Dropdown, Space } from "antd"
import UserImport from "./components/import"
import { useRef, useState } from "react"
import RoleForm from "./components/form"
import { apiUserExport } from "@/services/user"

const RolePage: React.FC = () => {

    const actionRef = useRef<ActionType>(null);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [role, setRole] = useState<API.Role>();

    const columns: ProColumns<API.Role>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
            width: 40,
            align: 'center'
        },
        {
            title: 'ID',
            dataIndex: 'name',
            width: 150,
            search: false
        },
        {
            title: 'Quyền',
            dataIndex: 'displayName'
        },
        {
            title: 'Mô tả',
            dataIndex: 'description'
        },
        {
            title: 'Đang làm',
            dataIndex: 'total'
        },
        {
            title: 'Đã nghỉ',
            dataIndex: 'leave'
        },
        {
            title: 'Chi tiết',
            valueType: 'option',
            render: (dom, entity) => [
                <Button key="detail" type="primary" size='small' icon={<UserAddOutlined />} onClick={() => history.push(`/user/roles/${entity.name}`)}>Quản lý</Button>,
                <Dropdown key="more" menu={{
                    items: [
                        {
                            key: 'edit',
                            label: 'Chỉnh sửa',
                            onClick: () => {
                                setRole(entity);
                                setOpenForm(true);
                            },
                            icon: <EditOutlined />
                        }
                    ]
                }}>
                    <Button type="dashed" size="small" icon={<MoreOutlined />} />
                </Dropdown>
            ],
            width: 100
        }
    ];

    const onInit = async () => {
        await apiRoleInit();
        actionRef.current?.reload();
    }

    const onExport = async () => {
        const response = await apiUserExport();
        const url = window.URL.createObjectURL(new Blob([response]));
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', 'roles.xlsx');
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
    }

    return (
        <PageContainer extra={(
            <Space>
                <UserImport reload={() => actionRef.current?.reload()} />
                <Button icon={<ReloadOutlined />} onClick={onInit}></Button>
            </Space>
        )}>
            <ProTable request={listRole}
                headerTitle={
                    <Button type="primary" onClick={onExport} icon={<ExportOutlined />}>Xuất Excel</Button>
                }
                actionRef={actionRef}
                rowKey="id"
                columns={columns}
                search={false}
                scroll={{
                    x: true
                }}
            />
            <RoleForm title="Thêm quyền" open={openForm} data={role} onOpenChange={setOpenForm} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default RolePage