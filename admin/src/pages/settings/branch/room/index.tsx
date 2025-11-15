import { apiRoomList } from "@/services/settings/room";
import { EditOutlined, LeftOutlined, MoreOutlined, PlusOutlined, SettingOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { history, useParams } from "@umijs/max";
import { Button, Dropdown } from "antd";
import RoomForm from "./components/form";
import { useRef, useState } from "react";

const Index: React.FC = () => {

    const { id } = useParams<{ id: string }>();
    const actionRef = useRef<ActionType>(null);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [room, setRoom] = useState<any>();

    return (
        <PageContainer extra={<Button icon={<LeftOutlined />} onClick={() => history.back()}>Quay lại</Button>}>
            <ProTable
                headerTitle={(
                    <Button type="primary" icon={<PlusOutlined />} onClick={() => {
                        setRoom(undefined);
                        setOpenForm(true);
                    }}>Thêm phòng</Button>
                )}
                actionRef={actionRef}
                rowKey={`id`}
                search={{
                    layout: 'vertical'
                }}
                request={apiRoomList}
                params={{ branchId: id }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Tên phòng',
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
                                            setRoom(record);
                                            setOpenForm(true);
                                        },
                                        icon: <EditOutlined />
                                    },
                                    {
                                        key: 'table',
                                        label: 'Quản lý bàn',
                                        onClick: () => {
                                            history.push(`/settings/branch/room/table/${record.id}`);
                                        },
                                        icon: <SettingOutlined />
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
            <RoomForm open={openForm} onOpenChange={setOpenForm} data={room} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default Index;