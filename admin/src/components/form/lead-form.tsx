import { apiAddLead } from "@/services/contact";
import { apiEventKeyInOptions, apiEventOptions } from "@/services/event";
import { apiSalesManagerOptions } from "@/services/role";
import { apiBranchOptions } from "@/services/settings/branch";
import { apiLeadDetail, apiLeadUpdate } from "@/services/users/lead";
import { GENDER_OPTIONS } from "@/utils/constants";
import { DrawerForm, DrawerFormProps, ProFormDatePicker, ProFormInstance, ProFormList, ProFormSelect, ProFormText, ProFormTextArea } from "@ant-design/pro-components";
import { useAccess, useModel } from "@umijs/max";
import { Col, message, Row } from "antd";
import dayjs from "dayjs";
import { useEffect, useRef, useState } from "react";

type Props = DrawerFormProps & {
    data?: any;
    reload?: () => void;
}

const LeadForm: React.FC<Props> = (props) => {

    const access = useAccess();
    const formRef = useRef<ProFormInstance>(null);
    const { initialState } = useModel('@@initialState');
    const [salesManagerId, setSalesManagerId] = useState<string | undefined>(undefined);
    const [salesOptions, setSalesOptions] = useState<any[]>([]);

    useEffect(() => {
        if (props.data && props.open) {
            apiLeadDetail(props.data.id).then(response => {
                const data = response.data;
                formRef.current?.setFields([
                    {
                        name: 'id',
                        value: data.id
                    },
                    {
                        name: 'name',
                        value: data.name
                    },
                    {
                        name: 'gender',
                        value: data.gender
                    },
                    {
                        name: 'dateOfBirth',
                        value: data.dateOfBirth
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
                        name: 'eventDate',
                        value: data.eventDate
                    },
                    {
                        name: 'eventId',
                        value: data.eventId
                    },
                    {
                        name: 'branchId',
                        value: data.branchId
                    },
                    {
                        name: 'note',
                        value: data.note
                    },
                    {
                        name: 'createdBy',
                        value: data.createdBy
                    },
                    {
                        name: 'subLeads',
                        value: data.subLeads
                    },
                    {
                        name: 'creatorLeaderId',
                        value: data.creatorLeaderId
                    },
                    {
                        name: 'identityNumber',
                        value: data.identityNumber
                    }
                ]);
                setSalesManagerId(data.creatorLeaderId);
            });
        }
        if (!props.data) {
            if (access.sales) {
                apiEventKeyInOptions().then(response => {
                    setSalesOptions(response);
                    formRef.current?.setFieldValue('createdBy', initialState?.currentUser?.id);
                });
            }
        }
    }, [props.data, props.open, access]);

    useEffect(() => {
        if (salesManagerId && props.open) {
            apiEventKeyInOptions({ salesManagerId }).then(response => {
                setSalesOptions(response);
            });
        }
    }, [salesManagerId, props.open]);

    const onFinish = async (values: any) => {
        const body = {
            ...values,
            eventDate: dayjs(values.eventDate).format('YYYY-MM-DD'),
            dateOfBirth: values.dateOfBirth ? dayjs(values.dateOfBirth).format('YYYY-MM-DD') : undefined
        }
        if (props.data) {
            await apiLeadUpdate(body);
        } else {
            await apiAddLead(body);
        }
        message.success('Thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <DrawerForm {...props} formRef={formRef} title="Cài đặt Key-In" onFinish={onFinish} width={1000}>
            <ProFormText name="id" hidden />
            <Row gutter={16}>
                <Col xs={12} md={10}>
                    <ProFormText name="name" label="Họ và tên" rules={[
                        {
                            required: true
                        }
                    ]} />
                </Col>
                <Col xs={12} md={6}>
                    <ProFormText name="identityNumber" label="Số CCCD" rules={[
                        {
                            required: true
                        }
                    ]} />
                </Col>
                <Col xs={12} md={4}>
                    <ProFormDatePicker.Year name="dateOfBirth" label="Năm sinh" fieldProps={{
                        className: 'w-full'
                    }} />
                </Col>
                <Col xs={12} md={4}>
                    <ProFormSelect label="Giới tính" name='gender' options={GENDER_OPTIONS} />
                </Col>
                <Col xs={12} md={6}>
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
                <Col xs={12} md={6}>
                    <ProFormText name="email" label="Email" rules={[
                        {
                            type: 'email'
                        }
                    ]} />
                </Col>
                <Col xs={12} md={6}>
                    <ProFormSelect label="Team Key-In" name={"creatorLeaderId"} request={apiSalesManagerOptions} showSearch
                        disabled={access.sales}
                        onChange={(value: string) => {
                            setSalesManagerId(value);
                            formRef.current?.setFieldsValue({
                                createdBy: undefined
                            });
                            setSalesOptions([]);
                        }}
                    />
                </Col>
                <Col xs={12} md={6}>
                    <ProFormSelect label="Người Key-In" name='createdBy' options={salesOptions} rules={[
                        {
                            required: true
                        }
                    ]} showSearch disabled={access.sales} />
                </Col>
                <Col xs={12} md={6}>
                    <ProFormSelect request={apiBranchOptions} name="branchId" label="Chi nhánh" allowClear={false} initialValue={1} rules={[
                        {
                            required: true
                        }
                    ]} />
                </Col>
                <Col xs={12} md={8}>
                    <ProFormText name="address" label="Địa chỉ" />
                </Col>
                <Col md={6} xs={12}>
                    <ProFormDatePicker name="eventDate" label="Ngày sự kiện" rules={[
                        {
                            required: true
                        }
                    ]} fieldProps={{
                        format: {
                            type: 'mask',
                            format: 'DD-MM-YYYY'
                        },
                        className: 'w-full'
                    }} />
                </Col>
                <Col md={4} xs={12}>
                    <ProFormSelect name="eventId" label="Khung giờ" request={apiEventOptions} rules={[
                        {
                            required: true
                        }
                    ]} allowClear={false} />
                </Col>
            </Row>
            <ProFormTextArea label="Ghi chú" name="note" />
            <ProFormList name="subLeads" label="Khách phụ" copyIconProps={{
                tooltipText: 'Thêm khách phụ'
            }} deleteIconProps={{
                tooltipText: 'Xóa khách phụ'
            }}>
                <div className="flex gap-4">
                    <ProFormText name="name" label="Họ và tên" rules={[{ required: true, message: 'Vui lòng nhập họ và tên' }]} />
                    <ProFormText name="phoneNumber" label="Số điện thoại" rules={[
                        { required: true, message: 'Vui lòng nhập số điện thoại' },
                        {
                            pattern: /((09|03|07|08|05)+([0-9]{8})\b)/,
                            message: 'Số điện thoại không hợp lệ'
                        }]} />
                </div>
            </ProFormList>
        </DrawerForm>
    )
}

export default LeadForm;