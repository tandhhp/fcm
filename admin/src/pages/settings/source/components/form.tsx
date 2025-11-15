import { apiSourceCreate, apiSourceDetail, apiSourceUpdate } from "@/services/settings/source";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormText } from "@ant-design/pro-components";
import { message } from "antd";
import { useEffect, useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const SourceForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);

    useEffect(() => {
        if (props.data) {
            apiSourceDetail(props.data.id).then(res => {
                const data = res.data;
                formRef.current?.setFields([
                    {
                        name: 'id',
                        value: data.id
                    },
                    {
                        name: 'name',
                        value: data.name
                    }
                ])
            });
        }
    }, [props.data, props.open]);

    const onFinish = async (values: any) => {
        if (props.data) {
            await apiSourceUpdate(values);
        } else {
            await apiSourceCreate(values);
        }
        message.success('Lưu thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Nguồn" formRef={formRef} onFinish={onFinish}>
            <ProFormText name="id" hidden />
            <ProFormText name="name" label="Tên nguồn" rules={[{ required: true, message: 'Vui lòng nhập tên nguồn' }]} />
        </ModalForm>
    )
}

export default SourceForm;