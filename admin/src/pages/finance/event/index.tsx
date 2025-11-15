import { apiApproveDebt, apiGetLeadFeedback, apiRejectDebt } from "@/services/contact";
import { apiListEventSaleRevenue } from "@/services/event";
import { apiBranchOptions } from "@/services/settings/branch";
import { CheckOutlined, CloseOutlined, MoreOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProFormTextArea, ProTable } from "@ant-design/pro-components"
import { useAccess } from "@umijs/max";
import { Button, Drawer, Dropdown, message } from "antd";
import { useEffect, useRef, useState } from "react";

const Index: React.FC = () => {

    const [openFeedback, setOpenFeedback] = useState<boolean>(false);
    const [keyIn, setKeyIn] = useState<any>();
    const [feedback, setFeedback] = useState<any>();
    const [open, setOpen] = useState<boolean>(false);
    const [isApproved, setIsApproved] = useState<boolean>(false);
    const actionRef = useRef<ActionType>();
    const access = useAccess();

    useEffect(() => {
        if (keyIn) {
            apiGetLeadFeedback(keyIn.keyInId).then((res) => { setFeedback(res); });
        }
    }, [keyIn]);

    const canAction = (status: number) => {
        if (status === 3) return false;
        if (access.dos && status === 0) return true;
        if (access.accountant && status === 1) return true;
        return false;
    }

    return (
        <PageContainer>
            <ProTable
                actionRef={actionRef}
                rowKey="id"
                request={apiListEventSaleRevenue}
                search={{
                    layout: 'vertical'
                }}
                scroll={{ x: true }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Key-In',
                        dataIndex: 'keyInName',
                        search: false,
                        render: (dom, entity) => (
                            <div className="font-medium hover:text-blue-500 text-slate-800 cursor-pointer" onClick={() => {
                                setKeyIn(entity);
                                setOpenFeedback(true);
                            }}>{dom}</div>
                        )
                    },
                    {
                        title: 'Điện thoại',
                        dataIndex: 'keyInPhoneNumber',
                        search: false,
                        width: 90
                    },
                    {
                        title: 'Chi nhánh',
                        dataIndex: 'branch',
                        valueType: 'select',
                        minWidth: 100,
                        search: false,
                        hideInTable: !access.chiefAccountant && !access.canAdmin,
                        request: apiBranchOptions
                    },
                    {
                        title: 'Trợ lý cá nhân',
                        dataIndex: 'saleName'
                    },
                    {
                        title: 'Số tiền',
                        dataIndex: 'amount',
                        valueType: 'digit',
                        search: false
                    },
                    {
                        title: 'Tạo lúc',
                        dataIndex: 'createdDate',
                        valueType: 'fromNow',
                        search: false
                    },
                    {
                        title: 'Tạo bởi',
                        dataIndex: 'createdBy',
                        search: false,
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'status',
                        valueType: 'select',
                        valueEnum: {
                            0: { text: 'Chờ GD duyệt', status: 'Default' },
                            1: { text: 'Chờ KT duyệt', status: 'Processing' },
                            2: { text: 'Đã duyệt', status: 'Success' },
                            3: { text: 'Đã hủy', status: 'Error' }
                        },
                        search: false,
                        width: 120
                    },
                    {
                        title: 'Giám đốc',
                        dataIndex: 'dosName',
                        search: false,
                        hideInTable: access.dos
                    },
                    {
                        title: 'GĐ Duyệt lúc',
                        dataIndex: 'directorApprovedDate',
                        valueType: 'fromNow',
                        search: false
                    },
                    {
                        title: 'Kế toán',
                        dataIndex: 'accountantName',
                        search: false,
                        hideInTable: access.accountant
                    },
                    {
                        title: 'KT Duyệt lúc',
                        dataIndex: 'accountantApprovedDate',
                        valueType: 'fromNow',
                        search: false
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note',
                        search: false
                    },
                    {
                        title: 'Tác vụ',
                        valueType: 'option',
                        render: (_, record) => [
                            <Dropdown key="more" menu={{
                                items: [
                                    {
                                        key: 'approve',
                                        label: 'Phê duyệt',
                                        onClick: () => {
                                            setIsApproved(true);
                                            setKeyIn(record);
                                            setOpen(true);
                                        },
                                        icon: <CheckOutlined />
                                    },
                                    {
                                        key: 'reject',
                                        label: 'Từ chối',
                                        onClick: () => {
                                            setIsApproved(false);
                                            setKeyIn(record);
                                            setOpen(true);
                                        },
                                        icon: <CloseOutlined />
                                    }
                                ]
                            }} trigger={['click']}>
                                <Button type="dashed" size="small" icon={<MoreOutlined />} disabled={!canAction(record.status)} />
                            </Dropdown>
                        ],
                        width: 60
                    }
                ]}
            />
            <Drawer open={openFeedback} onClose={() => setOpenFeedback(false)} title={`Chi tiết ${keyIn?.keyInName} - ${keyIn?.keyInPhoneNumber}`} width={800} footer={null}>
                <div dangerouslySetInnerHTML={{ __html: feedback?.evidence }} className="w-full h-full" />
            </Drawer>
            <ModalForm open={open} onOpenChange={setOpen} title={`${isApproved ? 'Phê duyệt' : 'Từ chối'} ${keyIn?.keyInName} - ${keyIn?.keyInPhoneNumber}`} onFinish={async (values) => {
                if (!keyIn) {
                    message.error('Không tìm thấy keyIn!');
                    return false;
                }
                values.id = keyIn?.id;
                if (isApproved) {
                    apiApproveDebt(values).then(() => {
                        message.success('Phê duyệt thành công!');
                        setOpen(false);
                        actionRef.current?.reload();
                    });
                } else {
                    apiRejectDebt(values).then(() => {
                        message.success('Từ chối thành công!');
                        setOpen(false);
                        actionRef.current?.reload();
                    });
                }
            }}>
                <ProFormTextArea label="Ghi chú" name="note" placeholder="Nhập lý do phê duyệt / từ chối" />
            </ModalForm>
        </PageContainer>
    )
}

export default Index;