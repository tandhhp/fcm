import { apiProvinceList } from "@/services/settings/province";
import { DeleteOutlined, EditOutlined, MoreOutlined, PlusOutlined, SettingOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { history } from "@umijs/max";
import { Button, Dropdown, Popconfirm } from "antd";
import { useRef, useState } from "react";
import ProvinceForm from "./components/form";

const Index: React.FC = () => {

    const actionRef = useRef<ActionType>();
    const [open, setOpen] = useState<boolean>(false);
    const [selectedProvince, setSelectedProvince] = useState<any>(null);

    return (
        <PageContainer extra={<Button type="primary" icon={<PlusOutlined />} onClick={() => setOpen(true)}>Thêm mới</Button>}>
            <ProTable
                actionRef={actionRef}
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
                        title: 'Tỉnh/Thành phố',
                        dataIndex: 'name'
                    },
                    {
                        title: 'Xã/Phường',
                        dataIndex: 'districtCount',
                        valueType: 'digit',
                        width: 120,
                        search: false
                    },
                    {
                        title: 'Tác vụ',
                        valueType: 'option',
                        render: (_, record) => [
                            <Dropdown key={`more`} menu={{
                                items: [
                                    {
                                        key: 'district',
                                        label: 'Quản lý xã/phường',
                                        icon: <SettingOutlined />,
                                        onClick: () => {
                                            history.push(`/settings/province/district/${record.id}`);
                                        }
                                    },
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        icon: <EditOutlined />,
                                        onClick: () => {
                                            setSelectedProvince(record);
                                            setOpen(true);
                                        }
                                    }
                                ]
                            }}>
                                <Button type="dashed" icon={<MoreOutlined />} size="small" />
                            </Dropdown>,
                            <Popconfirm key={`delete`} title="Bạn có chắc chắn muốn xóa tỉnh/thành phố này?" onConfirm={async () => {
                                // Handle delete confirmation
                            }}>
                                <Button type="primary" icon={<DeleteOutlined />} danger size="small" />
                            </Popconfirm>
                        ],
                        width: 60
                    }
                ]}
                request={apiProvinceList}
            />
            <ProvinceForm open={open} onOpenChange={setOpen} data={selectedProvince} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default Index;