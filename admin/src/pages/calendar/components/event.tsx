import LeadStatusRender from "@/components/lead/status-render";
import { apiCalendarEvents } from "@/services/calendar";
import { LeadStatus } from "@/utils/constants";
import { ActionType, ProTable } from "@ant-design/pro-components";
import { useModel } from "@umijs/max";
import { Drawer, DrawerProps, Tag } from "antd";
import { Dayjs } from "dayjs";
import { useEffect, useRef } from "react";

type Props = DrawerProps & {
    selectedDate?: Dayjs | null;
}

const Event: React.FC<Props> = (props) => {

    const { initialState } = useModel("@@initialState");
    const actionRef = useRef<ActionType>();

    useEffect(() => {
        if (props.open && props.selectedDate) {
            actionRef.current?.reload?.();
        }
    }, [props.open, props.selectedDate]);

    return (
        <>
            <Drawer {...props} title={`Sự kiện ${props.selectedDate ? props.selectedDate.format("YYYY-MM-DD") : ""}`} width={800}>
                <ProTable
                    headerTitle={`Danh sách khách mời`}
                    actionRef={actionRef}
                    size="small"
                    request={(params) => apiCalendarEvents({
                        ...params,
                        date: props.selectedDate ? props.selectedDate.format("YYYY-MM-DD") : undefined,
                        branchId: initialState?.currentUser?.branchId
                    })}
                    search={false}
                    ghost
                    columns={[
                        {
                            title: "#",
                            valueType: "indexBorder",
                            width: 30
                        },
                        {
                            title: 'Họ và tên',
                            dataIndex: 'name',
                            minWidth: 150,
                        },
                        {
                            title: 'Thời gian',
                            dataIndex: 'eventName',
                            search: false,
                            width: 100
                        },
                        {
                            title: 'Key-In',
                            dataIndex: 'keyInName',
                            search: false
                        },
                        {
                            title: 'Trạng thái',
                            dataIndex: 'status',
                            search: false,
                            render: (_, record) => <LeadStatusRender status={record.status} />,
                            width: 100
                        }
                    ]}
                />
            </Drawer>
        </>
    );
};

export default Event;