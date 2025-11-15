import { apiTableList } from "@/services/settings/table";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { useParams, history, useRequest } from "@umijs/max";
import { useRef, useState } from "react";
import TableForm from "./components/form";
import { EditOutlined, LeftOutlined, MoreOutlined, PlusOutlined, SettingOutlined } from "@ant-design/icons";
import { Button, Dropdown } from "antd";
import { apiRoomDetail } from "@/services/settings/room";

const Index: React.FC = () => {

    const { id } = useParams<{ id: string }>();
    const actionRef = useRef<ActionType>(null);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [table, setTable] = useState<any>();
    const { data } = useRequest(() => apiRoomDetail(id));

    return (
        <PageContainer
            title={data?.name}
            extra={<Button icon={<LeftOutlined />} onClick={() => history.back()}>Quay lại</Button>}>
            <ProTable
                headerTitle={(
                    <Button icon={<PlusOutlined />} type="primary" onClick={() => {
                        setTable(undefined);
                        setOpenForm(true);
                    }}>Thêm bàn</Button>
                )}
                request={apiTableList}
                actionRef={actionRef}
                rowKey={`id`}
                search={{
                    layout: 'vertical'
                }}
                params={{ roomId: id }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Tên bàn',
                        dataIndex: 'name',
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        render: (text, record) => [
                            <Dropdown key={`more`} menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        onClick: () => {
                                            setTable(record);
                                            setOpenForm(true);
                                        },
                                        icon: <EditOutlined />
                                    }
                                ]
                            }}>
                                <Button type="dashed" size="small" icon={<MoreOutlined />} />
                            </Dropdown>
                        ],
                        width: 50,
                        align: 'center'
                    }
                ]}
            />
            <TableForm open={openForm} onOpenChange={setOpenForm} data={table} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default Index;