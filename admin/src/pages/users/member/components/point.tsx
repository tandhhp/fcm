import { apiUserPointByCardHolder } from "@/services/user";
import { ActionType, ProTable } from "@ant-design/pro-components";
import { Drawer, DrawerProps } from "antd"
import { useEffect, useRef } from "react";

type Props = DrawerProps & {
    cardHolder?: any;
}

const PointDrawer: React.FC<Props> = (props) => {

    const actionRef = useRef<ActionType>();
    useEffect(() => {
        if (props.open) {
            actionRef.current?.reload();
        }
    }, [props.cardHolder, props.open]);

    return (
        <Drawer {...props} title={`Điểm ${props.cardHolder?.name}`} width={600} destroyOnClose>
            <ProTable
                actionRef={actionRef}
                rowKey="id"
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Ngày nạp',
                        dataIndex: 'createdDate',
                        valueType: 'date',
                        width: 150,
                    },
                    {
                        title: 'Điểm',
                        dataIndex: 'point',
                    },
                    {
                        title: 'Ngày hết hạn',
                        dataIndex: 'dueDate',
                        valueType: 'date',
                    },
                    {
                        title: 'Nạp bởi',
                        dataIndex: 'topupBy',
                    }
                ]}
                ghost
                request={(prams) => apiUserPointByCardHolder({
                    cardHolderId: props.cardHolder?.id,
                    ...prams
                })}
                search={false}
            />
        </Drawer>
    )
}

export default PointDrawer;