import { apiContactAssignSource } from "@/services/contact";
import { apiTelesalesOptions } from "@/services/role";
import { apiSourceOptions } from "@/services/settings/source";
import { UsergroupAddOutlined } from "@ant-design/icons";
import { ModalForm, ProFormDigit, ProFormInstance, ProFormSelect } from "@ant-design/pro-components"
import { Button, Col, message, Row } from "antd";
import { useRef } from "react";

type Props = {
    reload?: () => void;
}

const ContactAssignForm: React.FC<Props> = ({ reload }) => {

    const formRef = useRef<ProFormInstance>();

    const onFinish = async (values: any) => {
        await apiContactAssignSource(values);
        message.success('Phân công liên hệ thành công');
        formRef.current?.resetFields();
        reload?.();
        return true;
    }

    return (
        <ModalForm title="Phân công liên hệ"
            formRef={formRef}
            onFinish={onFinish}
            trigger={<Button type="primary" icon={<UsergroupAddOutlined />}>Phân công</Button>}>
            <Row gutter={16}>
                <Col md={12} xs={24}>
                    <ProFormSelect name="sourceId" label="Nguồn liên hệ" placeholder="Chọn nguồn liên hệ" request={apiSourceOptions} showSearch
                        rules={[{ required: true, message: 'Vui lòng chọn nguồn liên hệ' }]} />
                </Col>
                <Col md={12} xs={24}>
                    <ProFormDigit name="numberOfContact" label="Số lượng liên hệ" placeholder="Nhập số lượng liên hệ"
                        rules={[{ required: true, message: 'Vui lòng nhập số lượng liên hệ' }]} />
                </Col>
            </Row>
            <ProFormSelect name="telesalesId" label="Nhân viên telesales" placeholder="Chọn nhân viên telesales" showSearch
                request={apiTelesalesOptions}
                rules={[{ required: true, message: 'Vui lòng chọn nhân viên telesales' }]} />
        </ModalForm>
    )
}

export default ContactAssignForm;