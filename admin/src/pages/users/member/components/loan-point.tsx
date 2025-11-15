import { apiLoanPoint } from "@/services/user";
import { ModalForm, ModalFormProps, ProFormDigit, ProFormTextArea } from "@ant-design/pro-components"
import { message } from "antd";

type Props = ModalFormProps & {
    cardHolder?: any;
}

const LoanPointModal : React.FC<Props> = (props) => {

    const onFinish = async (values: any) => {
        values.userId = props.cardHolder?.id;
        await apiLoanPoint(values);
        message.success('Vay điểm thành công');
        props.onOpenChange?.(false);
    }

    return (
        <ModalForm title="Vay điểm" {...props} onFinish={onFinish}>
            <ProFormDigit
                name="amount"
                label="Số điểm vay"
                rules={[{ required: true, message: 'Vui lòng nhập số điểm vay' }]}
                fieldProps={{
                    placeholder: 'Nhập số điểm vay',
                    addonAfter: 'điểm'
                }}
            />
            <ProFormTextArea
                name="note"
                label="Ghi chú"
                rules={[{ required: true, message: 'Vui lòng nhập ghi chú' }]}
                fieldProps={{
                    placeholder: 'Nhập ghi chú',
                    autoSize: { minRows: 3, maxRows: 5 }
                }}
            />
        </ModalForm>
    )
}

export default LoanPointModal;