import { apiCampaignList } from "@/services/event/campaign";
import { DeleteOutlined, EditOutlined, MoreOutlined, PlusOutlined, SettingOutlined, TagsOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { Button, Dropdown, Popconfirm } from "antd";
import { useRef, useState } from "react";
import CampaignForm from "./components/form";
import { history } from "@umijs/max";

const Index: React.FC = () => {

    const [openForm, setOpenForm] = useState<boolean>(false);
    const actionRef = useRef<ActionType>(null);
    const [selectedRow, setSelectedRow] = useState<any>(null);

    return (
        <PageContainer extra={<Button icon={<PlusOutlined />} type="primary" onClick={() => setOpenForm(true)}>Thêm mới</Button>}>
            <ProTable
                actionRef={actionRef}
                request={apiCampaignList}
                rowKey={"id"}
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
                        title: 'Mã chiến dịch',
                        dataIndex: 'code',
                        search: false,
                        width: 150
                    },
                    {
                        title: 'Tên chiến dịch',
                        dataIndex: 'name'
                    },
                    {
                        title: 'Số voucher',
                        dataIndex: 'voucherCount',
                        search: false,
                        width: 100,
                        valueType: 'digit'
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'status',
                        valueEnum: {
                            0: { text: 'Không hoạt động', status: 'Default' },
                            1: { text: 'Hoạt động', status: 'Processing' },
                            2: { text: 'Hoàn thành', status: 'Success' }
                        },
                        width: 150,
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 50,
                        render: (text, record) => [
                            <Dropdown key={`more`} menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        icon: <EditOutlined />,
                                        onClick: () => {
                                            setSelectedRow(record);
                                            setOpenForm(true);
                                        }
                                    },
                                    {
                                        key: 'voucher',
                                        label: 'Voucher',
                                        icon: <TagsOutlined />,
                                        onClick: () => {
                                            history.push(`/event/campaign/voucher/${record.id}`);
                                        }
                                    }
                                ]
                            }}>
                                <Button type="dashed" size="small" icon={<MoreOutlined />} />
                            </Dropdown>,
                            <Popconfirm key={"delete"} title="Bạn có chắc chắn muốn xóa chiến dịch này?" okText="Xóa" cancelText="Hủy">
                                <Button type="primary" size="small" danger icon={<DeleteOutlined />}></Button>
                            </Popconfirm>
                        ]
                    }
                ]}
            />
            <CampaignForm open={openForm} onOpenChange={setOpenForm} data={selectedRow} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default Index;