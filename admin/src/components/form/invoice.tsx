import { apiInvoiceDetail, apiInvoiceUpdate } from "@/services/finances/invoice";
import { PaymentMethod } from "@/utils/enum";
import { ModalForm, ModalFormProps, ProFormDatePicker, ProFormDigit, ProFormInstance, ProFormSelect, ProFormText, ProFormTextArea } from "@ant-design/pro-components";
import { Col, message, Row } from "antd";
import dayjs from "dayjs";
import { useEffect, useRef } from "react";

type Props = ModalFormProps & {
    reload?: () => void;
    data?: any;
}

const InvoiceForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>();

    useEffect(() => {
        if (props.open && props.data) {
            apiInvoiceDetail(props.data.id).then((res) => {
                const data = res.data;
                formRef.current?.setFields([
                    {
                        name: 'id',
                        value: data.id
                    },
                    {
                        name: 'invoiceNumber',
                        value: data.invoiceNumber
                    },
                    {
                        name: 'contractId',
                        value: data.contractId
                    },
                    {
                        name: 'amount',
                        value: data.amount
                    },
                    {
                        name: 'paymentMethod',
                        value: data.paymentMethod
                    },
                    {
                        name: 'note',
                        value: data.note
                    },
                    {
                        name: 'createdAt',
                        value: dayjs(data.createdAt)
                    }
                ]);
            });
        }
    }, [props.open, props.data]);

    const onFinish = async (values: any) => {
        if (props.data) {
            values.createdAt = values.createdAt ? dayjs(values.createdAt).format('YYYY-MM-DD') : undefined;
            await apiInvoiceUpdate(values);
        }
        message.success(`Phiếu thu đã được ${props.data ? 'cập nhật' : 'tạo'} thành công`);
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm
            {...props}
            onFinish={onFinish}
            title={props.data ? 'Cập nhật phiếu thu' : 'Tạo phiếu thu'}
            formRef={formRef}
        >   <ProFormText name="id" hidden />
            <Row gutter={16}>
                <Col md={12} xs={24}>
                    <ProFormText name="invoiceNumber" label="Số phiếu thu" rules={[{ required: true, message: 'Vui lòng nhập số phiếu thu' }]} />
                </Col>
                <Col md={12} xs={24}>
                    <ProFormDigit
                        rules={[{ required: true, message: 'Vui lòng nhập số tiền' }]}
                        fieldProps={{
                            formatter: (value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ','),
                            parser: (value) => value?.replace(/(,*)/g, '') as unknown as number
                        }}
                        name="amount" label="Số tiền" />
                </Col>
                <Col md={12} xs={24}>
                    <ProFormSelect name="paymentMethod" label="Hình thức thanh toán" options={[
                        { label: 'Tiền mặt', value: PaymentMethod.Cash },
                        { label: 'Chuyển khoản', value: PaymentMethod.BankTransfer },
                        { label: 'Quẹt thẻ', value: PaymentMethod.Card }
                    ]} rules={[{ required: true, message: 'Vui lòng chọn hình thức thanh toán' }]} />
                </Col>
                <Col md={12} xs={24}>
                    <ProFormDatePicker name="createdAt" label="Ngày thu" rules={[{ required: true, message: 'Vui lòng chọn ngày thu' }]}
                        width="lg"
                    />
                </Col>


            </Row>
            <ProFormTextArea name="note" label="Ghi chú" />
        </ModalForm>
    );
}
export default InvoiceForm;