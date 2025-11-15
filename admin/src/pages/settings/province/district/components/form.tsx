import { apiDistrictCreate, apiDistrictUpdate } from "@/services/settings/district";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormText } from "@ant-design/pro-components"
import { useParams } from "@umijs/max";
import { message } from "antd";
import { useEffect, useRef } from "react";

type Props = ModalFormProps & {
    reload?: () => void;
    data?: any;
}

const DistrictForm: React.FC<Props> = (props) => {

    const { id } = useParams();
    const formRef = useRef<ProFormInstance>();

    useEffect(() => {
        if (props.open && props.data) {
            formRef.current?.setFieldsValue(props.data);
        }
    }, [props.data, props.open]);

    const onFinish = async (values: any) => {
        if (!id) {
            message.error('Không xác định được tỉnh/thành phố');
            return false;
        }
        values.provinceId = id;
        if (props.data) {
            values.id = props.data.id;
            await apiDistrictUpdate(values);
        }
        else {
            await apiDistrictCreate(values);
        }
        message.success('Thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Quản lý xã / phường" formRef={formRef} onFinish={onFinish}>
            <ProFormText name={`name`} label="Tên xã / phường" rules={[{ required: true, message: 'Tên xã / phường là bắt buộc' }]} />
        </ModalForm>
    )
}

export default DistrictForm;