import { apiApproveDeposit, apiLotaltyApproveList, apiRejectDeposit } from "@/services/user";
import { ManOutlined, MoreOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProColumnType, ProTable } from "@ant-design/pro-components"
import { Button, Dropdown, Popconfirm, message } from "antd";
import { useRef, useState } from "react";
import LoyaltyHistory from "./components/loyalty-history";
import { TransactionType } from "@/utils/constants";

const LoyaltyPage: React.FC = () => {

    const actionRef = useRef<ActionType>();
    const [openHistory, setOpenHistory] = useState<boolean>(false);
    const [cardHolderId, setCardHolderId] = useState<string>();

    const columns: ProColumnType<any>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
            width: 30,
        },
        {
            title: 'Mã HĐ',
            dataIndex: 'contractCode',
            search: false,
        },
        {
            title: 'Chủ thẻ',
            dataIndex: 'name',
            render: (dom, entity) => (
                <div>
                    <div
                        onClick={() => {
                            setCardHolderId(entity.cardHolderId);
                            setOpenHistory(true);
                        }}
                        className="font-bold hover:text-blue-500 cursor-pointer">{entity.gender === true && (<ManOutlined className='text-blue-500' />)}{entity.gender === false && (<WomanOutlined className='text-red-500' />)} {dom}</div>
                </div>
            ),
            minWidth: 120
        },
        {
            title: 'SĐT',
            dataIndex: 'phoneNumber'
        },
        {
            title: 'Người yêu cầu',
            dataIndex: 'createdBy',
            search: false,
            minWidth: 120
        },
        {
            title: 'Ngày yêu cầu',
            dataIndex: 'createdDate',
            valueType: 'dateTime',
            width: 200,
            search: false,
            minWidth: 120
        },
        {
            title: 'Điểm',
            dataIndex: 'point',
            valueType: 'digit',
            search: false
        },
        {
            title: 'Trạng thái',
            dataIndex: 'status',
            valueEnum: {
                1: {
                    text: 'Chờ duyệt',
                    status: 'Default',
                },
                2: {
                    text: 'Đã duyệt',
                    status: 'Processing',
                },
                3: {
                    text: 'Từ chối',
                    status: 'error',
                }
            },
            search: false,
            minWidth: 100
        },
        {
            title: 'Nội dung yêu cầu',
            dataIndex: 'memo',
            search: false,
            minWidth: 200
        },
        {
            title: 'Lý do',
            dataIndex: 'reason',
            search: false,
            minWidth: 100
        },
        {
            title: 'Loại',
            dataIndex: 'type',
            valueEnum: {
                [TransactionType.Default]: {
                    text: 'Điểm NP',
                    status: 'Default',
                },
                [TransactionType.Bonus]: {
                    text: 'Điểm thưởng',
                    status: 'Processing',
                },
                [TransactionType.Loan]: {
                    text: 'Điểm vay',
                    status: 'error',
                },
            },
            minWidth: 80
        },
        {
            title: 'Tác vụ',
            valueType: 'option',
            render: (dom, entity) => [
                <Popconfirm key="approve" title="Bạn có chắc chắn muốn phê duyệt yêu cầu này?" onConfirm={() => {
                    apiApproveDeposit(entity.id).then(() => {
                        message.success('Phê duyệt thành công!');
                        actionRef.current?.reload();
                    })
                }}>
                    <Button type="primary" size="small" hidden={entity.status !== 1}>Phê duyệt</Button>
                </Popconfirm>,
                <Popconfirm key="reject" title="Xác nhận từ chối yêu cầu?" onConfirm={() => {
                    apiRejectDeposit(entity.id).then(() => {
                        message.success('Phê duyệt thành công!');
                        actionRef.current?.reload();
                    })
                }}>
                    <Button type="primary" size="small" hidden={entity.status !== 1} danger>Từ chối</Button>
                </Popconfirm>,
                <Dropdown key="more" menu={{
                    items: [
                        {
                            key: 'history',
                            label: 'Lịch sử giao dịch',
                            onClick: () => {
                                setCardHolderId(entity.cardHolderId);
                                setOpenHistory(true);
                            }
                        }
                    ]
                }}>
                    <Button type="dashed" size="small" icon={<MoreOutlined />}></Button>
                </Dropdown>
            ],
            width: 100
        }
    ]

    return (
        <PageContainer>
            <ProTable
                scroll={{
                    x: true
                }}
                actionRef={actionRef}
                search={{
                    layout: 'vertical'
                }}
                request={apiLotaltyApproveList}
                columns={columns}
            />
            <LoyaltyHistory open={openHistory} onClose={() => setOpenHistory(false)} id={cardHolderId} />
        </PageContainer>
    )
}

export default LoyaltyPage;