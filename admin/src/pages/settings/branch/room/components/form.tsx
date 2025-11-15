import { apiRoomCreate, apiRoomUpdate } from "@/services/settings/room";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormText } from "@ant-design/pro-components";
import { useParams } from "@umijs/max";
import { message } from "antd";
import { useEffect, useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const RoomForm: React.FC<Props> = (props) => {

    const { id } = useParams<{ id: string }>();
    const formRef = useRef<ProFormInstance>(null);

    useEffect(() => {
        if (props.data && props.open) {
            formRef.current?.setFields([
                {
                    name: 'name',
                    value: props.data.name
                },
                {
                    name: 'id',
                    value: props.data.id
                }
            ]);
        }
    }, [props.data, props.open]);

    const onFinish = async (values: any) => {
        values.branchId = id;
        if (props.data) {
            values.id = props.data.id;
            await apiRoomUpdate(values);
        } else {
            await apiRoomCreate(values);
        }
        message.success('Thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Thông tin phòng" formRef={formRef} onFinish={onFinish}>
            <ProFormText name="name" label="Tên phòng" rules={[{ required: true, message: 'Tên phòng là bắt buộc' }]} />
        </ModalForm>
    )
}
export default RoomForm;