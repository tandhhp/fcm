import { apiContractPaymentCreate } from "@/services/finances/contract";
import { apiFileUpload, apiFileUploadMultiple } from "@/services/file-service";
import { PaymentMethod } from "@/utils/enum";
import { ModalForm, ModalFormProps, ProFormDatePicker, ProFormDigit, ProFormInstance, ProFormSelect, ProFormText, ProFormUploadDragger } from "@ant-design/pro-components";
import { Col, message, Row } from "antd";
import { useRef, useState } from "react";
import dayjs from "dayjs";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const ContractPayment: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>();
    const [fileUrlList, setFileUrlList] = useState<any[]>([]);

    const onFinish = async (values: any) => {
        if (!props.data) {
            message.warning('Chưa chọn hợp đồng');
            return false;
        }
        values.contractId = props.data.id;
        await apiContractPaymentCreate({
            contractId: props.data.id,
            amount: values.amount,
            paymentMethod: values.paymentMethod,
            evidenceUrls: fileUrlList?.map(item => item) ?? [],
            invoiceNumber: values.invoiceNumber,
            createdDate: values.createdDate ? dayjs(values.createdDate).format('YYYY-MM-DD') : dayjs().format('YYYY-MM-DD')
        });
        message.success('Tạo phiếu thu thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props}
            onFinish={onFinish}
            title={`Nộp tiền - Hợp đồng ${props.data?.contractCode}`} formRef={formRef}>
            <Row gutter={16}>
                <Col md={6} xs={24}>
                    <ProFormText name={"invoiceNumber"} label="Số phiếu thu" rules={[
                        { required: true, message: 'Vui lòng nhập số phiếu thu' }
                    ]} />
                </Col>
                <Col md={6} xs={24}>
                    <ProFormDigit name="amount" label="Số tiền nộp" min={1000} fieldProps={{
                        precision: 0, step: 1000,
                        formatter: (value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ','),
                        parser: (value) => value?.replace(/(,*)/g, '') as unknown as number
                    }} rules={[{ required: true, message: 'Vui lòng nhập số tiền nộp' }]} />
                </Col>
                <Col md={6} xs={24}>
                    <ProFormSelect name="paymentMethod" label="Hình thức thanh toán" options={[
                        { label: 'Tiền mặt', value: PaymentMethod.Cash },
                        { label: 'Chuyển khoản', value: PaymentMethod.BankTransfer },
                        { label: 'Quẹt thẻ', value: PaymentMethod.Card }
                    ]} rules={[{ required: true, message: 'Vui lòng chọn hình thức thanh toán' }]} />
                </Col>
                <Col md={6} xs={24}>
                    <ProFormDatePicker name="createdDate" label="Ngày thu" rules={[{ required: true, message: 'Vui lòng chọn ngày thu' }]}
                        width="md"
                        initialValue={dayjs()}
                    />
                </Col>
            </Row>
            <ProFormUploadDragger
                title="Tải lên chứng từ"
                description="Hỗ trợ tải lên file .png .jpg .jpeg"
                accept=".png,.jpg,.jpeg"
                name="evidence" label="Chứng từ" max={10} fieldProps={{
                    listType: 'picture',
                    beforeUpload: async (_, fileList) => {
                        const formData = new FormData();
                        fileList.forEach(file => {
                            formData.append('files', file);
                        });
                        const response = await apiFileUploadMultiple(formData);
                        message.success('Tải lên thành công');
                        const urls = response.data.map((item: any) => (item.url));
                        setFileUrlList(urls);
                        return false;
                    },
                    multiple: true
                }} />
            <ProFormText name={"evidenceUrl"} hidden />
        </ModalForm>
    )
}

export default ContractPayment;