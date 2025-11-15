import { apiAddLeadFeedback, apiGetLeadFeedback, apiGetSmDosOptions, apiUpdateLeadFeedback } from "@/services/contact";
import { apiEventTableOptions } from "@/services/event";
import { apiJobKindOptions } from "@/services/settings/job-kind";
import { apiSourceOptions } from "@/services/settings/source";
import { apiTransportOptions } from "@/services/settings/transport";
import { DrawerForm, DrawerFormProps, ProFormInstance, ProFormSelect, ProFormText, ProFormTimePicker } from "@ant-design/pro-components";
import { Col, message, Row } from "antd";
import dayjs from "dayjs";
import { useEffect, useRef } from "react";

type Props = DrawerFormProps & {
    reload: any;
    eventDate: string;
    eventId?: string;
}

const LeadFeedback: React.FC<Props> = (props) => {
    const formRef = useRef<ProFormInstance>();

    useEffect(() => {
        if (props.id && props.open) {
            apiGetLeadFeedback(props.id).then(response => {
                formRef.current?.setFields([
                    {
                        name: 'toById',
                        value: response.toById
                    },
                    {
                        name: 'financialSituation',
                        value: response.financialSituation
                    },
                    {
                        name: 'interestLevel',
                        value: response.interestLevel
                    },
                    {
                        name: 'id',
                        value: response.id
                    },
                    {
                        name: 'tableId',
                        value: response.tableId
                    },
                    {
                        name: 'jobKindId',
                        value: response.jobKindId
                    },
                    {
                        name: 'checkoutTime',
                        value: response.checkoutTime ? dayjs(response.checkoutTime, 'hh:mm:ss') : null
                    },
                    {
                        name: 'sourceId',
                        value: response.sourceId
                    }
                ]);
            })
        }
    }, [props.id, props.open]);

    return (
        <DrawerForm
            formRef={formRef}
            {...props} title="Feedback Form" onFinish={async (values) => {
                values.leadId = props.id;
                values.eventId = props.eventId;
                values.eventDate = props.eventDate;
                if (values.id) {
                    await apiUpdateLeadFeedback(values);
                } else {
                    await apiAddLeadFeedback(values);
                }
                message.success('Thành công!');
                props.onOpenChange?.(false);
                props.reload();
            }}
        >
            <ProFormText hidden name="id" />
            <Row gutter={[16, 16]}>
                <Col md={8} xs={12}>
                    <ProFormSelect request={apiGetSmDosOptions} name="toById" label="Người T.O" showSearch />
                </Col>
                <Col md={8} xs={12}>
                    <ProFormSelect options={[
                        'Thấp (không có tiết kiệm)',
                        'Trung bình (có tích lũy dưới 50M)',
                        'Khá (có tích lũy dưới 200M)',
                        'Tốt (có tích lũy trên 200M)'
                    ]} name="financialSituation" label="Tình hình tài chính" />
                </Col>
                <Col md={8} xs={12}>
                    <ProFormSelect options={['1', '2', '3', '4', '5', '6', '7', '8', '9', '10']} name="interestLevel" label="Mức độ quan tâm" />
                </Col>
                <Col md={8} xs={24}>
                    <ProFormSelect label="Nghề nghiệp" name="jobKindId" request={apiJobKindOptions} showSearch />
                </Col>
                <Col md={8} xs={12}>
                    <ProFormSelect name="tableId" label="Bàn"
                        request={apiEventTableOptions}
                        params={{ eventDate: props.eventDate, eventId: props.eventId }}
                        showSearch />
                </Col>
                <Col md={8} xs={12}>
                    <ProFormTimePicker name="checkoutTime" label="Giờ Check-out" width="lg" />
                </Col>
                <Col md={12} xs={12}>
                    <ProFormSelect name={"transportId"} label="Phương tiện" request={apiTransportOptions} showSearch />
                </Col>
                <Col md={12} xs={12}>
                    <ProFormSelect name="sourceId" label="Nguồn" request={apiSourceOptions} />
                </Col>
            </Row>
        </DrawerForm>
    )
}

export default LeadFeedback;