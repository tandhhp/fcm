import { apiCallStatusCreate, apiCallStatusUpdate } from "@/services/call";
import { ModalForm, ModalFormProps, ProFormDigit, ProFormInstance, ProFormSelect, ProFormText } from "@ant-design/pro-components";
import { Col, message, Row } from "antd";
import { useEffect, useRef } from "react";
import { callStatusOptions } from "./call-status-options";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const CallStatusForm: React.FC<Props> = (props) => {

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
                    name: 'code',
                    value: props.data.code
                },
                {
                    name: 'type',
                    value: props.data.type
                },
                {
                    name: 'sortOrder',
                    value: props.data.sortOrder
                }
            ])
        } else {
            formRef.current?.resetFields();
        }
    }, [props.data, props.open]);

    const onFinish = async (values: any) => {
        if (props.data) {
            await apiCallStatusUpdate({ id: props.data.id, ...values });
        } else {
            await apiCallStatusCreate(values);
        }
        message.success('Thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Thông tin trạng thái cuộc gọi" formRef={formRef} onFinish={onFinish}>
            <ProFormText name="id" hidden />
            <Row gutter={16}>
                <Col span={8}>
                    <ProFormText name="code" label="Mã trạng thái"
                        rules={[{ required: true, message: 'Mã trạng thái là bắt buộc' }]}
                    />
                </Col>
                <Col span={12}>
                    <ProFormSelect name="type" label="Loại trạng thái" options={callStatusOptions} rules={[{ required: true, message: 'Loại trạng thái là bắt buộc' }]} />
                </Col>
                <Col span={4}>
                    <ProFormDigit name="sortOrder" label="Thứ tự hiển thị" min={0} />
                </Col>
            </Row>
            <ProFormText name="name" label="Tên trạng thái cuộc gọi" rules={[{ required: true, message: 'Tên trạng thái cuộc gọi là bắt buộc' }]} />
        </ModalForm>
    )
}

export default CallStatusForm;