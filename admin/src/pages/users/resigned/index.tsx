import { apiLockUser, apiResignUserList, apiUnLockUser, apiUserByRoleOptions, apiUserOptions, apiUserUpdate, createEmployee, deleteUser, getUserInRoles, listUser, removeFromRole } from "@/services/user";
import { DeleteOutlined, EditOutlined, LeftOutlined, LockOutlined, ManOutlined, MoreOutlined, SettingOutlined, UserAddOutlined, UserOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, DrawerForm, ModalForm, PageContainer, ProColumns, ProFormDatePicker, ProFormInstance, ProFormSelect, ProFormText, ProTable } from "@ant-design/pro-components"
import { history, useAccess, useParams, useRequest } from "@umijs/max";
import { Button, Col, Dropdown, Popconfirm, Row, Select, Space, Tag, message } from "antd";
import dayjs from "dayjs";
import { useRef } from "react";

const Index: React.FC = () => {

    const { id } = useParams();
    const actionRef = useRef<ActionType>();
    const access = useAccess();

    const onConfirm = async (id?: string) => {
        const response = await deleteUser(id);
        if (response.succeeded) {
            message.success('Deleted');
            actionRef.current?.reload();
        }
    }

    const onLock = async (id: string) => {
        await apiLockUser(id);
        message.success('Thành công!');
        actionRef.current?.reload();
    }

    const onUnLock = async (id: string) => {
        await apiUnLockUser(id);
        message.success('Thành công!');
        actionRef.current?.reload();
    }

    const columns: ProColumns<any>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
            width: 30
        },
        {
            title: <UserOutlined />,
            dataIndex: 'avatar',
            search: false,
            valueType: 'avatar',
            width: 30
        },
        {
            title: 'Họ & Tên',
            dataIndex: 'name',
            render: (dom, entity) => (
                <div>
                    {entity.gender === false && (<ManOutlined className='text-blue-500' />)}{entity.gender === true && (<WomanOutlined className='text-red-500' />)} {dom}
                </div>
            ),
            width: 200
        },
        {
            title: 'Tài khoản',
            dataIndex: 'userName',
            search: false,
            width: 150
        },
        {
            title: 'Email',
            dataIndex: 'email',
            ellipsis: true,
            width: 200
        },
        {
            title: 'Số điện thoại',
            dataIndex: 'phoneNumber',
            width: 110
        },
        {
            title: 'Ngày sinh',
            dataIndex: 'dateOfBirth',
            valueType: 'date',
            width: 100,
            search: false,
            render: (_, entity) => entity.dateOfBirth ? dayjs(entity.dateOfBirth).format('DD-MM-YYYY') : '-'
        },
        {
            title: 'Chức vụ',
            dataIndex: 'position',
            search: false,
            width: 150
        },
        {
            title: 'Địa chỉ',
            dataIndex: 'address',
            search: false,
            ellipsis: true
        },
        {
            title: 'Ngày vào làm',
            dataIndex: 'contractDate',
            valueType: 'date',
            search: false,
            width: 120
        },
        {
            title: 'Team',
            dataIndex: 'teamName',
            search: false,
            width: 150
        },
        {
            title: 'Tác vụ',
            valueType: 'option',
            render: (dom, entity) => [
                <Popconfirm title="Mở khóa tài khoản?" key="unlock" onConfirm={() => onUnLock(entity.id)}>
                    <Button icon={<LockOutlined />} size='small' disabled={!access.hr} />
                </Popconfirm>,
                <Popconfirm title="Xác nhận xóa?" key={2} onConfirm={() => onConfirm(entity.id)}>
                    <Button type="primary" icon={<DeleteOutlined />} size='small' danger disabled={!access.canAdmin} />
                </Popconfirm>
            ],
            width: 80
        }
    ]

    return (
        <PageContainer
            extra={
                <Button icon={<LeftOutlined />} onClick={() => history.back()}>Quay lại</Button>
            }
        >
            <ProTable
                scroll={{
                    x: true
                }}
                search={{
                    layout: 'vertical'
                }}
                request={apiResignUserList} columns={columns}
                actionRef={actionRef} />
        </PageContainer>
    )
}

export default Index