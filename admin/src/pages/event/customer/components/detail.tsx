import { apiContractList } from "@/services/finances/contract";
import { apiAttendanceOptions } from "@/services/event/attendance";
import { apiLeadCheckinList, apiLeadDetail } from "@/services/users/lead";
import { ModalForm, ModalFormProps, ProDescriptions, ProTable } from "@ant-design/pro-components";
import { useEffect, useState } from "react";

type Props = ModalFormProps & {
    data?: any;
}

export const LeadDetail: React.FC<Props> = (props) => {

    const [data, setData] = useState<any>();

    useEffect(() => {
        if (props.data && props.open) {
            apiLeadDetail(props.data.id).then(res => {
                setData(res.data);
            });
        }
    }, [props.data, props.open]);

    return (
        <ModalForm {...props} title={`Chi tiết khách hàng ${data?.name}`} submitter={false} width={1000} clearOnDestroy>
            <ProDescriptions column={2} bordered size="small" className="mb-4">
                <ProDescriptions.Item label="Họ và tên">{data?.name}</ProDescriptions.Item>
                <ProDescriptions.Item label="số điện thoại">{data?.phoneNumber}</ProDescriptions.Item>
                <ProDescriptions.Item label="Số CCCD">{data?.identityNumber}</ProDescriptions.Item>
                <ProDescriptions.Item label="Ngày sinh" valueType="date">{data?.dateOfBirth}</ProDescriptions.Item>
            </ProDescriptions>
            <ProTable
                loading={!data}
                className="mb-4"
                request={apiLeadCheckinList}
                params={{
                    phoneNumber: data?.phoneNumber
                }}
                rowKey="id"
                search={false}
                size="small"
                ghost
                rowClassName={(record) => (record.id === props.data?.id ? 'hidden' : '')}
                headerTitle="Danh sách sự kiện đã tham gia"
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Ngày sự kiện',
                        dataIndex: 'eventDate',
                        valueType: 'date'
                    },
                    {
                        title: 'Khung giờ',
                        dataIndex: 'eventName'
                    },
                    {
                        title: 'Nguồn',
                        dataIndex: 'sourceName'
                    },
                    {
                        title: 'Key-In',
                        dataIndex: 'creatorName'
                    },
                    {
                        title: 'Rep',
                        dataIndex: 'salesName'
                    },
                    {
                        title: 'T.O',
                        dataIndex: 'toName'
                    },
                    {
                        title: 'TT Tham dự',
                        dataIndex: 'attendanceId',
                        valueType: 'select',
                        request: apiAttendanceOptions
                    }
                ]}
            />
            <ProTable
                headerTitle="Hợp đồng khách hàng"
                request={apiContractList}
                params={{
                    leadId: data?.id
                }}
                loading={!data}
                rowKey="id"
                search={false}
                ghost
                size="small"
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Số hợp đồng',
                        dataIndex: 'contractCode'
                    },
                    {
                        title: 'Ngày',
                        dataIndex: 'createdDate',
                        valueType: 'date',
                        width: 100
                    },
                    {
                        title: 'Rep',
                        dataIndex: 'salesName'
                    },
                    {
                        title: 'T.O',
                        dataIndex: 'toByName',
                        width: 150
                    },
                    {
                        title: 'GTHĐ',
                        dataIndex: 'amount',
                        valueType: 'digit'
                    },
                    {
                        title: 'Đã thanh toán',
                        dataIndex: 'paidAmount',
                        valueType: 'digit'
                    },
                    {
                        title: 'Chờ duyệt',
                        dataIndex: 'pendingAmount',
                        valueType: 'digit'
                    },
                    {
                        title: 'Tỷ lệ TT',
                        valueType: 'percent',
                        render: (text, record) => {
                            const total = record.amount || 0;
                            const paid = record.paidAmount || 0;
                            return total > 0 ? (paid / total) * 100 : 0;
                        }
                    }
                ]}
            />
        </ModalForm>
    )
}

export default LeadDetail;