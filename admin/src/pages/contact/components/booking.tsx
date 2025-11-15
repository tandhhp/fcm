import { apiContactBook } from "@/services/contact";
import { apiEventOptions } from "@/services/event";
import { ModalForm, ModalFormProps, ProFormDatePicker, ProFormInstance, ProFormSelect, ProFormText, ProFormTextArea } from "@ant-design/pro-components";
import { Col, message, Row } from "antd";
import { useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const BookingForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);

    const onFinish = async (values: any) => {
        if (!props.data) {
            message.error('Thiếu thông tin khách hàng');
            return false;
        }
        values.id = props.data.id;
        await apiContactBook(values);
        message.success('Đặt lịch hẹn thành công!');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title={`Đặt lịch hẹn ${props.data?.name}`} formRef={formRef} onFinish={onFinish} width={600}>
            <Row gutter={[16, 16]}>
                <Col md={12} xs={24}>
                    <ProFormDatePicker width="xl" name="eventDate" label="Ngày hẹn" rules={[{ required: true, message: 'Ngày hẹn là bắt buộc' }]} />
                </Col>
                <Col md={12} xs={24}>
                    <ProFormSelect name="eventId"
                        request={apiEventOptions}
                        allowClear={false}
                        label="Khung giờ" rules={[{ required: true, message: 'Khung giờ là bắt buộc' }]} />
                </Col>
            </Row>
            <ProFormTextArea name="note" label="Ghi chú" />
        </ModalForm>
    )
}

export default BookingForm;