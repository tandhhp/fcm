import {
    ActionType,
    ModalForm,
    PageContainer,
    ProFormInstance,
    ProFormText,
    ProTable
} from "@ant-design/pro-components"
import {
    apiGroupDataCreate,
    apiGroupDataDelete,
    apiGroupDataList,
    apiGroupDataUpdate
} from "@/services/users/group-data";
import { DeleteOutlined, EditOutlined, MoreOutlined, PlusOutlined, SettingOutlined } from "@ant-design/icons";
import { Button, Dropdown, message, Popconfirm } from "antd";
import { useAccess } from "@umijs/max";
import { useEffect, useRef, useState } from "react";

type GroupDataItem = {
    id: number;
    name: string;
    teamCount: number;
};

const Index: React.FC = () => {
    const access = useAccess();
    const actionRef = useRef<ActionType>();
    const formRef = useRef<ProFormInstance>();
    const [open, setOpen] = useState<boolean>(false);
    const [selectedRow, setSelectedRow] = useState<GroupDataItem | null>(null);

    useEffect(() => {
        if (open && selectedRow) {
            formRef.current?.setFields([
                {
                    name: 'id',
                    value: selectedRow.id
                },
                {
                    name: 'name',
                    value: selectedRow.name
                }
            ]);
        }
    }, [open, selectedRow]);

    const onFinish = async (values: { id?: number; name: string }) => {
        if (values.id) {
            await apiGroupDataUpdate(values as { id: number; name: string });
            message.success('Cập nhật group data thành công');
        } else {
            await apiGroupDataCreate({ name: values.name });
            message.success('Tạo group data thành công');
        }
        actionRef.current?.reload();
        setSelectedRow(null);
        formRef.current?.resetFields();
        return true;
    };

    const onDelete = async (id: number) => {
        await apiGroupDataDelete(id);
        message.success('Xóa group data thành công');
        actionRef.current?.reload();
    };

    return (
        <PageContainer extra={<Button type="primary" icon={<PlusOutlined />} onClick={() => {
            setSelectedRow(null);
            formRef.current?.resetFields();
            setOpen(true);
        }} disabled={!access.canAdmin && !access.dot && !access.adminData}>Tạo group data</Button>}>
            <ProTable<GroupDataItem>
                actionRef={actionRef}
                rowKey={'id'}
                request={apiGroupDataList}
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
                        title: 'Group data',
                        dataIndex: 'name'
                    },
                    {
                        title: 'Số lượng nhóm',
                        dataIndex: 'teamCount',
                        search: false,
                        valueType: 'digit'
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 80,
                        render: (_, record) => [
                            <Dropdown key="more" menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        icon: <EditOutlined />,
                                        onClick: () => {
                                            setSelectedRow(record);
                                            setOpen(true);
                                        },
                                        disabled: !access.canAdmin && !access.dot && !access.adminData
                                    }
                                ]
                            }}>
                                <Button type="dashed" icon={<MoreOutlined />} size="small" />
                            </Dropdown>,
                            <Popconfirm key="delete" title="Bạn có chắc muốn xóa group data này?" onConfirm={() => onDelete(record.id)}>
                                <Button type="primary" danger size="small" icon={<DeleteOutlined />} disabled={!access.canAdmin && !access.dot && !access.adminData} />
                            </Popconfirm>
                        ]
                    }
                ]}
            />
            <ModalForm
                open={open}
                onOpenChange={setOpen}
                title={selectedRow ? 'Chỉnh sửa group data' : 'Tạo group data'}
                onFinish={onFinish}
                formRef={formRef}
            >
                <ProFormText name="id" hidden />
                <ProFormText
                    name="name"
                    label="Tên group data"
                    rules={[
                        {
                            required: true,
                            message: 'Vui lòng nhập tên group data'
                        }
                    ]}
                />
            </ModalForm>
        </PageContainer>
    )
}

export default Index;