import { apiCallStatusCreate, apiCallStatusUpdate } from "@/services/call";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormText } from "@ant-design/pro-components";
import { message } from "antd";
import { useEffect, useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const CallStatusForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);

    useEffect(() => {
        if (props.data && props.open) {
            formRef.current?.setFields([
                {
                    name: 'id',
                    value: props.data.id
                },
                {
                    name: 'name',
                    value: props.data.name
                }
            ])
        }
    }, [props.data, props.open]);

    const onFinish = async (values: any) => {
        if (props.data) {
            await apiCallStatusUpdate({ id: props.data.id, ...values });
        } else {
            await apiCallStatusCreate(values);
        }
        message.success('Thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Thông tin trạng thái cuộc gọi" formRef={formRef} onFinish={onFinish}>
            <ProFormText name="id" hidden />
            <ProFormText name="name" label="Tên trạng thái cuộc gọi" rules={[{ required: true, message: 'Tên trạng thái cuộc gọi là bắt buộc' }]} />
        </ModalForm>
    )
}

export default CallStatusForm;