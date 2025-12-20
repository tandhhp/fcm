import { apiContractServiceUsageDelete, apiContractServiceUsageList } from "@/services/finances/contract-service-usage";
import { DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { ActionType, ProTable } from "@ant-design/pro-components";
import { Button, Drawer, message, Popconfirm } from "antd";
import { useRef, useState } from "react";
import ServiceUsageForm from "./service-usage-form";

interface ServiceUsageListProps {
    open: boolean;
    onOpenChange: (open: boolean) => void;
    data: any;
}

const ServiceUsageList: React.FC<ServiceUsageListProps> = ({ open, onOpenChange, data }) => {
    const actionRef = useRef<ActionType>();
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [record, setRecord] = useState<API.ContractServiceUsage | null>(null);

    const onDelete = async (id: string) => {
        await apiContractServiceUsageDelete(id);
        actionRef.current?.reload();
        message.success('Xóa phiếu sử dụng dịch vụ thành công');
    }

    return (
        <Drawer
            title="Danh sách phiếu sử dụng dịch vụ quà tặng"
            open={open}
            onClose={() => onOpenChange(false)}
            width={1000}
        >
            <ProTable<API.ContractServiceUsage>
                headerTitle={<Button type="primary" icon={<PlusOutlined />} onClick={() => {
                    setRecord(null);
                    setOpenForm(true);
                }}>Tạo phiếu</Button>}
                actionRef={actionRef}
                request={(params) => apiContractServiceUsageList(data?.id, params)}
                rowKey="id"
                search={false}
                ghost
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 50,
                        align: 'center'
                    },
                    {
                        title: 'Tên dịch vụ',
                        dataIndex: 'serviceName',
                    },
                    {
                        title: 'Ngày sử dụng',
                        dataIndex: 'usedDate',
                        valueType: 'date',
                    },
                    {
                        title: 'Số tiền',
                        dataIndex: 'amount',
                        valueType: 'digit',
                    },
                    {
                        title: 'Số người',
                        dataIndex: 'peopleCount',
                        valueType: 'digit',
                    },
                    {
                        title: 'Ngày tạo',
                        dataIndex: 'createdDate',
                        valueType: 'dateTime',
                    },
                    {
                        title: 'Thao tác',
                        valueType: 'option',
                        width: 100,
                        align: 'center',
                        render: (_, record) => [
                            <Button
                                key="edit"
                                type="link"
                                size="small"
                                onClick={() => {
                                    setRecord(record);
                                    setOpenForm(true);
                                }}
                            >
                                Sửa
                            </Button>,
                            <Popconfirm
                                key="delete"
                                title="Xóa phiếu này?"
                                onConfirm={() => onDelete(record.id)}
                            >
                                <Button type="link" danger size="small" icon={<DeleteOutlined />} />
                            </Popconfirm>
                        ]
                    }
                ]}
                pagination={{
                    pageSize: 10,
                }}
            />
            <ServiceUsageForm
                open={openForm}
                onOpenChange={setOpenForm}
                data={data}
                record={record}
                reload={() => actionRef.current?.reload()}
            />
        </Drawer>
    );
}

export default ServiceUsageList;
