import { apiGiftCreate, apiGiftUpdate } from "@/services/event/gift";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormText } from "@ant-design/pro-components"
import { message } from "antd";
import { useEffect, useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const GiftForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>();

    useEffect(() => {
        if (props.open && props.data) {
            formRef.current?.setFields([
                {
                    name: 'id',
                    value: props.data.id
                },
                {
                    name: 'name',
                    value: props.data.name
                }
            ]);
        }
    }, [props.open, props.data]);

    const onFinish = async (values: any) => {
        if (props.data) {
            await apiGiftUpdate(values);
        } else {
            await apiGiftCreate(values);
        }
        message.success('Lưu quà tặng thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm title="Quà tặng" {...props} formRef={formRef} onFinish={onFinish}>
            <ProFormText name="id" hidden />
            <ProFormText name="name" label="Tên quà tặng" rules={[
                {
                    required: true
                }
            ]} />
        </ModalForm>
    )
}
export default GiftForm;