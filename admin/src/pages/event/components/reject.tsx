import { apiLeadReject } from "@/services/users/lead";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormTextArea } from "@ant-design/pro-components"
import { message } from "antd";
import { useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const LeadRejectForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>();

    const onFinish = async (values: any) => {
        if (!props.data) {
            message.error('Không tìm thấy dữ liệu');
            return false;
        }
        values.id = props.data.id;
        await apiLeadReject(values);
        message.success('Từ chối khách hàng thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title={`Từ chối khách hàng ${props.data?.name ?? ''}`} formRef={formRef} onFinish={onFinish}>
            <ProFormTextArea name="note" label="Lý do từ chối" />
        </ModalForm>
    )
}

export default LeadRejectForm;