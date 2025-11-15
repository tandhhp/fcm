import { apiCloseDeal } from "@/services/event";
import { apiCardOptions } from "@/services/settings/card";
import { ModalForm, ModalFormProps, ProDescriptions, ProFormDigit, ProFormInstance, ProFormSelect, ProFormText } from "@ant-design/pro-components";
import { Col, message, Row } from "antd";
import { useRef } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const CloseDealForm: React.FC<Props> = (props) => {
    const formRef = useRef<ProFormInstance>();

    const onFinish = async (values: any) => {
        if (!props.data) {
            message.warning('Không tìm thấy dữ liệu');
            return false;
        }
        values.leadId = props.data.id;
        await apiCloseDeal(values);
        message.success('Chốt deal thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} formRef={formRef} title="Chốt Deal" onFinish={onFinish}>
            <ProDescriptions column={2} title="Thông tin hợp đồng" bordered size="small" className="mb-4">
                <ProDescriptions.Item label="Họ và tên">{props.data?.name}</ProDescriptions.Item>
                <ProDescriptions.Item label="Số điện thoại">{props.data?.phoneNumber}</ProDescriptions.Item>
                <ProDescriptions.Item label="Số CCCD">{props.data?.identityNumber}</ProDescriptions.Item>
                <ProDescriptions.Item label="Rep tiếp">{props.data?.salesName}</ProDescriptions.Item>
                <ProDescriptions.Item label="Team Key-In">{props.data?.tmKeyIn || props.data?.smKeyIn}</ProDescriptions.Item>
                <ProDescriptions.Item label="Key-In">{props.data?.creatorName}</ProDescriptions.Item>
                <ProDescriptions.Item label="T.O">{props.data?.toName}</ProDescriptions.Item>
            </ProDescriptions>
            <Row gutter={16}>
                <Col md={8} xs={24}>
                    <ProFormText name="contractCode" label="Số hợp đồng" rules={[
                        { required: true, message: 'Vui lòng nhập số hợp đồng' }
                    ]} />
                </Col>
                <Col md={8} xs={24}>
                    <ProFormSelect name="cardId" label="Thẻ" request={apiCardOptions} />
                </Col>
                <Col md={8} xs={24}>
                    <ProFormDigit name="contractAmount" label="GTHĐ" fieldProps={{
                        formatter: (value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ','),
                        parser: (value) => value?.replace(/(,*)/g, '') as unknown as number
                    }}
                        rules={[
                            { required: true, message: 'Vui lòng nhập GTHĐ' }
                        ]}
                    />
                </Col>
            </Row>
        </ModalForm>
    )
}

export default CloseDealForm;