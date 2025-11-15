import { apiContactUnassignedList, deleteContact } from "@/services/contact";
import { DeleteOutlined, ImportOutlined, SettingOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { useAccess } from "@umijs/max";
import { Button, message, Popconfirm, Space } from "antd";
import ContactImport from "../components/import";
import { useRef, useState } from "react";
import ContactAssignForm from "../components/assign";
import { apiSourceOptions } from "@/services/settings/source";

const Index: React.FC = () => {
    const access = useAccess();
    const actionRef = useRef<ActionType>();
    const [openImport, setOpenImport] = useState<boolean>(false);

    const onDelete = async (id: string) => {
        await deleteContact(id);
        message.success('Xoá liên hệ thành công');
        actionRef.current?.reload();
    }

    return (
        <PageContainer>
            <ProTable
                actionRef={actionRef}
                request={apiContactUnassignedList}
                rowKey={"id"}
                headerTitle={(
                    <Space>
                        <Button icon={<ImportOutlined />} disabled={!access.canAdmin && !access.dot && !access.adminData} onClick={() => setOpenImport(true)}>Đổ dữ liệu</Button>
                        <ContactAssignForm reload={() => actionRef.current?.reload()} />
                    </Space>
                )}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'name',
                        search: false,
                    },
                    {
                        title: 'Số điện thoại',
                        dataIndex: 'phoneNumber',
                    },
                    {
                        title: 'Email',
                        dataIndex: 'email',
                        search: false,
                    },
                    {
                        title: 'Người tạo',
                        dataIndex: 'creatorName',
                        search: false
                    },
                    {
                        title: 'Ngày tạo',
                        valueType: 'date',
                        dataIndex: 'createdDate',
                        search: false,
                        width: 100
                    },
                    {
                        title: 'Nguồn liên hệ',
                        dataIndex: 'sourceId',
                        valueType: 'select',
                        request: apiSourceOptions                        
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note',
                        search: false,
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 50,
                        render: (text, record) => [
                            <Popconfirm key="delete" title="Xoá liên hệ?" onConfirm={() => onDelete(record.id)}>
                                <Button icon={<DeleteOutlined />} type="primary" danger size="small"></Button>
                            </Popconfirm>
                        ]
                    }
                ]}
                search={{
                    layout: 'vertical'
                }}
            />
            <ContactImport open={openImport} onOpenChange={setOpenImport} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default Index;