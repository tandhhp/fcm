import { apiAddCity, apiDeleteCity, apiListCity } from "@/services/tour";
import { DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProFormInstance, ProFormText, ProTable } from "@ant-design/pro-components"
import { Button, message, Popconfirm } from "antd";
import { useRef, useState } from "react";

const Index: React.FC = () => {

    const actionRef = useRef<ActionType>();
    const formRef = useRef<ProFormInstance>();
    const [open, setOpen] = useState<boolean>();

    const onFinish = async (values: any) => {
        await apiAddCity(values);
        message.success('Thành công!');
        actionRef.current?.reload();
        formRef.current?.resetFields();
        setOpen(false);
    }

    return (
        <PageContainer extra={<Button type="primary" icon={<PlusOutlined />} onClick={() => setOpen(true)}>Thêm mới</Button>}>
            <ProTable
            actionRef={actionRef}
                search={{
                    layout: 'vertical'
                }}
                request={apiListCity}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Địa điểm',
                        dataIndex: 'name'
                    },
                    {
                        title: 'Tác vụ',
                        valueType: 'option',
                        render: (_, entity) => [
                            <Popconfirm key="delete" title="Xác nhận xóa?" onConfirm={async () => {
                                await apiDeleteCity(entity.id);
                                message.success('Thành công!');
                                actionRef.current?.reload();
                            }}>
                                <Button type="primary" danger size="small" icon={<DeleteOutlined />} />
                            </Popconfirm>
                        ],
                        width: 60
                    }
                ]}
            />
            <ModalForm open={open} onOpenChange={setOpen} title="Địa điểm" formRef={formRef} onFinish={onFinish}>
                <ProFormText name="name" label="Địa điểm" rules={[
                    {
                        required: true
                    }
                ]} />
            </ModalForm>
        </PageContainer>
    )
}

export default Index;