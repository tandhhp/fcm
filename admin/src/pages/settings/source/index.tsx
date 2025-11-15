import { apiSourceList } from "@/services/settings/source";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { useRef, useState } from "react";
import SourceForm from "./components/form";
import { EditOutlined, MoreOutlined, PlusOutlined, SettingOutlined } from "@ant-design/icons";
import { Button, Dropdown } from "antd";

const Index: React.FC = () => {

    const actionRef = useRef<ActionType>(null);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [selectedRow, setSelectedRow] = useState<any>(null);

    return (
        <PageContainer extra={<Button type="primary" onClick={() => setOpenForm(true)} icon={<PlusOutlined />}>Thêm nguồn</Button>}>
            <ProTable
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
        </PageContainer>
    )
}

export default Index;