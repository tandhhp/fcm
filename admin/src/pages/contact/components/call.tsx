import { apiCallComplete, apiCallOptions } from "@/services/call";
import { apiEventOptions } from "@/services/event";
import { CALL_STATUS_CODE } from "@/utils/constants";
import { PhoneOutlined } from "@ant-design/icons";
import { DrawerForm, DrawerFormProps, ProDescriptions, ProFormDatePicker, ProFormInstance, ProFormSelect, ProFormText, ProFormTextArea, ProFormTimePicker } from "@ant-design/pro-components"
import { Button, Col, message, Row } from "antd";
import dayjs, { Dayjs } from "dayjs";
import { useRef, useState } from "react";

type Props = DrawerFormProps & {
    data?: any;
    reload?: () => void;
}

const CallForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);
    const [callStatusType, setCallStatusType] = useState<number>();
    const [selectedStatus, setSelectedStatus] = useState<string>();

    const onFinish = async (values: any) => {
        if (!props.data) {
            message.error('Liên hệ không tồn tại');
            return false;
        }
        values.contactId = props.data?.id;
        await apiCallComplete(values);
        message.success('Lưu thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <DrawerForm {...props} title={`Cuộc gọi ${props.data?.name}`} onFinish={onFinish} formRef={formRef}
            drawerProps={{
                destroyOnHidden: true,
                closable: false,
                  maskClosable: false
            }}
        >
            <ProDescriptions column={2} className="mb-4" bordered size="small" title="Thông tin liên hệ" dataSource={props.data} >
                <ProDescriptions.Item label="Họ và tên">{props.data?.name} {props.data?.gender === true ? '(Nữ)' : props.data?.gender === false ? '(Nam)' : ''}</ProDescriptions.Item>
                <ProDescriptions.Item label="Số điện thoại">{props.data?.phoneNumber}</ProDescriptions.Item>
                <ProDescriptions.Item label="Họ và tên 2">{props.data?.name2}</ProDescriptions.Item>
                <ProDescriptions.Item label="Số điện thoại 2">{props.data?.phoneNumber2}</ProDescriptions.Item>
                <ProDescriptions.Item label="Nguồn">{props.data?.sourceName}</ProDescriptions.Item>
                <ProDescriptions.Item label="Ngày tạo" valueType={"date"}>{props.data?.createdDate}</ProDescriptions.Item>
            </ProDescriptions>
            <Button type="primary" icon={<PhoneOutlined />} block href={`tel:${props.data?.phoneNumber}`} className="mb-4">Gọi điện</Button>
            <Row gutter={16}>
                <Col xs={24} md={12}>
                    <ProFormSelect
                        name="callStatusType"
                        label="Loại trạng thái cuộc gọi"
                        options={[
                            {
                                label: 'Không nghe máy',
                                value: 1
                            },
                            {
                                label: 'Thuê bao',
                                value: 2
                            },
                            {
                                label: 'Sai số',
                                value: 3
                            },
                            {
                                label: 'Ngoại tỉnh',
                                value: 4
                            },
                            {
                                label: 'Gọi lại sau',
                                value: 5
                            },
                            {
                                label: 'Khách đạt yêu cầu',
                                value: 6
                            },
                            {
                                label: 'Khách không đạt yêu cầu',
                                value: 7
                            }
                        ]}
                        fieldProps={{
                            allowClear: true,
                            onChange: (value: number) => {
                                setCallStatusType(value);
                                setSelectedStatus(undefined);
                                formRef.current?.setFieldValue('callStatusId', undefined);
                            }
                        }}
                        showSearch
                    />
                </Col>
                <Col xs={24} md={12}>
                    <ProFormSelect name={`callStatusId`}
                        params={{ type: callStatusType }}
                        dependencies={['callStatusType']}
                        onChange={(_, option) => {
                            setSelectedStatus(option.code);
                        }}
                        label="Trạng thái" request={(params) => apiCallOptions(params)} showSearch
                        disabled={!callStatusType}
                        rules={[
                            {
                                required: true
                            }
                        ]} />
                </Col>
                <Col xs={24} md={12}>
                    <ProFormSelect name={`extraStatus`} label="Extra Status" options={[
                        {
                            label: 'Có tiền',
                            value: 'Có tiền'
                        },
                        {
                            label: 'Có thói quen',
                            value: 'Có thói quen'
                        },
                        {
                            label: 'Có cả 2',
                            value: 'Có cả 2'
                        }
                    ]} />
                </Col>
                <Col xs={24} md={12}>
                    <ProFormDatePicker name="followUpDate" label="Follow up date" width="lg" />
                </Col>
                <Col xs={24} md={8}>
                    <ProFormTimePicker name="followUpTime" label="Follow up time" width="lg" />
                </Col>
                <Col xs={24} md={8}>
                    <ProFormText label="Nghề nghiệp" name="job" />
                </Col>
                <Col xs={24} md={8}>
                    <ProFormText label="Tuổi" name="age" />
                </Col>
            </Row>
            <ProFormText label="Thói quen du lịch" name="travelHabit" />
            <ProFormTextArea label="Ghi chú" name="note" />
            {
                (selectedStatus === CALL_STATUS_CODE.CONFIRM1 || selectedStatus === CALL_STATUS_CODE.CONSIDER) && (
                    <>
                        <Row gutter={16}>
                            <Col xs={24} md={12}>
                                <ProFormDatePicker name="eventDate" label="Event date" width="lg"
                                    formProps={{
                                        disabledDate: (current: Dayjs) => current && current < dayjs().startOf('day')
                                    }}
                                />
                            </Col>
                            <Col xs={24} md={12}>
                                <ProFormSelect name="eventId" label="Khung giờ" request={apiEventOptions} showSearch />
                            </Col>
                        </Row>
                    </>
                )
            }
        </DrawerForm>
    )
}

export default CallForm;