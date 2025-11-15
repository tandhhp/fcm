import { apiSubUserAdd, apiSubUserDelete, apiSubUserList, apiSubUserUpdate } from "@/services/user";
import { GENDER_OPTIONS } from "@/utils/constants";
import { DeleteOutlined, EditOutlined, UserAddOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, ProCard, ProColumnType, ProFormDatePicker, ProFormInstance, ProFormSelect, ProFormText, ProTable } from "@ant-design/pro-components";
import { useParams } from "@umijs/max";
import { Button, Col, Popconfirm, Row, Tooltip, message } from "antd";
import dayjs from "dayjs";
import { useEffect, useRef, useState } from "react";

const SubUser: React.FC = () => {

    const { id } = useParams();
    const formRef = useRef<ProFormInstance>();
    const actionRef = useRef<ActionType>();

    const [open, setOpen] = useState<boolean>(false);
    const [user, setUser] = useState<any>();
    useEffect(() => {
        if (user) {
            formRef.current?.setFields([
                {
                    name: 'id',
                    value: user.id
                },
                {
                    name: 'name',
                    value: user.name
                },
                {
                    name: 'phoneNumber',
                    value: user.phoneNumber
                },
                {
                    name: 'email',
                    value: user.email
                },
                {
                    name: 'identityNumber',
                    value: user.identityNumber
                },
                {
                    name: 'gender',
                    value: user.gender
                },
                {
                    name: 'dateOfBirth',
                    value: user.dateOfBirth
                }
            ])
        }
    }, [user]);

    const onFinish = async (values: any) => {
        values.userId = id;
        if (values.dateOfBirth) {
            values.dateOfBirth = dayjs(values.dateOfBirth).format('YYYY-MM-DD');
        }
        if (values.id) {
            await apiSubUserUpdate(values);
            message.success('Cập nhật thành công!');
        } else {
            await apiSubUserAdd(values);
            message.success('Thêm thành công!');
        }
        setUser(null);
        formRef.current?.resetFields();
        actionRef.current?.reload();
        setOpen(false);
    }

    const columns: ProColumnType<any>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
            width: 50,
            align: 'center'
        },
        {
            title: 'Họ và tên',
            dataIndex: 'name'
        },
        {
            title: 'CMT/CCCD/Hộ chiếu',
            dataIndex: 'identityNumber'
        },
        {
            title: 'Email',
            dataIndex: 'email'
        },
        {
            title: 'Số điện thoại',
            dataIndex: 'phoneNumber'
        },
        {
            title: 'Ngày sinh',
            dataIndex: 'dateOfBirth',
            valueType: 'date',
            render: (_, entity) => entity.dateOfBirth ? dayjs(entity.dateOfBirth).format('DD-MM-YYYY') : '-'
        },
        {
            title: 'Giới tính',
            dataIndex: 'gender',
            valueEnum: {
                false: 'Nam',
                true: 'Nữ'
            }
        },
        {
            title: 'Tác vụ',
            valueType: 'option',
            render: (dom, entity) => [
                <Tooltip title="Chỉnh sửa" key="edit">
                    <Button type="primary" size="small" icon={<EditOutlined />} onClick={() => {
                        setUser(entity);
                        setOpen(true);
                    }} />
                </Tooltip>,
                <Popconfirm key="delete" title="Bạn có chắc chắn muốn xóa?" onConfirm={() => {
                    apiSubUserDelete(entity.id).then(() => {
                        message.success('Xóa thành công!');
                        actionRef.current?.reload();
                    })
                }}>
                    <Button type="primary" danger size="small" icon={<DeleteOutlined />} />
                </Popconfirm>
            ],
            width: 100,
            align: 'center'
        }
    ]

    return (
        <>
            <ProCard
                extra={<Button type="primary" icon={<UserAddOutlined />} onClick={() => setOpen(true)}>Thêm thành viên</Button>}
                ghost>
                <ProTable
                    scroll={{
                        x: true
                    }}
                    actionRef={actionRef}
                    search={false}
                    ghost
                    request={() => apiSubUserList(id)}
                    columns={columns}
                />
            </ProCard>
            <ModalForm open={open} onOpenChange={setOpen} title="Thành viên" onFinish={onFinish} formRef={formRef}>
                <ProFormText name="id" hidden />
                <ProFormText label="Họ và tên" name="name" rules={[
                    {
                        required: true
                    }
                ]} />
                <Row gutter={16}>
                    <Col span={12}>
                        <ProFormText label="Số điện thoại" name="phoneNumber" />
                    </Col>
                    <Col span={12}>
                        <ProFormText label="Email" name="email" rules={[
                            {
                                type: 'email'
                            }
                        ]} />
                    </Col>
                    <Col span={8}>
                        <ProFormDatePicker label="Ngày sinh" name="dateOfBirth" width="xl" fieldProps={{
                            format: {
                                type: 'mask',
                                format: 'DD-MM-YYYY'
                            }
                        }} />
                    </Col>
                    <Col span={8}>
                        <ProFormText label="CMND/CCCD/Hộ chiếu" name="identityNumber" />
                    </Col>
                    <Col span={8}>
                        <ProFormSelect label="Giới tính" name="gender" options={GENDER_OPTIONS} />
                    </Col>
                </Row>
            </ModalForm>
        </>
    )
}

export default SubUser;