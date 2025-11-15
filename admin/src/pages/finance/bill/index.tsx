import { apiBillApprove, apiBillList, apiBillReject } from "@/services/finances/bill";
import { BillStatus } from "@/utils/enum";
import { CheckOutlined, CloseOutlined, EditOutlined, MoreOutlined, SettingOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { useAccess } from "@umijs/max";
import { Button, Dropdown, message, Popconfirm } from "antd";
import BillForm from "./components/form";
import { useRef, useState } from "react";

const Index: React.FC = () => {

    const access = useAccess();
    const actionRef = useRef<ActionType>();
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [selectedRow, setSelectedRow] = useState<any>();

    const onApprove = async (id: string) => {
        await apiBillApprove(id);
        message.success("Duyệt phiếu chi thành công");
        actionRef.current?.reload();
    }

    const onReject = async (id: string) => {
        await apiBillReject(id);
        message.success("Từ chối phiếu chi thành công");
        actionRef.current?.reload();
    }

    return (
        <PageContainer>
            <ProTable
                actionRef={actionRef}
                rowKey={"id"}
                request={apiBillList}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Số HĐ',
                        dataIndex: 'contractCode',
                        search: false
                    },
                    {
                        title: 'Tên phiếu chi',
                        dataIndex: 'name'
                    },
                    {
                        title: 'Số phiếu chi',
                        dataIndex: 'billNumber'
                    },
                    {
                        title: 'Số tiền',
                        dataIndex: 'amount',
                        valueType: 'digit',
                        search: false
                    },
                    {
                        title: 'Ngày tạo',
                        dataIndex: 'createdDate',
                        valueType: 'date',
                        search: false
                    },
                    {
                        title: 'Tạo bởi',
                        dataIndex: 'createdBy',
                        search: false
                    },
                    {
                        title: 'Ngày duyệt',
                        dataIndex: 'approvedAt',
                        valueType: 'date',
                        search: false
                    },
                    {
                        title: 'Người duyệt',
                        dataIndex: 'approvedBy',
                        search: false
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note',
                        search: false
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 50,
                        render: (_, record) => [
                            <Popconfirm key={"approve"} title="Xác nhận duyệt phiếu chi?" onConfirm={() => onApprove(record.id)} okText="Duyệt" cancelText="Hủy">
                                <Button type="primary" icon={<CheckOutlined />} size="small" disabled={!access.accountant || record.status !== BillStatus.Pending}></Button>
                            </Popconfirm>,
                            <Popconfirm key={"reject"} title="Xác nhận từ chối phiếu chi?" onConfirm={() => onReject(record.id)} okText="Từ chối" cancelText="Hủy">
                                <Button type="primary" icon={<CloseOutlined />} size="small" disabled={!access.accountant || record.status !== BillStatus.Pending}></Button>
                            </Popconfirm>,
                            <Dropdown key={"more"} menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        icon: <EditOutlined />,
                                        disabled: record.status !== BillStatus.Pending,
                                        onClick: () => {
                                            setSelectedRow(record);
                                            setOpenForm(true);
                                        }
                                    }
                                ]
                            }}>
                                <Button icon={<MoreOutlined />} size="small" />
                            </Dropdown>
                        ]
                    }
                ]}
                search={{
                    layout: 'vertical'
                }}
            />
            <BillForm open={openForm} onOpenChange={setOpenForm} reload={() => actionRef.current?.reload()} data={selectedRow} />
        </PageContainer>
    )
}

export default Index;