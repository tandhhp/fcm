import { apiGiftUpdate } from "@/services/event/gift";
import { apiContractGiftAdd } from "@/services/finances/contract";
import { ModalForm, ModalFormProps, ProFormDatePicker, ProFormDigit, ProFormInstance, ProFormText } from "@ant-design/pro-components";
import { Col, message, Row } from "antd";
import { useEffect, useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
    contractId?: string;
}

const GiftForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);

    useEffect(() => {
        if (props.open) {
            formRef.current?.setFields([
                {
                    name: 'id',
                    value: props.data?.id || null
                },
                {
                    name: 'name',
                    value: props.data?.name || ''
                },
                {
                    name: 'amount',
                    value: props.data?.amount || null
                },
                {
                    name: 'expiredDate',
                    value: props.data?.expiredDate || null
                }
            ])
        }
    }, [props.open]);

    const onFinish = async (values: any) => {
        if (!props.contractId) {
            message.warning('Vui lòng chọn hợp đồng');
            return false;
        }
        if (values.id) {
            apiGiftUpdate(values);
        } else {
            await apiContractGiftAdd({
                contractId: props.contractId,
                ...values
            });
        }
        message.success('Thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Tặng quà" formRef={formRef} onFinish={onFinish}>
            <ProFormText name={"id"} hidden />
            <ProFormText name={"name"} label="Tên quà tặng" rules={[{ required: true }]} />
            <Row gutter={16}>
                <Col md={12} xs={24}>
                    <ProFormDigit
                    fieldProps={{
                        formatter: (value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')
                    }}
                    name="amount" label="Giá trị (VND)" min={1} rules={[{ required: true }]} />
                </Col>
                <Col md={12} xs={24}>
                    <ProFormDatePicker name="expiredDate" width="lg" label="Hạn sử dụng" rules={[{ required: true }]} />
                </Col>
            </Row>
        </ModalForm>
    )
}

export default GiftForm;