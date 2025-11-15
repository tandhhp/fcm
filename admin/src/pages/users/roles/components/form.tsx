import { apiRoleUpdate } from "@/services/role";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormText, ProFormTextArea } from "@ant-design/pro-components"
import { message } from "antd";
import { useEffect, useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const RoleForm: React.FC<Props> = ({ data, reload, ...rest }) => {

    const formRef = useRef<ProFormInstance>(null);

    useEffect(() => {
        if (data && rest.open) {
            formRef.current?.setFields([
                {
                    name: 'id',
                    value: data.id
                },
                {
                    name: 'displayName',
                    value: data.displayName
                },
                {
                    name: 'description',
                    value: data.description
                }
            ])
        }
        console.log(data, rest.open);
    }, [data, rest.open]);

    const onFinish = async (values: any) => {
        if (data) {
            await apiRoleUpdate(values);
        }
        message.success('Thành công');
        formRef.current?.resetFields();
        reload?.();
        return true;
    }

    return (
        <ModalForm {...rest} formRef={formRef} onFinish={onFinish}>
            <ProFormText name="id" hidden />
            <ProFormText name="displayName" label="Quyền" rules={[{ required: true, message: 'Vui lòng nhập quyền' }]} />
            <ProFormTextArea name="description" label="Mô tả" />
        </ModalForm>
    )
}

export default RoleForm;