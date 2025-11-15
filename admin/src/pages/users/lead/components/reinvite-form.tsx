import { apiEventOptions, apiEventReinvite } from "@/services/event";
import { ModalForm, ModalFormProps, ProFormDatePicker, ProFormInstance, ProFormSelect, ProFormTextArea } from "@ant-design/pro-components";
import { Col, message, Row } from "antd";
import { useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const ReinviteForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);

    const onFinish = async (values: any) => {
        await apiEventReinvite({
            id: props.data.id,
            ...values
        });
        message.success('Mời lại thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title={`Mời lại - ${props.data?.name || ''}`} formRef={formRef} onFinish={onFinish}>
            <Row gutter={16}>
                <Col md={18} xs={24}>
                    <ProFormDatePicker name="eventDate" label="Ngày tham gia sự kiện" width="xl" rules={[{ required: true }]} />
                </Col>
                <Col md={6} xs={24}>
                    <ProFormSelect name="eventId" label="Khung giờ" width="md" rules={[{ required: true }]} request={apiEventOptions} />
                </Col>
            </Row>
            <ProFormTextArea name="note" label="Ghi chú" />
        </ModalForm>
    )
}

export default ReinviteForm;