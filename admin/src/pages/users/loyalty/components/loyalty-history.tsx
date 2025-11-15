import { apiGetMyTransactions } from "@/services/user";
import { ActionType, ProColumns, ProTable } from "@ant-design/pro-components";
import { Drawer } from "antd";
import { DrawerProps } from "antd/lib";
import { useEffect, useRef } from "react";

type Props = DrawerProps

const LoyaltyHistory: React.FC<Props> = (props) => {

    const actionRef = useRef<ActionType>();

    useEffect(() => {
        if (props.open && props.id) {
            actionRef.current?.reload();
        }
    }, [props.id])

    const columns: ProColumns<any>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
            width: 50,
            align: 'center'
        },
        {
            title: 'Ngày',
            dataIndex: 'createdDate',
            valueType: 'dateTime',
            search: false,
            width: 180
        },
        {
            title: 'Điểm',
            dataIndex: 'point',
            valueType: 'digit',
            search: false,
            width: 100
        },
        {
            title: 'Ghi chú',
            dataIndex: 'memo'
        },
        {
            title: 'Feedback của khách',
            dataIndex: 'feedback'
        }
    ]

    return (
        <Drawer title="Lịch sử giao dịch" {...props} width={1000}>
            <ProTable
                actionRef={actionRef}
                ghost
                search={false}
                request={() => apiGetMyTransactions(props.id)}
                columns={columns}
            />
        </Drawer>
    )
}

export default LoyaltyHistory;