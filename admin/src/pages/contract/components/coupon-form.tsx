import { apiCouponCreate } from "@/services/finances/coupon";
import { ModalForm, ModalFormProps, ProFormDigit, ProFormInstance, ProFormText } from "@ant-design/pro-components";
import { message } from "antd";
import { useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const CouponForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>();

    const onFinish = async (values: any) => {
        if (!props.data) {
            message.warning('Dữ liệu phiếu quy đổi không hợp lệ');
            return false;
        }
        await apiCouponCreate({ ...values, contractId: props.data.id });
        message.success('Lưu phiếu quy đổi thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm title="Phiếu quy đổi" {...props} formRef={formRef} onFinish={onFinish}>
            <ProFormText name="id" hidden />
            <ProFormText name="name" label="Tên phiếu quy đổi" rules={[
                {
                    required: true
                }
            ]} />
            <ProFormDigit name="discount" label="Giá trị (VNĐ)" min={0} rules={[
                {
                    required: true
                }
            ]} />
        </ModalForm>
    )
}

export default CouponForm;