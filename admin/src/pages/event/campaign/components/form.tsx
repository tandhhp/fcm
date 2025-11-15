import { apiCampaignCreate, apiCampaignDetail, apiCampaignUpdate } from "@/services/event/campaign";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormSelect, ProFormText } from "@ant-design/pro-components"
import { Col, message, Row } from "antd";
import { useEffect, useRef } from "react";
import { CampaignStatus } from "../enum";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const CampaignForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);

    useEffect(() => {
        if (props.data && props.open) {
            apiCampaignDetail(props.data?.id).then((res) => {
                formRef.current?.setFields([
                    {
                        name: 'id',
                        value: res.data.id
                    },
                    {
                        name: 'code',
                        value: res.data.code
                    },
                    {
                        name: 'name',
                        value: res.data.name
                    },
                    {
                        name: 'status',
                        value: res.data.status
                    }
                ])
            });
        }
    }, [props.data]);

    const onFinish = async (values: any) => {
        if (props.data) {
            await apiCampaignUpdate(values);
        } else {
            await apiCampaignCreate(values);
        }
        message.success("Thành công!");
        props.reload?.();
        formRef.current?.resetFields();
        return true;
    }

    return (
        <ModalForm {...props} title="Chiến dịch" formRef={formRef} onFinish={onFinish}>
            <ProFormText name={"id"} hidden />
            <Row gutter={16}>
                <Col xs={24} md={16}>
                    <ProFormText name={"code"} label="Mã chiến dịch" rules={[{ required: true }]} />
                </Col>
                <Col xs={24} md={8}>
                    <ProFormSelect
                        options={[
                            {
                                value: CampaignStatus.Inactive,
                                label: 'Không hoạt động'
                            },
                            {
                                value: CampaignStatus.Active,
                                label: 'Hoạt động'
                            },
                            {
                                value: CampaignStatus.Completed,
                                label: 'Hoàn thành'
                            }
                        ]}
                        allowClear={false}
                        name={"status"} label="Trạng thái" rules={[{ required: true }]} />
                </Col>
            </Row>
            <ProFormText name={"name"} label="Tên chiến dịch" rules={[{ required: true }]} />
        </ModalForm>
    )
}

export default CampaignForm;