import { apiContactBlock } from "@/services/contact";
import { ModalForm, ModalFormProps, ProFormTextArea } from "@ant-design/pro-components"
import { message } from "antd";

type Props = ModalFormProps & {
    contact?: any;
    reload?: () => void;
}

const BlockContactModal: React.FC<Props> = (props) => {

    const onFinish = async (values: any) => {
        if (!props.contact) return;
        values.id = props.contact.id;
        await apiContactBlock(values);
        message.success("Chặn liên hệ thành công");
        props.reload?.();
        return true;
    }

    return (
        <ModalForm
            {...props}
            onFinish={onFinish}
            title="Chặn liên hệ"
        >
            <ProFormTextArea name="note" label="Ghi chú" rules={[{ required: true, message: 'Vui lòng nhập ghi chú' }]} />
        </ModalForm>
    )
}

export default BlockContactModal;