import { apiAttendanceCreate, apiAttendanceUpdate } from "@/services/event/attendance";
import { ModalForm, ModalFormProps, ProFormDigit, ProFormInstance, ProFormSelect, ProFormText } from "@ant-design/pro-components";
import { message } from "antd";
import { useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const AttendanceForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);

    const onFinish = async (values: any) => {
        if (props.data) {
            await apiAttendanceUpdate(values);
        } else {
            await apiAttendanceCreate(values);
        }
        message.success('Lưu thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Trạng thái tham dự" formRef={formRef} onFinish={onFinish}>
            <ProFormText name="name" label="Tên trạng thái" rules={[{ required: true }]} />
            <ProFormDigit name="sortOrder" label="Thứ tự" rules={[{ required: true }]} />
            <ProFormDigit name="suRate" label="Tỷ lệ SU" rules={[{ required: true }]} />
            <ProFormSelect name="isActive" label="Kích hoạt" valueEnum={{ true: 'Có', false: 'Không' }} rules={[{ required: true }]} />
        </ModalForm>

    )
}

export default AttendanceForm;