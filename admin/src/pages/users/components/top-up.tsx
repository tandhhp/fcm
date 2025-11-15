import MyCKEditor from "@/components/ckeditor"
import { TopupType } from "@/utils/constants"
import { ModalForm, ModalFormProps, ProFormDigit, ProFormInstance, ProFormSelect } from "@ant-design/pro-components"
import { Col, message, Row } from "antd"
import { useEffect, useRef } from "react"

type Props = ModalFormProps & {
    amount?: number;
    type?: TopupType;
}

const TopUpModal: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>();

    useEffect(() => {
        formRef.current?.setFields([
            {
                name: 'amount',
                value: props.amount
            },
            {
                name: 'type',
                value: props.type
            }
        ]);
    }, [props.id]);

    const onFinish = async (values: any) => {
        if (!props.id) {
            message.warning('Không tìm thấy chủ thẻ');
            return;
        }
        values.cardHolderId = props.id;
        message.success('Nạp tiền thành công, đang chờ giám đốc phê duyệt!');
        props.onOpenChange?.(false);
    }

    return (
        <ModalForm {...props} title="Nạp tiền" onFinish={onFinish} formRef={formRef}>
            <Row gutter={16}>
                <Col xs={24} md={18}>
                    <ProFormDigit name="amount" label="Số tiền" rules={[
                        {
                            required: true,
                            message: 'Vui lòng nhập số tiền nạp'
                        }
                    ]}
                        fieldProps={{
                            formatter: (value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ','),
                            parser: (value) => value?.replace(/(,*)/g, '') as unknown as number
                        }}
                    />
                </Col>
                <Col xs={24} md={6}>
                    <ProFormSelect name="type" label="Loại" options={[
                        {
                            label: 'Top-up',
                            value: TopupType.Topup
                        },
                        {
                            label: 'Công nợ',
                            value: TopupType.Debt
                        }
                    ]} rules={[
                        {
                            required: true
                        }
                    ]} />
                </Col>
            </Row>
            <MyCKEditor name="note" label="Ghi chú" />
        </ModalForm>
    )
}

export default TopUpModal;