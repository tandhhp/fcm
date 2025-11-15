import { apiDepartmentCreate, apiDepartmentDelete, apiDepartmentList, apiDepartmentUpdate } from "@/services/department";
import { apiBranchOptions } from "@/services/settings/branch";
import { apiUserOptions } from "@/services/user";
import { DeleteOutlined, EditOutlined, MoreOutlined, PlusOutlined, UsergroupAddOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProFormInstance, ProFormSelect, ProFormText, ProTable } from "@ant-design/pro-components"
import { history, useAccess } from "@umijs/max";
import { Button, Col, Dropdown, message, Popconfirm, Row } from "antd";
import { useEffect, useRef, useState } from "react";

const Index: React.FC = () => {

    const [open, setOpen] = useState<boolean>(false);
    const [department, setDepartment] = useState<any>();
    const actionRef = useRef<ActionType>();
    const formRef = useRef<ProFormInstance>();
    const access = useAccess();

    useEffect(() => {
        if (open && department) {
            formRef.current?.setFields([
                {
                    name: 'branchId',
                    value: department.branchId
                },
                {
                    name: 'name',
                    value: department.name
                },
                {
                    name: 'id',
                    value: department.id
                },
                {
                    name: 'leaderId',
                    value: department.leaderId
                }
            ])
        }
    }, [open, department])

    const handleSubmit = async (values: any) => {
        if (values.id) {
            await apiDepartmentUpdate(values);
        } else {
            await apiDepartmentCreate(values);
        }
        message.success('Thành công');
        actionRef.current?.reload();
        formRef.current?.resetFields();
        setDepartment(undefined);
        return true;
    }

    const onDelete = async (id: string) => {
        await apiDepartmentDelete(id);
        message.success('Xóa thành công');
        actionRef.current?.reload();
    }

    return (
        <PageContainer extra={<Button type="primary" onClick={() => {
            setDepartment(undefined);
            setOpen(true);
        }} icon={<PlusOutlined />} hidden={!access.canHR}>Thêm phòng ban</Button>}>
            <ProTable
                request={apiDepartmentList}
                search={{
                    layout: 'vertical'
                }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center',
                    },
                    {
                        title: 'Tên phòng ban',
                        dataIndex: 'name',
                    },
                    {
                        title: 'Chi nhánh',
                        dataIndex: 'branchId',
                        valueType: 'select',
                        request: apiBranchOptions,
                        search: false
                    },
                    {
                        title: 'Giám đốc',
                        dataIndex: 'leaderName',
                        search: false
                    },
                    {
                        title: 'Nhóm',
                        dataIndex: 'teamCount',
                        valueType: 'digit',
                        search: false
                    },
                    {
                        title: 'Tác vụ',
                        valueType: 'option',
                        render: (_, record) => [
                            <Dropdown key="more" menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        onClick: () => {
                                            setDepartment(record);
                                            setOpen(true);
                                        },
                                        icon: <EditOutlined />
                                    },
                                    {
                                        key: 'team',
                                        label: 'Quản lý nhóm',
                                        onClick: () => {
                                            history.push(`/user/department/team/${record.id}`);
                                        },
                                        icon: <UsergroupAddOutlined />
                                    }
                                ]
                            }}>
                                <Button type="dashed" size="small" icon={<MoreOutlined />} />
                            </Dropdown>,
                            <Popconfirm key="delete" title="Bạn có chắc muốn xóa?" onConfirm={() => onDelete(record.id)} disabled={!access.canHR}>
                                <Button type="primary" danger size="small" icon={<DeleteOutlined />} />
                            </Popconfirm>
                        ],
                        width: 60
                    }
                ]}
                actionRef={actionRef}
            />
            <ModalForm open={open} onOpenChange={setOpen} title="Phòng ban" onFinish={handleSubmit} formRef={formRef}>
                <ProFormText name="id" hidden />
                <ProFormText name="name" label="Tên phòng ban" rules={[
                    {
                        required: true
                    }
                ]} />
                <Row gutter={16}>
                    <Col md={12} xs={24}>
                        <ProFormSelect name="leaderId" label="Trưởng phòng" request={apiUserOptions} showSearch rules={[{ required: true }]} />
                    </Col>
                    <Col md={12} xs={24}>
                        <ProFormSelect name="branchId" label="Chọn chi nhánh" request={apiBranchOptions} rules={[
                            {
                                required: true
                            }
                        ]} allowClear={false} showSearch />
                    </Col>
                </Row>
            </ModalForm>
        </PageContainer>
    )
}

export default Index;