import { apiCheckoutTable, apiGetTableOptions } from "@/services/contact";
import { apiTableAll } from "@/services/settings/table";
import { AuditOutlined } from "@ant-design/icons";
import { useRequest } from "@umijs/max";
import { Button, Divider, Drawer, message, Popconfirm } from "antd";
import { useEffect, useState } from "react";

type Props = & {
    eventId?: string;
    eventDate: string;
}

const TableComponent: React.FC<Props> = ({ eventDate, eventId }) => {

    const [open, setOpen] = useState<boolean>(false);
    const { data, refresh } = useRequest(() => apiTableAll({ eventId, eventDate }));

    useEffect(() => {
        if (eventId && eventDate && open) {
            refresh();
        }
    }, [eventId, eventDate, open]);

    const onCheckout = async (values: any) => {
        const body = {
            tableId: values.tableId,
            eventDate,
            eventId
        }
        await apiCheckoutTable(body);
        message.success('Checkout thành công!');
        refresh();
    }

    return (
        <>
            <Button icon={<AuditOutlined />} onClick={() => setOpen(true)}>Bàn</Button>
            <Drawer open={open} onClose={() => setOpen(false)} title="Bàn" width={1000}>
                <div className="flex gap-2 mb-4">
                    <div className="w-4 h-4 bg-green-500"></div> Bàn có sẵn
                    <div className="w-4 h-4 bg-red-500"></div> Bàn bận
                </div>
                {
                    data && data.map((room: any) => (
                        <>
                        <Divider>{room.roomName}</Divider>
                        <div className="grid grid-cols-4 md:grid-cols-8 gap-2 mb-6">
                            {
                                room.tables.map((x: any) => (
                                    <Popconfirm key={x.tableId} title="Xác nhận checkout bàn?" onConfirm={() => onCheckout(x)}>
                                        <div
                                            className={`cursor-pointer hover:opacity-75 shadow rounded px-1 h-10 flex items-center justify-center text-white font-medium ${x.disabled ? 'bg-red-500' : 'bg-green-500'}`}>
                                            {x.tableName}
                                        </div>
                                    </Popconfirm>
                                ))
                            }
                            </div>
                        </>
                    ))
                }
            </Drawer>
        </>
    )
}

export default TableComponent;