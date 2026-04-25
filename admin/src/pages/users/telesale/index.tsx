import { apiLockUser, apiTelesaleCreate, apiTelesaleDelete, apiTelesaleList, apiTelesaleUpdate, apiUnLockUser, apiGetManagerOptions } from "@/services/user";
import { apiBranchOptions } from "@/services/settings/branch";
import { apiTeamOptions } from "@/services/team";
import { DeleteOutlined, EditOutlined, LockOutlined, ManOutlined, PlusOutlined, UnlockOutlined, UserOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProFormDatePicker, ProFormInstance, ProFormRadio, ProFormSelect, ProFormText, ProTable } from "@ant-design/pro-components"
import { Button, Col, message, Popconfirm, Row, Tag } from "antd";
import { useEffect, useRef, useState } from "react";
import dayjs from "dayjs";

const Index: React.FC = () => {

    const [open, setOpen] = useState<boolean>(false);
    const [telesale, setTelesale] = useState<any>();
    const actionRef = useRef<ActionType>();
    const formRef = useRef<ProFormInstance>();

    useEffect(() => {
        if (open && telesale) {
            formRef.current?.setFieldsValue({
                id: telesale.id,
                userName: telesale.userName,
                name: telesale.name,
                email: telesale.email,
                phoneNumber: telesale.phoneNumber,
                gender: telesale.gender,
                dateOfBirth: telesale.dateOfBirth ? dayjs(telesale.dateOfBirth) : null,
                branchId: telesale.branchId,
                teamId: telesale.teamId,
                lineCode: telesale.lineCode,
                managerId: telesale.managerId,
            });
        } else if (open) {
            formRef.current?.resetFields();
        }
    }, [open, telesale])

    const handleSubmit = async (values: any) => {
        const data = {
            ...values,
            dateOfBirth: values.dateOfBirth ? dayjs(values.dateOfBirth).format('YYYY-MM-DD') : null,
        };
        
        if (values.id) {
            await apiTelesaleUpdate(data);
            message.success('Cập nhật thành công');
        } else {
            await apiTelesaleCreate(data);
            message.success('Tạo thành công');
        }
        actionRef.current?.reload();
        formRef.current?.resetFields();
        setTelesale(undefined);
        return true;
    }

    const onDelete = async (id: string) => {
        await apiTelesaleDelete(id);
        message.success('Xóa thành công');
        actionRef.current?.reload();
    }

    const onLock = async (id: string) => {
        await apiLockUser(id);
        message.success('Đã khóa tài khoản');
        actionRef.current?.reload();
    }

    const onUnlock = async (id: string) => {
        await apiUnLockUser(id);
        message.success('Đã mở khóa tài khoản');
        actionRef.current?.reload();
    }

    return (
        <PageContainer extra={<Button type="primary" onClick={() => {
            setTelesale(undefined);
            setOpen(true);
        }} icon={<PlusOutlined />}>Tạo nhân viên</Button>}>
            <ProTable
                request={apiTelesaleList}
                actionRef={actionRef}
                rowKey="id"
                search={{
                    layout: 'vertical'
                }}
                size="small"
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center',
                    },
                    {
                        title: <UserOutlined />,
                        dataIndex: 'avatar',
                        valueType: 'avatar',
                        width: 40,
                        search: false,
                        align: 'center'
                    },
                    {
                        title: 'Tài khoản',
                        dataIndex: 'userName',
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'name',
                        render: (dom, entity) => {
                            const genderIcon = entity.gender === false 
                                ? <ManOutlined className="text-blue-500" /> 
                                : entity.gender === true 
                                ? <WomanOutlined className="text-pink-500" /> 
                                : null;
                            return (
                                <>
                                    {genderIcon} {dom}
                                </>
                            )
                        }
                    },
                    {
                        title: 'Email',
                        dataIndex: 'email',
                        search: false
                    },
                    {
                        title: 'Số điện thoại',
                        dataIndex: 'phoneNumber',
                    },
                    {
                        title: 'Ngày sinh',
                        dataIndex: 'dateOfBirth',
                        valueType: 'date',
                        search: false,
                        render: (_, entity) => entity.dateOfBirth ? dayjs(entity.dateOfBirth).format('DD/MM/YYYY') : '-'
                    },
                    {
                        title: 'Chi nhánh',
                        dataIndex: 'branchId',
                        valueType: 'select',
                        request: apiBranchOptions,
                    },
                    {
                        title: 'Nhóm',
                        dataIndex: 'teamId',
                        valueType: 'select',
                        request: apiTeamOptions,
                        search: false
                    },
                    {
                        title: 'Line Code',
                        dataIndex: 'lineCode',
                        search: false
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'status',
                        valueType: 'select',
                        valueEnum: {
                            0: { text: 'Đang làm việc', status: 'Success' },
                            1: { text: 'Đã nghỉ việc', status: 'Error' }
                        },
                        width: 140,
                        search: false
                    },
                    {
                        title: 'Tác vụ',
                        valueType: 'option',
                        render: (_, record) => [
                            <Button key="edit" type="primary" icon={<EditOutlined />} size="small" onClick={() => {
                                setTelesale(record);
                                setOpen(true);
                            }} disabled={record.status === 1} />,
                            record.status === 0 ? (
                                <Popconfirm key="lock" title="Bạn có chắc muốn khóa tài khoản này?" onConfirm={() => onLock(record.id)}>
                                    <Button type="default" icon={<LockOutlined />} size="small" danger />
                                </Popconfirm>
                            ) : (
                                <Popconfirm key="unlock" title="Bạn có chắc muốn mở khóa tài khoản này?" onConfirm={() => onUnlock(record.id)}>
                                    <Button type="default" icon={<UnlockOutlined />} size="small" style={{ color: '#52c41a', borderColor: '#52c41a' }} />
                                </Popconfirm>
                            ),
                            <Popconfirm key="delete" title="Bạn có chắc muốn xóa?" onConfirm={() => onDelete(record.id)}>
                                <Button type="primary" danger size="small" icon={<DeleteOutlined />} />
                            </Popconfirm>
                        ],
                        width: 130,
                        align: 'center'
                    }
                ]}
            />
            <ModalForm 
                open={open} 
                onOpenChange={setOpen} 
                title={telesale ? "Cập nhật nhân viên" : "Tạo nhân viên"} 
                onFinish={handleSubmit} 
                formRef={formRef}
            >
                <ProFormText name="id" hidden />
                <Row gutter={16}>
                    <Col md={12} xs={24}>
                        <ProFormText 
                            name="userName" 
                            label="Tên đăng nhập" 
                            rules={[{ required: true, message: 'Vui lòng nhập tên đăng nhập' }]} 
                            disabled={!!telesale}
                        />
                    </Col>
                    <Col md={12} xs={24}>
                        <ProFormText 
                            name="name" 
                            label="Họ và tên" 
                            rules={[{ required: true, message: 'Vui lòng nhập họ và tên' }]} 
                        />
                    </Col>
                </Row>
                <Row gutter={16}>
                    <Col md={12} xs={24}>
                        <ProFormText 
                            name="email" 
                            label="Email" 
                            rules={[{ type: 'email', message: 'Email không hợp lệ' }]} 
                        />
                    </Col>
                    <Col md={12} xs={24}>
                        <ProFormText 
                            name="phoneNumber" 
                            label="Số điện thoại" 
                        />
                    </Col>
                </Row>
                <Row gutter={16}>
                    <Col md={12} xs={24}>
                        <ProFormRadio.Group
                            name="gender"
                            label="Giới tính"
                            options={[
                                { label: 'Nam', value: false },
                                { label: 'Nữ', value: true },
                            ]}
                        />
                    </Col>
                    <Col md={12} xs={24}>
                        <ProFormDatePicker 
                            name="dateOfBirth" 
                            label="Ngày sinh" 
                            width="md"
                            fieldProps={{
                                format: 'DD/MM/YYYY'
                            }}
                        />
                    </Col>
                </Row>
                <Row gutter={16}>
                    <Col md={12} xs={24}>
                        <ProFormSelect 
                            name="branchId" 
                            label="Chi nhánh" 
                            request={apiBranchOptions} 
                            rules={[{ required: true, message: 'Vui lòng chọn chi nhánh' }]}
                            showSearch
                        />
                    </Col>
                    <Col md={12} xs={24}>
                        <ProFormSelect 
                            name="teamId" 
                            label="Nhóm" 
                            request={apiTeamOptions}
                            showSearch
                        />
                    </Col>
                </Row>
                <Row gutter={16}>
                    <Col md={12} xs={24}>
                        <ProFormSelect 
                            name="managerId" 
                            label="Người quản lý" 
                            request={apiGetManagerOptions}
                            showSearch
                        />
                    </Col>
                    <Col md={12} xs={24}>
                        <ProFormText 
                            name="lineCode" 
                            label="Line Code" 
                        />
                    </Col>
                </Row>
            </ModalForm>
        </PageContainer>
    )
}

export default Index;
