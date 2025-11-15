import { apiListContactActivity, deleteContact } from "@/services/contact";
import { FolderOutlined, DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProColumnType, ProFormDatePicker, ProFormTextArea, ProTable } from "@ant-design/pro-components"
import { useParams } from "@umijs/max";
import { Tooltip, Button, Popconfirm, message } from "antd";
import { useRef, useState } from "react";

const ContactActivityPage: React.FC = () => {
    const actionRef = useRef<ActionType>();
    const { id } = useParams();
    const [open, setOpen] = useState<boolean>(false);

    const columns: ProColumnType<any>[] = [
        {
            title: 'STT',
            valueType: 'indexBorder',
            width: 50
        },
        {
            title: 'Ngày gọi',
            dataIndex: 'calledDate',
            width: 200,
        },
        {
            title: 'Ghi chú',
            dataIndex: 'note'
        },
        {
            title: 'Tác vụ',
            valueType: 'option',
            render: (dom, entity) => [
                <Tooltip title="Cập nhật" key="update">
                    <Button size="small" type="primary" icon={<FolderOutlined />} />
                </Tooltip>,
                <Popconfirm key="delete" title="Bạn có chắc chắn muốn xóa?" onConfirm={async () => {
                    await deleteContact(entity.id);
                    message.success('Xóa thành công!');
                    actionRef.current?.reload();
                }}>
                    <Button type="primary" danger icon={<DeleteOutlined />} size="small"></Button>
                </Popconfirm>
            ],
            width: 100
        }
    ]

    return (
        <PageContainer extra={<Button type="primary" icon={<PlusOutlined />} onClick={() => setOpen(true)}>Tạo hoạt động</Button>}>
            <ProTable
                actionRef={actionRef}
                search={{
                    layout: 'vertical'
                }}
                request={() => apiListContactActivity(id)}
                columns={columns}
            />
            <ModalForm open={open} onOpenChange={setOpen} title="Hoạt động">
                <ProFormDatePicker name="calledDate" label="Ngày gọi" rules={[{
                    required: true
                }]} />
                <ProFormTextArea name="note" label="Note" />
            </ModalForm>
        </PageContainer>
    )
}

export default ContactActivityPage;