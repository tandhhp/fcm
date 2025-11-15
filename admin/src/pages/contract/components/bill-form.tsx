import { apiBillCreate } from "@/services/finances/bill";
import { ModalForm, ModalFormProps, ProFormDigit, ProFormInstance, ProFormText, ProFormTextArea } from "@ant-design/pro-components";
import { Col, message, Row } from "antd";
import { useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const BillForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>();

    const onFinish = async (values: any) => {
        if (!props.data) {
            message.error('Vui lòng chọn hợp đồng');
            return false;
        }
        await apiBillCreate({
            ...values,
            contractId: props.data.id
        });
        message.success(`Lưu phiếu chi thành công`);
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm title="Phiếu chi" {...props} formRef={formRef} onFinish={onFinish}>
            <ProFormText name="name" label="Tên phiếu chi" rules={[{ required: true }]} />
            <Row gutter={16}>
                <Col md={12} xs={24}>
                    <ProFormText name="billNumber" label="Số phiếu chi" rules={[{ required: true }]} />
                </Col>
                <Col md={12} xs={24}>
                    <ProFormDigit name="amount" label="Số tiền" rules={[{ required: true }]}
                        fieldProps={{
                            formatter: (value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ','),
                            parser: (value) => value?.replace(/(,*)/g, '') as unknown as number
                        }} />
                </Col>
            </Row>
            <ProFormTextArea name="note" label="Ghi chú" />
        </ModalForm>
    )
}

export default BillForm;