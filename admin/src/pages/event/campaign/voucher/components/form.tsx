import { apiCampaignOptions } from "@/services/event/campaign";
import { apiVoucherAdd, apiVoucherUpdate } from "@/services/event/voucher";
import { ModalForm, ModalFormProps, ProFormDigit, ProFormInstance, ProFormSelect, ProFormText, ProFormTextArea } from "@ant-design/pro-components"
import { Col, message, Row } from "antd";
import { useEffect, useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const VoucherForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);

    useEffect(() => {
        if (props.data && props.open) {
            formRef.current?.setFields([
                {
                    name: 'id',
                    value: props.data.id
                },
                {
                    name: 'name',
                    value: props.data.name
                },
                {
                    name: 'campaignId',
                    value: props.data.campaignId
                },
                {
                    name: 'expiredDays',
                    value: props.data.expiredDays
                },
                {
                    name: 'status',
                    value: props.data.status
                },
                {
                    name: 'note',
                    value: props.data.note
                }
            ])
        }
    }, [props.data, props.open]);

    const onFinish = async (values: any) => {
        if (props.data) {
            await apiVoucherUpdate(values);
        } else {
            await apiVoucherAdd(values);
        }
        message.success('Thành công!');
        props.reload?.();
        formRef.current?.resetFields();
        return true;
    }

    return (
        <ModalForm {...props} title="Cài đặt Voucher" autoFocusFirstInput
            onFinish={onFinish}
            formRef={formRef}
        >
            <ProFormText name="id" hidden />
            <ProFormText name="name" label="Mã voucher" rules={[{ required: true }]} />
            <Row gutter={16}>
                <Col md={8} sm={24}>
                    <ProFormSelect name="campaignId" label="Loại" rules={[{ required: true }]} request={apiCampaignOptions} />
                </Col>
                <Col md={8} sm={24}>
                    <ProFormDigit name="expiredDays" label="Số ngày hết hạn" rules={[{ required: true }]} />
                </Col>
                <Col md={8} sm={24}>
                    <ProFormSelect name={"status"} label="Trạng thái" options={[
                        {
                            value: 0,
                            label: 'Chưa sử dụng'
                        },
                        {
                            value: 1,
                            label: 'Đã sử dụng'
                        },
                        {
                            value: 2,
                            label: 'Hết hạn'
                        },
                        {
                            value: 3,
                            label: 'Đã kích hoạt'
                        },
                        {
                            value: 4,
                            label: 'Đã hủy'
                        },
                        {
                            value: 5,
                            label: 'Từ chối'
                        }
                    ]} rules={[{ required: true }]} />
                </Col>
            </Row>
            <ProFormTextArea name="note" label="Ghi chú" />
        </ModalForm>
    )
}

export default VoucherForm;