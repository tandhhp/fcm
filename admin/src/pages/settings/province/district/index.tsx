import { apiDistrictDelete, apiDistrictList } from "@/services/settings/district";
import { DeleteOutlined, LeftOutlined, PlusOutlined, SettingOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { history, useParams } from "@umijs/max";
import { Button, message, Popconfirm } from "antd";
import DistrictForm from "./components/form";
import { useRef, useState } from "react";

const Index: React.FC = () => {

    const { id } = useParams<{ id: string }>();
    const actionRef = useRef<ActionType>();
    const [open, setOpen] = useState<boolean>(false);

    const onDelete = async (districtId: number) => {
        await apiDistrictDelete(districtId);
        actionRef.current?.reload();
        message.success('Xóa thành công');
    }

    return (
        <PageContainer extra={<Button icon={<LeftOutlined />} onClick={() => history.back()}>Quay lại</Button>}>
            <ProTable
                actionRef={actionRef}
                params={{ provinceId: id }}
                headerTitle={<Button type="primary" icon={<PlusOutlined />} onClick={() => setOpen(true)}>Thêm mới</Button>}
                columns={[
                    {
                        title: "#",
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Xã / Phường',
                        dataIndex: 'name'
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        render: (dom, entity) => [
                            <Popconfirm key={`delete`} title="Xác nhận xóa?" onConfirm={() => onDelete(entity.id)}>
                                <Button icon={<DeleteOutlined />} size="small" type="primary" danger />
                            </Popconfirm>
                        ],
                        width: 60
                    }
                ]}
                search={{
                    layout: 'vertical'
                }}
                rowKey={`id`}
                request={apiDistrictList}
            />
            <DistrictForm open={open} onOpenChange={setOpen} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default Index;
