import { apiBackToCheckin } from "@/services/event";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormText, ProFormTextArea } from "@ant-design/pro-components"
import { message } from "antd";
import { useEffect, useRef } from "react";

type Props = ModalFormProps & {
    reload?: () => void;
}

const BackToCheckinModal: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>();

    useEffect(() => {
        formRef.current?.setFieldValue('keyInId', props.id);
    }, [props.open]);

    const onFinish = async (values: any) => {
        await apiBackToCheckin(values);
        message.success("Chuyển trạng thái thành công");
        formRef.current?.resetFields();
        props.reload?.();
    }

    return (
        <ModalForm {...props} title="Chuyển trạng thái về check-in" onFinish={onFinish} formRef={formRef}>
            <ProFormText name="keyInId" hidden />
            <ProFormTextArea
                name="note"
                label="Ghi chú"
                placeholder="Nhập ghi chú"
            />
        </ModalForm>
    )
}

export default BackToCheckinModal;