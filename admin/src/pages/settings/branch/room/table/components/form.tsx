import { apiTableCreate, apiTableUpdate } from "@/services/settings/table";
import { ModalForm, ModalFormProps, ProFormDigit, ProFormInstance, ProFormText } from "@ant-design/pro-components";
import { useParams } from "@umijs/max";
import { message } from "antd";
import { useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const TableForm: React.FC<Props> = (props) => {

    const { id } = useParams<{ id: string }>();
    const formRef = useRef<ProFormInstance>(null);

    const onFinish = async (values: any) => {
        if (props.data) {
            await apiTableUpdate(values);
        } else {
            values.roomId = id;
            await apiTableCreate(values);
        }
        message.success('Thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }
    
    return (
        <ModalForm {...props} title="Thông tin bàn" formRef={formRef} onFinish={onFinish}>
            <ProFormText name="name" label="Tên bàn" rules={[{ required: true, message: 'Tên bàn là bắt buộc' }]} />
            <ProFormDigit name="sortOrder" label="Thứ tự" rules={[{ required: true, message: 'Thứ tự là bắt buộc' }]} />
        </ModalForm>
    )
}

export default TableForm;