import { apiAttendanceOptions } from "@/services/event/attendance";
import { apiVoucherOptions } from "@/services/event/voucher";
import { apiSalesManagerOptions } from "@/services/role";
import { apiJobKindOptions } from "@/services/settings/job-kind";
import { apiTableOptions } from "@/services/settings/table";
import { apiTransportOptions } from "@/services/settings/transport";
import { apiDosOptions, apiSalesOptions, apiSmOptions, apiUserOptions } from "@/services/user";
import { apiLeadDetail, apiLeadUpdateFeedback } from "@/services/users/lead";
import { GENDER_OPTIONS } from "@/utils/constants";
import { DrawerForm, DrawerFormProps, ProFormDatePicker, ProFormInstance, ProFormSelect, ProFormText, ProFormTextArea } from "@ant-design/pro-components";
import { useAccess } from "@umijs/max";
import { Col, message, Row } from "antd";
import dayjs from "dayjs";
import { useEffect, useRef, useState } from "react";

type Props = DrawerFormProps & {
    data?: any;
    reload?: () => void;
}

const LeadFeedbackUpdateForm: React.FC<Props> = (props) => {

    const access = useAccess();
    const formRef = useRef<ProFormInstance>(null);
    const [smOptions, setSmOptions] = useState<any[]>([]);
    const [salesOptions, setSalesOptions] = useState<any[]>([]);
    const [salesManagerId, setSalesManagerId] = useState<string | undefined>();
    const [dosId, setDosId] = useState<string | undefined>();

    useEffect(() => {
        if (props.data && props.open) {

        }
    }, [props.data, props.open]);

    useEffect(() => {
        if (dosId && props.open) {
            apiSmOptions(dosId).then(response => {
                setSmOptions(response);
            });
        }
    }, [dosId, props.open]);

    useEffect(() => {
        if (salesManagerId && props.open) {
            apiSalesOptions({ salesManagerId }).then(response => {
                setSalesOptions(response);
            });
        }
    }, [salesManagerId, props.open]);

    useEffect(() => {
        if (props.data && props.open) {
            apiLeadDetail(props.data.id).then(response => {
                const data = response.data;
                formRef.current?.setFields([
                    {
                        name: 'dateOfBirth',
                        value: data.dateOfBirth ? dayjs(data.dateOfBirth) : undefined
                    },
                    {
                        name: 'dosId',
                        value: data.dosId
                    },
                    {
                        name: 'salesManagerId',
                        value: data.salesManagerId
                    },
                    {
                        name: 'salesId',
                        value: data.salesId
                    },
                    {
                        name: 'toById',
                        value: data.toById
                    },
                    {
                        name: 'tableId',
                        value: data.tableId
                    },
                    {
                        name: 'attendanceId',
                        value: data.attendanceId
                    },
                    {
                        name: 'interestLevel',
                        value: data.interestLevel
                    },
                    {
                        name: 'financialSituation',
                        value: data.financialSituation
                    },
                    {
                        name: 'jobKindId',
                        value: data.jobKindId
                    },
                    {
                        name: 'transportId',
                        value: data.transportId
                    },
                    {
                        name: 'note',
                        value: data.note
                    },
                    {
                        name: 'id',
                        value: data.id
                    },
                    {
                        name: 'name',
                        value: data.name
                    },
                    {
                        name: 'phoneNumber',
                        value: data.phoneNumber
                    },
                    {
                        name: 'email',
                        value: data.email
                    },
                    {
                        name: 'address',
                        value: data.address
                    },
                    {
                        name: 'gender',
                        value: data.gender
                    },
                    {
                        name: 'createdBy',
                        value: data.createdBy
                    },
                    {
                        name: 'voucher1Id',
                        value: data.voucher1Id
                    },
                    {
                        name: 'voucher2Id',
                        value: data.voucher2Id
                    },
                    {
                        name: 'identityNumber',
                        value: data.identityNumber
                    }
                ]);
                setDosId(data.dosId);
                setSalesManagerId(data.salesManagerId);
            });
        }
    }, [props.data, props.open]);

    const onFinish = async (values: any) => {
        await apiLeadUpdateFeedback({
            ...values,
            dateOfBirth: values.dateOfBirth ? dayjs(values.dateOfBirth).format('YYYY-MM-DD') : undefined
        });
        message.success('Cập nhật thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <DrawerForm {...props} title={`Cập nhật khách hàng ${props.data?.name}`} formRef={formRef} onFinish={onFinish} width={800}>
            <ProFormText name="id" hidden />
            <Row gutter={16}>
                <Col md={8} xs={24}>
                    <ProFormText name={"name"} label="Họ và tên" rules={[{ required: true }]} />
                </Col>
                <Col md={6} xs={24}>
                    <ProFormText name={"identityNumber"} label="Số CCCD" rules={[
                        { required: true, message: 'Vui lòng nhập CCCD' }
                    ]} />
                </Col>
                <Col md={6} xs={24}>
                    <ProFormDatePicker name={"dateOfBirth"} label="Năm sinh" width="xl" />
                </Col>
                <Col md={4} xs={24}>
                    <ProFormSelect name={"gender"} label="Giới tính" options={GENDER_OPTIONS} />
                </Col>
                <Col xs={12} md={8}>
                    <ProFormText name="phoneNumber" label="Số điện thoại" rules={[
                        {
                            required: true
                        },
                        {
                            pattern: /((09|03|07|08|05)+([0-9]{8})\b)/,
                            message: 'Số điện thoại không hợp lệ'
                        }
                    ]} />
                </Col>
                <Col xs={12} md={8}>
                    <ProFormText name="email" label="Email" rules={[
                        {
                            type: 'email'
                        }
                    ]} />
                </Col>
                <Col md={8} xs={24}>
                    <ProFormSelect name={"createdBy"} label="Người Key-In" request={apiUserOptions} showSearch />
                </Col>
                <Col md={8} xs={24}>
                    <ProFormSelect name={"toById"} label="Người T.O" request={apiSalesManagerOptions} showSearch />
                </Col>
                <Col md={8} xs={24}>
                    <ProFormSelect name={"tableId"} label="Bàn" request={apiTableOptions} />
                </Col>
                <Col md={8} xs={24}>
                    <ProFormSelect name={"attendanceId"} label="Trạng thái tham dự" request={apiAttendanceOptions} showSearch />
                </Col>
                <Col md={8} xs={12}>
                    <ProFormSelect options={['1', '2', '3', '4', '5', '6', '7', '8', '9', '10']} name="interestLevel" label="Mức độ quan tâm" />
                </Col>
                <Col md={8} xs={12}>
                    <ProFormSelect options={[
                        'Thấp (không có tiết kiệm)',
                        'Trung bình (có tích lũy dưới 50M)',
                        'Khá (có tích lũy dưới 200M)',
                        'Tốt (có tích lũy trên 200M)'
                    ]} name="financialSituation" label="Tình hình tài chính" />
                </Col>
                <Col md={8} xs={24}>
                    <ProFormSelect label="Nghề nghiệp" name="jobKindId" request={apiJobKindOptions} showSearch />
                </Col>
                <Col md={8} xs={24}>
                    <ProFormSelect name={"transportId"} label="Phương tiện" request={apiTransportOptions} showSearch />
                </Col>
                <Col md={8} xs={24}>
                    <ProFormText name="address" label="Địa chỉ" />
                </Col>
                <Col md={4} xs={24}>
                    <ProFormSelect name={"voucher1Id"} label="Voucher 1" request={apiVoucherOptions} params={{
                        campaignId: 1
                    }} showSearch fieldProps={{
                        popupMatchSelectWidth: false
                    }} />
                </Col>
                <Col md={4} xs={24}>
                    <ProFormSelect name={"voucher2Id"} label="Voucher 2" request={apiVoucherOptions}
                        params={{
                            campaignId: 2
                        }}
                        showSearch fieldProps={{
                            popupMatchSelectWidth: false
                        }} />
                </Col>
            </Row>

            <Row gutter={16} hidden={access.sales}>
                <Col md={8} xs={24}>
                    <ProFormSelect name={`dosId`} label="DOS" request={apiDosOptions} showSearch disabled={access.sm}
                        fieldProps={{
                            onChange: (value: string) => {
                                setDosId(value);
                                formRef.current?.setFields([
                                    {
                                        name: 'salesManagerId',
                                        value: undefined
                                    }
                                ]);
                                setSalesManagerId(undefined);
                                setSalesOptions([]);
                            }
                        }}
                    />
                </Col>
                <Col md={8} xs={24}>
                    <ProFormSelect name="salesManagerId" label="Sales Manager" options={smOptions} showSearch disabled={access.sm} fieldProps={{
                        onChange: (value: string) => {
                            setSalesManagerId(value);
                            formRef.current?.setFields([
                                {
                                    name: 'salesId',
                                    value: undefined
                                }
                            ]);
                            setSalesOptions([]);
                        }
                    }}
                    />
                </Col>
                <Col md={8} xs={24}>
                    <ProFormSelect name="salesId" label="Người Rep" options={salesOptions} showSearch />
                </Col>
            </Row>
            <ProFormTextArea name={"note"} label="Ghi chú" />
        </DrawerForm>
    )
}

export default LeadFeedbackUpdateForm;