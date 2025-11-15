import { apiProvinceCreate, apiProvinceUpdate } from "@/services/settings/province";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormText } from "@ant-design/pro-components"
import { message } from "antd";
import { useEffect, useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const ProvinceForm : React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);

    useEffect(() => {
        if (props.data) {
            formRef.current?.setFieldsValue(props.data);
        }
    }, [props.data]);

    const onFinish = async (values: any) => {
        if (props.data) {
            // Update existing province
            await apiProvinceUpdate({ ...values, id: props.data.id });
        } else {
            await apiProvinceCreate(values);
        }
        message.success('Lưu thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Cài đặt Tỉnh/Thành phố" formRef={formRef} onFinish={onFinish}>
            <ProFormText name="name" label="Tỉnh/Thành phố" placeholder="Nhập Tỉnh/Thành phố" rules={[{ required: true, message: 'Vui lòng nhập Tỉnh/Thành phố' }]} />
        </ModalForm>
    )
}

export default ProvinceForm;