import { apiMyKeyin, apiUpdateEventDate } from "@/services/contact";
import { CalendarOutlined, ManOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProFormDatePicker, ProFormInstance, ProFormSelect, ProFormText, ProFormTextArea, ProTable } from "@ant-design/pro-components"
import { Button, Col, message, Row, Tag, Tooltip } from "antd";
import dayjs from "dayjs";
import { useEffect, useRef, useState } from "react";
import ReinviteModal from "./components/reinvite";
import { useModel } from "@umijs/max";
import { apiBranchOptions } from "@/services/settings/branch";

const MyKeyInPage: React.FC = () => {

    const [open, setOpen] = useState<boolean>(false);
    const [lead, setLead] = useState<any>();
    const actionRef = useRef<ActionType>();
    const formRef = useRef<ProFormInstance>();
    const [openHistory, setOpenHistory] = useState<boolean>(false);
    const { initialState } = useModel('@@initialState');

    useEffect(() => {
        if (lead) {
            formRef.current?.setFields([
                {
                    name: 'id',
                    value: lead.id
                },
                {
                    name: 'eventDate',
                    value: lead.eventDate
                },
                {
                    name: 'eventTime',
                    value: lead.eventTime
                }
            ])
        }
    }, [lead]);

    return (
        <PageContainer>
            <ProTable
                actionRef={actionRef}
                search={{
                    layout: 'vertical'
                }}
                scroll={{
                    x: true
                }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'name',
                        render: (dom, entity) => (
                            <div className="min-w-[150px]">
                                <div>{entity.gender === true && (<ManOutlined className='text-blue-500' />)}{entity.gender === false && (<WomanOutlined className='text-red-500' />)} {dom}</div>
                            </div>
                        )
                    },
                    {
                        title: 'Chi nhánh',
                        dataIndex: 'branchId',
                        search: false,
                        minWidth: 100,
                        valueType: 'select',
                        request: apiBranchOptions
                    },
                    {
                        title: 'SĐT',
                        dataIndex: 'phoneNumber',
                        width: 100
                    },
                    {
                        title: 'Email',
                        dataIndex: 'email',
                        width: 100,
                        search: false
                    },
                    {
                        title: 'Người gọi',
                        dataIndex: 'teleName',
                        width: 150,
                        search: false,
                        minWidth: 150
                    },
                    {
                        title: 'Trợ lý',
                        dataIndex: 'saleName',
                        width: 150,
                        search: false,
                        minWidth: 150
                    },
                    {
                        title: 'Bước',
                        dataIndex: 'status',
                        valueEnum: {
                            0: <Tag color="yellow">Chờ duyệt</Tag>,
                            1: <Tag color="orange">Đã duyệt</Tag>,
                            2: <Tag color="red">Check-In</Tag>,
                            3: <Tag color="blue">Chốt deal</Tag>,
                            4: <Tag color="black">Từ chối</Tag>,
                            5: <Tag color="green">Hoàn thành</Tag>,
                            6: <Tag color="green">KT Xác nhận</Tag>,
                            7: <Tag color="green">GĐ Xác nhận</Tag>,
                            8: <Tag color="tomato">Mời lại</Tag>
                        },
                        width: 80,
                        search: false
                    },
                    {
                        title: 'Ngày tham gia',
                        dataIndex: 'eventDate',
                        render: (_, entity) => entity.eventDate ? dayjs(entity.eventDate).format('DD-MM-YYYY') : '-',
                        width: 120,
                        search: false,
                        minWidth: 120
                    },
                    {
                        title: 'Giờ tham gia',
                        dataIndex: 'eventTime',
                        width: 100,
                        search: false,
                        minWidth: 100
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note',
                        search: false,
                        minWidth: 100
                    },
                    {
                        title: 'Lượt',
                        dataIndex: 'inviteCount',
                        search: false,
                        render: (_, entity) => {
                            return <Button size="small"
                                onClick={() => {
                                    setLead(entity);
                                    setOpenHistory(true);
                                }}
                                type="dashed" disabled={entity.inviteCount === 1}>{entity.inviteCount}</Button>
                        }
                    },
                    {
                        title: 'Tác vụ',
                        valueType: 'option',
                        render: (_, entity) => [
                            <Tooltip key="move" title="Đổi ngày tham gia">
                                <Button icon={<CalendarOutlined />} size="small" type="primary" onClick={() => {
                                    setLead(entity);
                                    setOpen(true);
                                }} />
                            </Tooltip>
                        ],
                        width: 80
                    }
                ]}
                request={(params) => apiMyKeyin({
                    ...params,
                    branch: initialState?.currentUser?.branch
                })}
            />
            <ModalForm open={open} onOpenChange={setOpen} title="Đổi ngày tham gia" formRef={formRef} onFinish={async (values: any) => {
                if (values.eventDate) {
                    values.eventDate = dayjs(values.eventDate).format('YYYY-MM-DD');
                }
                await apiUpdateEventDate(values);
                message.success('Đổi thành công!');
                actionRef.current?.reload();
                setOpen(false);
            }}>
                <ProFormText name="id" hidden />
                <Row gutter={16}>
                    <Col md={16} xs={24}>
                        <ProFormDatePicker name="eventDate" label="Ngày tham gia" rules={[
                            {
                                required: true
                            }
                        ]} width="xl" fieldProps={{
                            format: {
                                type: 'mask',
                                format: 'DD-MM-YYYY'
                            }
                        }} />
                    </Col>
                    <Col md={8} xs={24}>
                        <ProFormSelect name="eventTime" label="Giờ tham gia" rules={[
                            {
                                required: true
                            }
                        ]} options={[
                            {
                                label: '09:00',
                                value: '09:00'
                            },
                            {
                                label: '14:30',
                                value: '14:30'
                            }
                        ]} />
                    </Col>
                </Row>
                <ProFormTextArea label="Ghi chú" name="note" />
            </ModalForm>
            <ReinviteModal open={openHistory} onCancel={() => setOpenHistory(false)} leadId={lead?.id} />
        </PageContainer>
    )
}

export default MyKeyInPage;