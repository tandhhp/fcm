import { apiJobKindCreate, apiJobKindUpdate } from "@/services/settings/job-kind";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormSelect, ProFormText } from "@ant-design/pro-components"
import { message } from "antd";
import { useEffect, useRef } from "react";

type Props = ModalFormProps & {
    data?: any
    reload?: () => void;
}

const JobKindForm: React.FC<Props> = (props) => {

    useEffect(() => {
        if (props.data) {
            formRef.current?.setFields([
                {
                    name: 'name',
                    value: props.data.name
                },
                {
                    name: 'isActive',
                    value: props.data.isActive
                }
            ])
        }
    }, [props.data])

    const formRef = useRef<ProFormInstance>(null);

    const onFinish = async (values: any) => {
        if (props.data) {
            values.id = props.data.id;
            await apiJobKindUpdate(values);
        } else {
            await apiJobKindCreate(values);
        }
        message.success('Thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Thông tin loại công việc" formRef={formRef} onFinish={onFinish}>
            <ProFormText name="name" label="Tên loại công việc" rules={[{ required: true, message: 'Tên loại công việc là bắt buộc' }]} />
            <ProFormSelect name="isActive" label="Trạng thái" allowClear={false} initialValue={true} rules={[{ required: true, message: 'Trạng thái là bắt buộc' }]} options={[
                { label: 'Kích hoạt', value: true },
                { label: 'Ngừng kích hoạt', value: false },
            ]} />
        </ModalForm>
    )
}

export default JobKindForm;