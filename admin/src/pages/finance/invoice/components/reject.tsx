import { apiInvoiceReject } from "@/services/finances/invoice";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormTextArea } from "@ant-design/pro-components"
import { message } from "antd";
import { useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const InvoiceRejectForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>();

    const onFinish = async (values: any) => {
        if (!props.data) {
            message.error('Dữ liệu không hợp lệ');
            return false;
        }
        values.id = props.data.id;
        await apiInvoiceReject(values);
        message.success('Từ chối thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Từ chối hóa đơn" formRef={formRef} onFinish={onFinish}>
            <ProFormTextArea name="note" label="Lý do từ chối" rules={[{ required: true }]} />
        </ModalForm>
    )
}

export default InvoiceRejectForm;