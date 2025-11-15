import { apiDistrictOptions } from "@/services/settings/district";
import { apiProvinceOptions } from "@/services/settings/province";
import { apiUserDetail, apiUserProfileUpdate } from "@/services/user";
import { GENDER_OPTIONS } from "@/utils/constants";
import { ProForm, ProFormDatePicker, ProFormInstance, ProFormSelect, ProFormText } from "@ant-design/pro-components";
import { useParams } from "@umijs/max";
import { Col, message, Row } from "antd";
import { useEffect, useRef } from "react";

const ProfileTab: React.FC = () => {

    const { id } = useParams();
    const formRef = useRef<ProFormInstance>();

    useEffect(() => {
        apiUserDetail(id).then(res => {
            const response = res.data;
            formRef.current?.setFields([
                {
                    name: 'name',
                    value: response.name
                },
                {
                    name: 'gender',
                    value: response.gender
                },
                {
                    name: 'address',
                    value: response.address
                },
                {
                    name: 'phoneNumber',
                    value: response.phoneNumber
                },
                {
                    name: 'email',
                    value: response.email
                },
                {
                    name: 'address',
                    value: response.address
                },
                {
                    name: 'branch',
                    value: response.branch
                },
                {
                    name: 'identityNumber',
                    value: response.identityNumber
                },
                {
                    name: 'identityAddress',
                    value: response.identityAddress
                },
                {
                    name: 'identityDate',
                    value: response.identityDate
                }
            ]);
        })
    }, []);

    return (
        <>
            <Row gutter={16}>
                <Col md={24} xs={24}>
                    <ProForm formRef={formRef} onFinish={async (values) => {
                        await apiUserProfileUpdate(values);
                        message.success('Cập nhật thành công!');
                    }}>
                        <ProFormText name="name" label="Họ và tên" rules={[
                            {
                                required: true
                            }
                        ]} />
                        <Row gutter={16}>
                            <Col md={6} xs={12}>
                                <ProFormSelect name="gender" label="Giới tính" options={GENDER_OPTIONS} />
                            </Col>
                            <Col md={6} xs={12}>
                                <ProFormDatePicker label="Ngày sinh" name="dateOfBirth" width="lg" />
                            </Col>
                            <Col md={6} xs={12}>
                                <ProFormText label="Email" name="email" />
                            </Col>
                            <Col md={6} xs={12}>
                                <ProFormText label="SDT" name="phoneNumber" rules={[
                                    {
                                        required: true
                                    }
                                ]} />
                            </Col>
                            <Col md={8} xs={12}>
                                <ProFormText label="Số CCCD" name="identityNumber" />
                            </Col>
                            <Col md={8} xs={12}>
                                <ProFormDatePicker label="Ngày cấp" name="identityDate" width="xl" />
                            </Col>
                            <Col md={8} xs={12}>
                                <ProFormText label="Nơi cấp" name="identityAddress" />
                            </Col>
                            <Col md={4} xs={24}>
                                <ProFormSelect name="provinceId" label="Tỉnh/Thành phố" request={apiProvinceOptions} showSearch />
                            </Col>
                            <Col md={4} xs={24}>
                                <ProFormSelect name="districtId" label="Xã/Phường" request={apiDistrictOptions} showSearch />
                            </Col>
                            <Col md={16} xs={24}>
                                <ProFormText name="address" label="Địa chỉ" />
                            </Col>
                        </Row>
                    </ProForm>
                </Col>
            </Row>
        </>
    )
}

export default ProfileTab;