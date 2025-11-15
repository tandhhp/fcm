import { apiApproveTopup, apiListTopup } from "@/services/user";
import { CheckOutlined, CloseOutlined, EyeOutlined, MailOutlined, PhoneOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { Button, message, Popconfirm, Popover, Tag, Tooltip } from "antd";
import dayjs from "dayjs";
import { useRef } from "react";

const TopupPage: React.FC = () => {

    const actionRef = useRef<ActionType>();

    return (
        <PageContainer>
            <ProTable
                scroll={{
                    x: true
                }}
                search={{
                    layout: 'vertical'
                }}
                request={apiListTopup}
                actionRef={actionRef}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Chủ thẻ',
                        dataIndex: 'name',
                        render: (_, entity) => entity.cardHolderName
                    },
                    {
                        title: 'Liên hệ',
                        dataIndex: 'phoneNumber',
                        render: (_, entity) => (
                            <>
                                <div><MailOutlined /> {entity.email}</div>
                                <div><PhoneOutlined /> {entity.phoneNumber}</div>
                            </>
                        )
                    },
                    {
                        title: 'Số tiền',
                        dataIndex: 'amount',
                        valueType: 'digit',
                        search: false
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note',
                        search: false,
                        render: (_, entity) => (
                            <Popover title="Ghi chú" content={
                                <div className="md:w-96 w-full">
                                    <div dangerouslySetInnerHTML={{ __html: entity.note }} />
                                </div>
                            }>
                                <Button type="primary" icon={<EyeOutlined />} size="small" />
                            </Popover>
                        ),
                        align: 'center'
                    },
                    {
                        title: 'Giám đốc',
                        dataIndex: 'directorName',
                        search: false,
                        render: (_, entity) => (
                            <>
                                <div>{entity.directorName}</div>
                                <div>{entity.directorApprovedDate && dayjs(entity.directorApprovedDate).format('DD/MM/YYYY hh:mm:ss')}</div>
                            </>
                        )
                    },
                    {
                        title: 'Kế toán',
                        dataIndex: 'accountantName',
                        search: false,
                        render: (_, entity) => (
                            <>
                                <div>{entity.accountantName}</div>
                                <div>{entity.accountantApprovedDate && dayjs(entity.accountantApprovedDate).format('DD/MM/YYYY hh:mm:ss')}</div>
                            </>
                        )
                    },
                    {
                        title: 'Loại',
                        dataIndex: 'type',
                        valueEnum: {
                            0: <Tag color="green" className="w-16 text-center">Top-Up</Tag>,
                            1: <Tag color="blue" className="w-16 text-center">Công nợ</Tag>
                        },
                        search: false
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'status',
                        valueEnum: {
                            0: 'Đang chờ',
                            1: 'GD phê duyệt',
                            2: 'KT phê duyệt'
                        },
                        search: false
                    },
                    {
                        title: 'Tác vụ',
                        valueType: 'option',
                        render: (_, entity) => [
                            <Popconfirm key="approve" title="Xác nhận phê duyệt" onConfirm={async () => {
                                let status = 0;
                                if (entity.status === 0) {
                                    status = 1;
                                }
                                if (entity.status === 1) {
                                    status = 2
                                }
                                await apiApproveTopup({
                                    id: entity.id,
                                    status
                                });
                                message.success('Phê duyệt thành công!');
                                actionRef.current?.reload();
                            }}>
                                <Tooltip title="Phê duyệt">
                                    <Button type="primary" size="small" icon={<CheckOutlined />} hidden={entity.status === 2} />
                                </Tooltip>
                            </Popconfirm>,
                            <Popconfirm key="reject" title="Xác nhận  từ chối">
                                <Tooltip title="Từ chối">
                                    <Button type="primary" danger size="small" icon={<CloseOutlined />} hidden={entity.status === 2} />
                                </Tooltip>
                            </Popconfirm>
                        ],
                        width: 60
                    }
                ]}
            />
        </PageContainer>
    )
}

export default TopupPage;