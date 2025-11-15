import { apiLeadLeadHistory } from "@/services/contact";
import { apiEventOptions } from "@/services/event";
import { apiAttendanceOptions } from "@/services/event/attendance";
import { ManOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, ProTable } from "@ant-design/pro-components";
import { Drawer, DrawerProps } from "antd"
import { useEffect, useRef } from "react";

type Props = DrawerProps & {
    leadId?: string;
}

const ReinviteHistories: React.FC<Props> = (props) => {

    const actionRef = useRef<ActionType>();

    useEffect(() => {
        if (props.open) {
            actionRef.current?.reload();
        }
    }, [props.leadId, props.open]);

    return (
        <Drawer title="Lịch sử mời" {...props} width={1100} footer={false}>
            <ProTable
                request={(params) => apiLeadLeadHistory(params, props.leadId)}
                actionRef={actionRef}
                search={false}
                rowKey="id"
                scroll={{
                    x: true
                }}
                ghost
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'name',
                        render: (dom, entity) => {
                            if (entity.gender === true) {
                                return <><WomanOutlined className="text-red-500 mr-1" />{dom}</>
                            }
                            if (entity.gender === false) {
                                return <><ManOutlined className="text-blue-500 mr-1" />{dom}</>
                            }
                            return dom;
                        },
                        width: 160,
                        minWidth: 160
                    },
                    {
                        title: 'SDT',
                        dataIndex: 'phoneNumber',
                        width: 120,
                        minWidth: 120
                    },
                    {
                        title: 'Rep',
                        dataIndex: 'salesName',
                        width: 160,
                        minWidth: 160
                    },
                    {
                        title: 'Người T.O',
                        dataIndex: 'toName',
                        width: 160,
                        minWidth: 160
                    },
                    {
                        title: 'Ngày mời',
                        dataIndex: 'eventDate',
                        valueType: 'date',
                        width: 100,
                        minWidth: 100
                    },
                    {
                        title: 'Khung giờ',
                        dataIndex: 'eventId',
                        valueType: 'select',
                        request: apiEventOptions,
                        width: 100,
                        minWidth: 100
                    },
                    {
                        title: 'Check-in',
                        dataIndex: 'checkinTime',
                        valueType: 'time',
                        width: 100,
                        minWidth: 100
                    },
                    {
                        title: 'Check-out',
                        dataIndex: 'checkoutTime',
                        valueType: 'time',
                        width: 100,
                        minWidth: 100
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'attendanceId',
                        request: apiAttendanceOptions,
                        valueType: 'select',
                        width: 120,
                        minWidth: 120
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note',
                        width: 200,
                        minWidth: 200
                    }
                ]}
            />
        </Drawer>
    )
}

export default ReinviteHistories;