import { apiCallWebhookLogs } from "@/services/call";
import { apiContactDetailsByPhone } from "@/services/contact";
import { DrawerForm, DrawerFormProps, ProCard, ProDescriptions, ProTable } from "@ant-design/pro-components";
import { Alert, Button, Col, Empty, Row, Spin, Tag } from "antd";
import { useEffect, useState } from "react";

type Props = DrawerFormProps & {
    lead?: {
        phoneNumber: string;
        name: string;
    };
}

type CallHistory = {
    id: string;
    note?: string;
    age?: number;
    extraStatus?: string;
    createdDate?: string;
    caller?: string;
    status?: string;
};

type ContactDetails = {
    id: string;
    name?: string;
    phoneNumber?: string;
    email?: string;
    address?: string;
    confirm1?: boolean;
    note?: string;
    createdDate?: string;
    callHistories?: CallHistory[];
    CallHistories?: CallHistory[];
};

const LeadDetails: React.FC<Props> = ({ lead, ...rest }) => {

    const { open, ...drawerProps } = rest;

    const [contact, setContact] = useState<ContactDetails | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>('');

    useEffect(() => {
        let cancelled = false;

        const fetchContactDetails = async () => {
            if (!open || !lead?.phoneNumber) {
                setContact(null);
                setError('');
                return;
            }

            setLoading(true);
            setError('');
            try {
                const response = await apiContactDetailsByPhone(lead.phoneNumber);
                if (cancelled) return;

                if (response?.succeeded === false) {
                    setContact(null);
                    return;
                }

                const payload: any = response?.data ?? response;
                setContact({
                    ...payload,
                    callHistories: payload?.callHistories ?? payload?.CallHistories ?? []
                });
            } catch (err: any) {
                if (cancelled) return;
                setContact(null);
            } finally {
                if (!cancelled) {
                    setLoading(false);
                }
            }
        };

        fetchContactDetails();

        return () => {
            cancelled = true;
        };
    }, [lead?.phoneNumber, open]);

    const callHistories = contact?.callHistories ?? contact?.CallHistories ?? [];

    return (
        <DrawerForm submitter={false} open={open} {...drawerProps} title={`Thông tin chi tiết của ${lead?.name || 'khách hàng'}`} width={1000}>
            <Spin spinning={loading}>
                {contact ? (
                    <Row gutter={[16, 16]}>
                        <Col xs={24} md={24}>
                            <ProDescriptions
                                title="Thông tin liên hệ"
                                column={1} bordered size="small">
                                <ProDescriptions.Item label="Họ và tên">{contact.name || '--'}</ProDescriptions.Item>
                                <ProDescriptions.Item label="Số điện thoại">{contact.phoneNumber || lead?.phoneNumber || '--'}</ProDescriptions.Item>
                                <ProDescriptions.Item label="Email">{contact.email || '--'}</ProDescriptions.Item>
                                <ProDescriptions.Item label="Địa chỉ">{contact.address || '--'}</ProDescriptions.Item>
                                <ProDescriptions.Item label="Xác nhận 1">
                                    {contact.confirm1 ? <Tag color="success">Đã xác nhận</Tag> : <Tag>Chưa xác nhận</Tag>}
                                </ProDescriptions.Item>
                                <ProDescriptions.Item label="Ngày tạo" valueType="dateTime">
                                    {contact.createdDate || '--'}
                                </ProDescriptions.Item>
                                <ProDescriptions.Item label="Ghi chú">{contact.note || '--'}</ProDescriptions.Item>
                            </ProDescriptions>
                        </Col>
                        <Col xs={24} md={24}>
                            <ProTable<CallHistory>
                                className="mb-4"
                                headerTitle="Lịch sử cuộc gọi"
                                search={false}
                                ghost
                                options={false}
                                dataSource={callHistories}
                                rowKey="id"
                                pagination={{
                                    pageSize: 6,
                                    showSizeChanger: false
                                }}
                                locale={{
                                    emptyText: <Empty description="Chưa có lịch sử cuộc gọi" />
                                }}
                                columns={[
                                    {
                                        title: '#',
                                        valueType: 'indexBorder',
                                        width: 40,
                                        align: 'center'
                                    },
                                    {
                                        title: 'Ngày gọi',
                                        dataIndex: 'createdDate',
                                        valueType: 'dateTime'
                                    },
                                    {
                                        title: 'Người gọi',
                                        dataIndex: 'caller'
                                    },
                                    {
                                        title: 'Trạng thái',
                                        dataIndex: 'status'
                                    },
                                    {
                                        title: 'Trạng thái bổ sung',
                                        dataIndex: 'extraStatus'
                                    },
                                    {
                                        title: 'Tuổi',
                                        dataIndex: 'age',
                                        width: 70
                                    },
                                    {
                                        title: 'Ghi chú',
                                        dataIndex: 'note',
                                        ellipsis: true
                                    }
                                ]}
                            />
                        </Col>
                    </Row>
                ) : (!loading && !error ? <Empty description="Không có dữ liệu liên hệ" /> : null)}
            </Spin>
            <ProTable
                headerTitle="Nhật ký cuộc gọi"
                request={apiCallWebhookLogs}
                params={{
                    fromNumber: lead?.phoneNumber
                }}
                search={false}
                rowKey="id"
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                    },
                    {
                        dataIndex: 'timeStarted',
                        title: 'Thời gian bắt đầu',
                        valueType: 'dateTime',
                    },
                    {
                        dataIndex: 'timeEnded',
                        title: 'Thời gian kết thúc',
                        valueType: 'dateTime',
                    },
                    {
                        dataIndex: 'timeAnswered',
                        title: 'Thời gian trả lời',
                        valueType: 'dateTime',
                    },
                    {
                        dataIndex: 'status',
                        title: 'Trạng thái',
                    },
                    {
                        dataIndex: 'recordingUrl',
                        title: 'Bản ghi âm',
                        render: (text, record) => text ? <Button type="primary" size="small" href={record.recordingUrl} target="_blank"
                            rel="noopener noreferrer">
                            Nghe ghi âm
                        </Button> : '--'
                    }
                ]}
                ghost
                size="small"
            />
        </DrawerForm>
    )
}

export default LeadDetails;