import { apiCallStatusDelete, apiCallStatusList, apiCallTMRReport } from "@/services/call";
import { ActionType, PageContainer, ProCard, ProTable } from "@ant-design/pro-components"
import { useRef, useState } from "react";
import CallStatusForm from "./components/form";
import { Button, Dropdown, message, Popconfirm, Statistic } from "antd";
import { DeleteOutlined, EditOutlined, MoreOutlined, PlusOutlined, SettingOutlined } from "@ant-design/icons";
import { useAccess, useRequest } from "@umijs/max";

const Index: React.FC = () => {

    const access = useAccess();
    const actionRef = useRef<ActionType>();
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [selectedRow, setSelectedRow] = useState<any>(null);

    const { data } = useRequest(apiCallTMRReport);

    const onDelete = async (id: number) => {
        await apiCallStatusDelete(id);
        actionRef.current?.reload();
        message.success('Xoá trạng thái cuộc gọi thành công');
    }

    return (
        <PageContainer extra={<Button type="primary" icon={<PlusOutlined />} onClick={() => setOpenForm(true)} disabled={!access.telesaleManager}>Thêm trạng thái cuộc gọi</Button>}>
            
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-4">
                <ProCard title="Tổng liên hệ" headerBordered>
                    <Statistic value={data?.totalContact || 0} />
                </ProCard>
                <ProCard title="Chờ phân công" headerBordered>
                    <Statistic value={data?.totalAvailableAssign || 0} />
                </ProCard>
                <ProCard title="Đã liên hệ" headerBordered>
                    <Statistic value={data?.totalCalled || 0} />
                </ProCard>
                <ProCard title="Chưa liên hệ" headerBordered>
                    <Statistic value={data?.totalNotContacted || 0} />
                </ProCard>
            </div>
            
            <ProTable
                actionRef={actionRef}
                request={apiCallStatusList}
                rowKey={`id`}
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
                        title: 'Trạng thái cuộc gọi',
                        dataIndex: 'name',
                    },
                    {
                        dataIndex: 'callCount',
                        title: 'Số cuộc gọi',
                        search: false,
                        valueType: 'digit'
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 80,
                        render: (text, record) => [
                            <Dropdown key={"more"} menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        icon: <EditOutlined />,
                                        onClick: () => {
                                            setSelectedRow(record);
                                            setOpenForm(true);
                                        },
                                        disabled: !access.telesaleManager
                                    }
                                ]
                            }}>
                                <Button type="dashed" icon={<MoreOutlined />} size="small" />
                            </Dropdown>,
                            <Popconfirm key={"delete"} title="Xác nhận xoá trạng thái cuộc gọi?" onConfirm={() => onDelete(record.id)}>
                                <Button type="primary" danger size="small" icon={<DeleteOutlined />} disabled={!access.telesaleManager} />
                            </Popconfirm>
                        ]
                    }
                ]}
            />
            <CallStatusForm open={openForm} onOpenChange={setOpenForm} reload={() => actionRef.current?.reload()} data={selectedRow} />
        </PageContainer>
    )
}

export default Index;