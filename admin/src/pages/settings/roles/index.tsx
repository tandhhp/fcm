import { listRole } from "@/services/role"
import { DeleteOutlined, EditOutlined, FolderOutlined, PlusOutlined } from "@ant-design/icons";
import { ModalForm, PageContainer, ProColumns, ProFormText, ProTable } from "@ant-design/pro-components"
import { Button, Popconfirm } from "antd";
import { useState } from "react";

const Roles: React.FC = () => {
    const [open, setOpen] = useState<boolean>(false);
    const columns: ProColumns<API.AppSetting>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
        },
        {
            title: 'id',
            dataIndex: 'id',
        },
        {
            title: 'name',
            dataIndex: 'name'
        },
        {
            title: 'normalizedName',
            dataIndex: 'normalizedName'
        },
        {
            title: '',
            valueType: 'option',
            render: (dom, entity) => [
                <Button
                    icon={<FolderOutlined />}
                    type="primary"
                    key={0}
                ></Button>,
                <Button
                    icon={<EditOutlined />}
                    key={1}
                ></Button>,
                <Popconfirm key={2} title="Are you sure?">
                    <Button
                        type="primary"
                        danger
                        icon={<DeleteOutlined />}
                    ></Button>,
                </Popconfirm>
            ],
        },
    ];
    return (
        <PageContainer extra={<Button type="primary" icon={<PlusOutlined />} onClick={() => setOpen(true)}>Create new</Button>}>
            <ProTable request={listRole} columns={columns} search={{
                layout: 'vertical'
            }} />
            <ModalForm open={open} onOpenChange={setOpen} title="Roles">
                <ProFormText name="name" label="Name" rules={[
                    {
                        required: true
                    }
                ]} />
            </ModalForm>
        </PageContainer>
    )
}

export default Roles