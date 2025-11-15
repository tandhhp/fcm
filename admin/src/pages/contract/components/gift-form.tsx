import { apiContractGiftAdd } from "@/services/finances/contract";
import { apiGiftOptions } from "@/services/event/gift";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormSelect } from "@ant-design/pro-components";
import { message } from "antd";
import { useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const GiftForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);

    const onFinish = async (values: any) => {
        if (!props.data) {
            message.warning('Vui lòng chọn hợp đồng');
            return false;
        }
        await apiContractGiftAdd({
            contractId: props.data.id,
            ...values
        });
        message.success('Thêm quà tặng thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Tặng quà" formRef={formRef} onFinish={onFinish}>
            <ProFormSelect
                name="giftId"
                label="Chọn quà tặng"
                request={apiGiftOptions}
                showSearch
                rules={[
                    {
                        required: true,
                        message: 'Vui lòng chọn quà tặng'
                    }
                ]}
            />
        </ModalForm>
    )
}

export default GiftForm;