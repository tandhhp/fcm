import { apiEventOptions } from "@/services/event";
import { apiLeadWaitingList } from "@/services/users/lead";
import { CheckOutlined, DeleteOutlined, EditOutlined, ManOutlined, MoreOutlined, PlusOutlined, SettingOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, ProTable } from "@ant-design/pro-components";
import { useRef, useState } from "react";
import { Button, Dropdown, message, Popconfirm } from "antd";
import { useAccess } from "@umijs/max";
import { apiDeleteLead, apiUpdateLeadStatus } from "@/services/contact";
import { LeadStatus } from "@/utils/constants";
import LeadForm from "@/components/form/lead-form";

const WaitingList: React.FC = () => {

    const access = useAccess();
    const [keyInOpen, setKeyInOpen] = useState<boolean>(false);
    const [selectedRecord, setSelectedRecord] = useState<any>(null);
    const actionRef = useRef<ActionType>();

    const onApprove = async (id: string) => {
        await apiUpdateLeadStatus({
            id: id,
            status: LeadStatus.Approved
        });
        message.success('Thành công');
        actionRef.current?.reload();
    }

    return (
        <>
            <ProTable
                rowKey={"id"}
                search={{
                    layout: 'vertical'
                }}
                actionRef={actionRef}
                headerTitle={<Button type="primary" onClick={() => {
                    setSelectedRecord(null);
                    setKeyInOpen(true);
                }} icon={<PlusOutlined />} disabled={access.canAdmin || access.telesale || access.telesaleManager || access.dot || access.cx}>Tạo Key-In</Button>}
                request={apiLeadWaitingList}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'name',
                        render: (dom, entity) => {
                            if (entity.gender === true) {
                                return <><WomanOutlined className="text-red-500 mr-2" />{dom}</>
                            }
                            if (entity.gender === false) {
                                return <><ManOutlined className="text-blue-500 mr-2" />{dom}</>
                            }
                            return <>{dom}</>
                        }
                    },
                    {
                        title: 'SDT',
                        dataIndex: 'phoneNumber'
                    },
                    {
                        title: 'Khách phụ',
                        dataIndex: 'subLeads',
                        valueType: 'select',
                        search: false
                    },
                    {
                        title: 'Rep',
                        dataIndex: 'salesName',
                        search: false
                    },
                    {
                        title: 'Key-In',
                        dataIndex: 'creatorName',
                        search: false
                    },
                    {
                        title: 'Ngày sự kiện',
                        dataIndex: 'eventDate',
                        valueType: 'date'
                    },
                    {
                        title: 'Khung giờ',
                        dataIndex: 'eventName',
                        search: false
                    },
                    {
                        title: 'Nguồn',
                        dataIndex: 'sourceName',
                        search: false
                    },
                    {
                        dataIndex: 'eventId',
                        valueType: 'select',
                        request: apiEventOptions,
                        hideInTable: true,
                        title: 'Sự kiện'
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'status',
                        valueEnum: {
                            0: { text: 'Chờ duyệt', status: 'Default' },
                            1: { text: 'Đã duyệt', status: 'Success' },
                            5: { text: 'Mời lại', status: 'Warning' }
                        },
                        search: false
                    },
                    {
                        title: 'Ngày tạo',
                        dataIndex: 'createdDate',
                        valueType: 'date',
                        search: false,
                        width: 100,
                        minWidth: 100
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 60,
                        align: 'center',
                        render: (dom, entity) => [
                            <Popconfirm key={"approve"} title="Duyệt khách hàng này?" onConfirm={() => onApprove(entity.id)}>
                                <Button type="primary" size="small" icon={<CheckOutlined />} hidden={!access.canSm && !access.event} disabled={entity.status === LeadStatus.Approved} />
                            </Popconfirm>,
                            <Dropdown key={"more"} menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        icon: <EditOutlined />,
                                        onClick: () => {
                                            setSelectedRecord(entity);
                                            setKeyInOpen(true);
                                        },
                                        disabled: entity.status === LeadStatus.Approved && (access.sales || access.telesale || access.cx)
                                    }
                                ]
                            }}>
                                <Button size="small" icon={<MoreOutlined />} />
                            </Dropdown>,
                            <Popconfirm key={"delete"} title="Xóa khách hàng này?" onConfirm={async () => {
                                await apiDeleteLead(entity.id);
                                message.success('Đã xóa');
                                actionRef.current?.reload();
                            }}>
                                <Button type="primary" danger size="small" icon={<DeleteOutlined />} disabled={entity.status === LeadStatus.Approved && !access.canAdmin && !access.event} />
                            </Popconfirm>
                        ]
                    }
                ]}
            />
            <LeadForm open={keyInOpen} onOpenChange={setKeyInOpen} reload={() => actionRef.current?.reload()} data={selectedRecord} />
        </>
    )
}

export default WaitingList;