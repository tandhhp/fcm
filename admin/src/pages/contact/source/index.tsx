import { apiSourceList } from "@/services/settings/source";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { useRef, useState } from "react";
import { EditOutlined, ImportOutlined, MoreOutlined, PlusOutlined, SettingOutlined } from "@ant-design/icons";
import { Button, Dropdown, Space } from "antd";
import SourceForm from "@/pages/settings/source/components/form";
import { useAccess } from "@umijs/max";
import ContactImport from "../components/import";

const Index: React.FC = () => {

    const access = useAccess();
    const actionRef = useRef<ActionType>(null);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [selectedRow, setSelectedRow] = useState<any>(null);
    const [openImport, setOpenImport] = useState<boolean>(false);

    return (
        <PageContainer extra={<Button type="primary" onClick={() => setOpenForm(true)} icon={<PlusOutlined />}>Thêm nguồn</Button>}>
            <ProTable
                headerTitle={(
                    <Space>
                        <Button icon={<ImportOutlined />} disabled={!access.canAdmin && !access.dot && !access.adminData} onClick={() => setOpenImport(true)}>Đổ dữ liệu</Button>
                    </Space>
                )}
                actionRef={actionRef}
                rowKey={"id"}
                search={{
                    layout: 'vertical'
                }}
                request={apiSourceList}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Tên nguồn',
                        dataIndex: 'name'
                    },
                    {
                        title: 'SL liên hệ',
                        dataIndex: 'contactCount',
                        search: false,
                        valueType: 'digit'
                    },
                    {
                        title: 'Đã phân bổ',
                        dataIndex: 'assignedCount',
                        search: false,
                        valueType: 'digit'
                    },
                    {
                        title: 'Chưa phân bổ',
                        search: false,
                        valueType: 'digit',
                        render: (_, entity) => {
                            return entity.contactCount - entity.assignedCount;
                        }
                    },
                    {
                        title: 'Đã gọi',
                        dataIndex: 'dialedCount',
                        align: 'center',
                        search: false,
                        valueType: 'digit'
                    },
                    {
                        title: 'Chưa gọi',
                        align: 'center',
                        search: false,
                        valueType: 'digit',
                        render: (_, entity) => {
                            return entity.contactCount - entity.dialedCount;
                        }
                    },
                    {
                        title: 'SL cơ hội',
                        dataIndex: 'leadCount',
                        search: false,
                        valueType: 'digit'
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 50,
                        align: 'center',
                        render: (_, entity) => [
                            <Dropdown key={"more"} menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        onClick: () => {
                                            setSelectedRow(entity);
                                            setOpenForm(true);
                                        },
                                        icon: <EditOutlined />
                                    }
                                ]
                            }}>
                                <Button type="dashed" size="small" icon={<MoreOutlined />} />
                            </Dropdown>
                        ]
                    }
                ]}
            />
            <SourceForm open={openForm} onOpenChange={setOpenForm} data={selectedRow} reload={() => actionRef.current?.reload()} />
            <ContactImport open={openImport} onOpenChange={setOpenImport} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default Index;