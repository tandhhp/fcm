import { apiEventTableOptions } from "@/services/event";
import { apiAttendanceOptions } from "@/services/event/attendance";
import { apiLeadCheckin, apiLeadDetail } from "@/services/users/lead";
import { GENDER_OPTIONS } from "@/utils/constants";
import { ModalForm, ModalFormProps, ProFormDatePicker, ProFormInstance, ProFormList, ProFormSelect, ProFormText, ProFormTextArea } from "@ant-design/pro-components";
import { useParams } from "@umijs/max";
import { Col, message, Row } from "antd";
import dayjs from "dayjs";
import { useEffect, useRef, useState } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const LeadCheckin: React.FC<Props> = (props) => {

    const { id } = useParams<{ id: string }>();
    const formRef = useRef<ProFormInstance>();
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (props.open && props.data) {
            setLoading(true);
            apiLeadDetail(props.data.id).then(res => {
                const data = res.data;
                formRef.current?.setFields([
                    {
                        name: 'leadId',
                        value: data.id
                    },
                    {
                        name: 'name',
                        value: data.name
                    },
                    {
                        name: 'phoneNumber',
                        value: data.phoneNumber
                    },
                    {
                        name: 'dateOfBirth',
                        value: data.dateOfBirth
                    },
                    {
                        name: 'gender',
                        value: data.gender
                    },
                    {
                        name: 'identityNumber',
                        value: data.identityNumber
                    },
                    {
                        name: 'address',
                        value: data.address
                    },
                    {
                        name: 'note',
                        value: data.note
                    },
                    {
                        name: 'salesId',
                        value: data.salesId
                    },
                    {
                        name: 'telesaleId',
                        value: data.telesaleId
                    },
                    {
                        name: 'branchId',
                        value: data.branchId
                    },
                    {
                        name: 'attendanceId',
                        value: data.attendanceId
                    },
                    {
                        name: 'tableId',
                        value: data.tableId
                    },
                    {
                        name: 'subLeads',
                        value: data.subLeads
                    }
                ]);
                setLoading(false);
            });
        }
    }, [props.data, props.open]);

    const onFinish = async (values: any) => {
        if (!props.data) {
            message.warning('Dữ liệu không hợp lệ');
            return false;
        }
        await apiLeadCheckin({
            ...values,
            dateOfBirth: values.dateOfBirth ? dayjs(values.dateOfBirth).format('YYYY-MM-DD') : undefined
        });
        message.success('Check-in thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title={`Check-in ${props.data?.name}`} formRef={formRef} onFinish={onFinish} loading={loading}>
            <ProFormText name="leadId" hidden />
            <Row gutter={16}>
                <Col md={8} xs={12}>
                    <ProFormText label="Họ và tên" name="name" rules={[
                        {
                            required: true
                        }
                    ]} />
                </Col>
                <Col md={8} xs={12}>
                    <ProFormText label="Số điện thoại" name="phoneNumber" rules={[
                        {
                            required: true
                        }
                    ]} />
                </Col>
                <Col md={8} xs={12}>
                    <ProFormText name="identityNumber" label="Số CCCD" rules={[
                        { required: true, message: 'Vui lòng nhập CCCD' }
                        ]} />
                </Col>
                <Col md={6} xs={12}>
                    <ProFormDatePicker.Year label="Năm sinh" name="dateOfBirth" width="xl" rules={[{ required: true }]} />
                </Col>
                <Col md={4} xs={12}>
                    <ProFormSelect label="Giới tính" name="gender" options={GENDER_OPTIONS} />
                </Col>
                <Col md={6} xs={12}>
                    <ProFormSelect name="tableId" label="Bàn" request={apiEventTableOptions} params={{
                        eventId: id,
                        eventDate: props.data?.eventDate ? dayjs(props.data.eventDate).format('YYYY-MM-DD') : undefined
                    }} rules={[{ required: true }]} showSearch />
                </Col>
                <Col md={8} xs={12}>
                    <ProFormSelect name="attendanceId" label="Trạng thái tham dự" request={apiAttendanceOptions} showSearch rules={[
                        { required: true }
                    ]} />
                </Col>
                <Col md={24} xs={24}>
                    <ProFormTextArea name="note" label="Ghi chú" />
                </Col>
            </Row>
            <ProFormList name={"subLeads"} label="Khách phụ">
                <div className="flex gap-4">
                    <ProFormText name="name" label="Họ và tên" rules={[{ required: true, message: 'Vui lòng nhập họ và tên' }]} />
                    <ProFormText name="phoneNumber" label="Số điện thoại" rules={[
                        { required: true, message: 'Vui lòng nhập số điện thoại' },
                        {
                            pattern: /((09|03|07|08|05)+([0-9]{8})\b)/,
                            message: 'Số điện thoại không hợp lệ'
                        }]} />
                    <ProFormText name="identityNumber" label="Số CCCD" rules={[{ required: true, message: 'Vui lòng nhập số CCCD' }]} />
                    <ProFormDatePicker name="dateOfBirth" label="Ngày sinh" />
                    <ProFormSelect name="gender" label="Giới tính" options={GENDER_OPTIONS} rules={[{ required: true, message: 'Vui lòng chọn giới tính' }]} />
                </div>
            </ProFormList>
        </ModalForm>
    )
}

export default LeadCheckin;