import { apiGiftDelete, apiGiftList } from "@/services/event/gift";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { useRef, useState } from "react";
import GiftForm from "./components/form";
import { Button, Dropdown, message, Popconfirm } from "antd";
import { DeleteOutlined, EditOutlined, MoreOutlined, PlusOutlined, SettingOutlined } from "@ant-design/icons";

const Index: React.FC = () => {

    const actionRef = useRef<ActionType>();
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [gift, setGift] = useState<any>(null);

    return (
        <PageContainer extra={<Button icon={<PlusOutlined />} type="primary" onClick={() => {
            setGift(null);
            setOpenForm(true);
        }}>Thêm quà tặng</Button>}>
            <ProTable
                actionRef={actionRef}
                request={apiGiftList}
                rowKey="id"
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
                        title: 'Tên quà tặng',
                        dataIndex: 'name',
                    },
                    {
                        title: <SettingOutlined />,
                        width: 60,
                        align: 'center',
                        valueType: 'option',
                        render: (_, record) => [
                            <Dropdown key={"more"} menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        onClick: () => {
                                            setGift(record);
                                            setOpenForm(true);
                                        },
                                        icon: <EditOutlined />
                                    }
                                ]
                            }}>
                                <Button type="dashed" icon={<MoreOutlined />} size="small" />
                            </Dropdown>,
                            <Popconfirm key={"delete"} title="Xoá quà tặng?" description="Hành động này không thể hoàn tác!" okText="Xoá" cancelText="Huỷ" onConfirm={async () => {
                                await apiGiftDelete(record.id);
                                actionRef.current?.reload();
                                message.success('Xoá quà tặng thành công');
                            }}>
                                <Button type="primary" danger icon={<DeleteOutlined />} size="small" />
                            </Popconfirm>
                        ]
                    }
                ]}
            />
            <GiftForm open={openForm} onOpenChange={setOpenForm} data={gift} reload={() => () => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default Index;