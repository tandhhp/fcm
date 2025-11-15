import { apiContactCreate, apiContactDetail, apiContactUpdate } from "@/services/contact";
import { apiDistrictOptions } from "@/services/settings/district";
import { apiJobKindOptions } from "@/services/settings/job-kind";
import { apiProvinceOptions } from "@/services/settings/province";
import { apiTransportOptions } from "@/services/settings/transport";
import { apiUserOptions, apiUserTelesalesManagerOptions } from "@/services/user";
import { GENDER_OPTIONS } from "@/utils/constants";
import { DrawerForm, DrawerFormProps, ProFormInstance, ProFormSelect, ProFormText, ProFormTextArea } from "@ant-design/pro-components"
import { useAccess } from "@umijs/max";
import { Col, message, Row } from "antd";
import { useEffect, useRef, useState } from "react";

type Props = DrawerFormProps & {
    reload?: () => void;
    data?: any;
}

const ContactForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>();
    const [districtOptions, setDistrictOptions] = useState<any[]>([]);
    const [selectedProvinceId, setSelectedProvinceId] = useState<number | undefined>(undefined);
    const [telesalesManagerId, setTelesalesManagerId] = useState<number | undefined>(undefined);
    const [userOptions, setUserOptions] = useState<any[]>([]);
    const access = useAccess();

    useEffect(() => {
        if (telesalesManagerId) {
            apiUserOptions({ telesalesManagerId }).then((response) => {
                setUserOptions(response);
            });
        }
    }, [telesalesManagerId]);

    useEffect(() => {
        if (selectedProvinceId) {
            apiDistrictOptions({ provinceId: selectedProvinceId }).then((response) => {
                setDistrictOptions(response);
            });
        }
    }, [selectedProvinceId]);

    useEffect(() => {
        if (props.data && props.open) {
            apiContactDetail(props.data.id).then((response) => {
                formRef.current?.setFields([
                    {
                        name: 'id',
                        value: response.data.id
                    },
                    {
                        name: 'name',
                        value: response.data.name
                    },
                    {
                        name: 'gender',
                        value: response.data.gender
                    },
                    {
                        name: 'email',
                        value: response.data.email
                    },
                    {
                        name: 'phoneNumber',
                        value: response.data.phoneNumber
                    },
                    {
                        name: 'note',
                        value: response.data.note
                    },
                    {
                        name: 'jobKindId',
                        value: response.data.jobKindId
                    },
                    {
                        name: 'transportId',
                        value: response.data.transportId
                    },
                    {
                        name: 'provinceId',
                        value: response.data.provinceId
                    },
                    {
                        name: 'districtId',
                        value: response.data.districtId
                    },
                    {
                        name: 'userId',
                        value: response.data.userId
                    },
                    {
                        name: 'telesalesManagerId',
                        value: response.data.telesalesManagerId
                    },
                    {
                        name: 'marriedStatus',
                        value: response.data.marriedStatus
                    }
                ]);
                setSelectedProvinceId(response.data.provinceId);
                setTelesalesManagerId(response.data.telesalesManagerId);
            });
        }
    }, [props.open && props.data]);

    const onFinish = async (values: any) => {
        if (props.data) {
            await apiContactUpdate(values);
        } else {
            await apiContactCreate(values);
        }
        formRef.current?.resetFields();
        message.success("Thao tác thành công");
        props.reload?.();
        return true;
    }

    return (
        <DrawerForm {...props} title="Liên hệ" formRef={formRef} onFinish={onFinish}>
            <ProFormText name={"id"} hidden />
            <Row gutter={[16, 16]}>
                <Col xs={24} md={12}>
                    <ProFormText name="name" label="Họ và tên" rules={[{ required: true, message: "Vui lòng nhập họ và tên" }]} />
                </Col>
                <Col xs={24} md={12}>
                    <ProFormSelect name="gender" label="Giới tính" options={GENDER_OPTIONS} />
                </Col>
                <Col xs={24} md={12}>
                    <ProFormText name="email" label="Email" rules={[{ type: "email", message: "Vui lòng nhập email hợp lệ" }]} />
                </Col>
                <Col xs={24} md={12}>
                    <ProFormText name="phoneNumber" label="Số điện thoại" rules={[
                        {
                            pattern: /(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})\b/,
                            message: "Vui lòng nhập số điện thoại hợp lệ"
                        },
                        {
                            required: true
                        }
                    ]} />
                </Col>
                <Col xs={24} md={12} hidden={access.telesale}>
                    <ProFormSelect name={`telesalesManagerId`} label="Quản lý Telesales" request={apiUserTelesalesManagerOptions} showSearch
                        onChange={(value: number) => setTelesalesManagerId(value)}
                    />
                </Col>
                <Col xs={24} md={12} hidden={access.telesale}>
                    <ProFormSelect name={`userId`} label="Nhân viên phụ trách" showSearch options={userOptions} disabled={!telesalesManagerId} />
                </Col>
                <Col xs={24} md={12}>
                    <ProFormSelect name={`provinceId`} label="Tỉnh/Thành phố" request={apiProvinceOptions}
                        onChange={(value: number) => setSelectedProvinceId(value)}
                        showSearch />
                </Col>
                <Col xs={24} md={12}>
                    <ProFormSelect name={`districtId`} label="Xã/Phường" options={districtOptions} showSearch disabled={!selectedProvinceId} />
                </Col>
                <Col xs={24} md={8}>
                    <ProFormSelect name={`jobKindId`} label="Nghề nghiệp" request={apiJobKindOptions} showSearch />
                </Col>
                <Col xs={24} md={8}>
                    <ProFormSelect name={`transportId`} label="Phương tiện" request={apiTransportOptions} showSearch />
                </Col>
                <Col xs={24} md={8}>
                    <ProFormSelect name={`marriedStatus`} label="Tình trạng hôn nhân" options={[
                        { label: "Độc thân", value: 0 },
                        { label: "Đã kết hôn", value: 1 },
                        { label: "Ly hôn", value: 2 },
                        { label: "Góa", value: 3 }
                    ]} />
                </Col>
            </Row>
            <ProFormTextArea name="note" label="Ghi chú" />
        </DrawerForm>
    )
}

export default ContactForm;