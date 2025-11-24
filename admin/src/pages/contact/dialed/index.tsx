import { apiCallOptions } from "@/services/call";
import { apiContactDialedCalls, apiExportDialedCalls } from "@/services/contact";
import { CalendarOutlined, EditOutlined, ExportOutlined, EyeOutlined, MoreOutlined, PhoneOutlined, SettingOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProFormDateRangePicker, ProTable } from "@ant-design/pro-components"
import { Button, Dropdown } from "antd";
import { useRef, useState } from "react";
import CallForm from "../components/call";
import BookingForm from "../components/booking";
import { history } from "@umijs/max";
import ContactForm from "../components/form";

const Index: React.FC = () => {

    const actionRef = useRef<ActionType>();
    const [openExport, setOpenExport] = useState<boolean>(false);
    const [openCall, setOpenCall] = useState<boolean>(false);
    const [contact, setContact] = useState<any>();
    const [openBooking, setOpenBooking] = useState<boolean>(false);
    const [openForm, setOpenForm] = useState<boolean>(false);

    const onFinishExport = async (values: any) => {
        const response = await apiExportDialedCalls({
            fromDate: values.dateRange ? values.dateRange[0].format("YYYY-MM-DD") : undefined,
            toDate: values.dateRange ? values.dateRange[1].format("YYYY-MM-DD") : undefined,
        });
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `dialed_calls_${Date.now()}.xlsx`;
        a.click();
        window.URL.revokeObjectURL(url);
        return true;
    }

    return (
        <PageContainer extra={<Button onClick={() => setOpenExport(true)} type="primary" icon={<ExportOutlined />}>Xuất dữ liệu</Button>}>
            <ProTable
                actionRef={actionRef}
                request={apiContactDialedCalls}
                rowKey="id"
                scroll={{
                    x: true
                }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'SDT',
                        dataIndex: 'phoneNumber',
                        minWidth: 100,
                        width: 100,
                        render: (text, record) => {
                            return (
                                <Button type="link" size="small" icon={<PhoneOutlined />} onClick={() => {
                                    setContact(record);
                                    setOpenCall(true);
                                }}>{text}</Button>
                            )
                        }
                    },
                    {
                        title: 'Tên liên hệ',
                        dataIndex: 'name',
                        minWidth: 150,
                    },
                    {
                        title: 'Ngày gọi',
                        dataIndex: 'calledAt',
                        valueType: 'dateTime',
                        search: false,
                        width: 150,
                        minWidth: 150
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'callStatusId',
                        request: apiCallOptions,
                        valueType: 'select',
                        fieldProps: {
                            showSearch: true
                        }
                    },
                    {
                        title: 'Nguồn',
                        dataIndex: 'sourceName',
                        search: false
                    },
                    {
                        title: 'Người gọi',
                        dataIndex: 'teleName',
                        search: false
                    },
                    {
                        title: 'Extra',
                        dataIndex: 'extraStatus'
                    },
                    {
                        title: 'Tuổi',
                        dataIndex: 'age'
                    },
                    {
                        title: 'Công việc',
                        dataIndex: 'job'
                    },
                    {
                        title: 'Hẹn gọi',
                        dataIndex: 'followUpDate',
                        valueType: 'date',
                        search: false,
                        width: 100,
                        minWidth: 100
                    },
                    {
                        title: 'Sự kiện',
                        dataIndex: 'isBooked',
                        valueType: 'select',
                        valueEnum: {
                            true: { text: 'Đã đặt', status: 'Success' },
                            false: { text: 'Chưa đặt', status: 'Default' }
                        }
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note'
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        render: (dom, entity) => [
                            <Dropdown key="more" menu={{
                                items: [
                                    {
                                        key: 'view',
                                        label: 'Chi tiết',
                                        onClick: () => {
                                            history.push(`/contact/center/${entity.id}`);
                                        },
                                        icon: <EyeOutlined />
                                    },
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        icon: <EditOutlined />,
                                        onClick: () => {
                                            setContact(entity);
                                            setOpenForm(true);
                                        }
                                    },
                                    {
                                        key: 'call',
                                        label: 'Cuộc gọi',
                                        onClick: () => {
                                            setContact(entity);
                                            setOpenCall(true);
                                        },
                                        icon: <PhoneOutlined />
                                    },
                                    {
                                        key: 'booking',
                                        label: 'Đặt lịch hẹn',
                                        onClick: () => {
                                            setContact(entity);
                                            setOpenBooking(true);
                                        },
                                        icon: <CalendarOutlined />,
                                        disabled: entity.isBooked
                                    }
                                ]
                            }}>
                                <Button size="small" type="dashed" icon={<MoreOutlined />} />
                            </Dropdown>
                        ],
                        width: 40
                    }
                ]}
                search={{
                    layout: 'vertical'
                }}
            />
            <ModalForm
                open={openExport}
                onOpenChange={setOpenExport}
                title="Xuất dữ liệu cuộc gọi đã gọi"
                width={400}
                onFinish={onFinishExport}
            >
                <ProFormDateRangePicker
                    name="dateRange"
                    label="Chọn khoảng ngày"
                    width="md"
                />
            </ModalForm>
            <ContactForm open={openForm} onOpenChange={setOpenForm} data={contact} reload={() => actionRef.current?.reload()} />
            <CallForm open={openCall} data={contact} onOpenChange={setOpenCall} reload={() => actionRef.current?.reload()} />
            <BookingForm open={openBooking} data={contact} onOpenChange={setOpenBooking} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}

export default Index;