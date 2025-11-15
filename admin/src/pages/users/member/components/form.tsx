import WfUpload from "@/components/file-explorer/upload";
import { apiBranchOptions } from "@/services/settings/branch";
import { apiCardOptions, apiSalesWithSmOptions, apiUserUpdate } from "@/services/user";
import { GENDER_OPTIONS } from "@/utils/constants";
import { ProFormText, ProFormDatePicker, ProFormSelect, ProFormDigit, ProFormTextArea, ProFormInstance, DrawerForm } from "@ant-design/pro-components";
import { Button, Col, message, Row } from "antd";
import dayjs from "dayjs";
import { useEffect, useRef, useState } from "react";

type Props = {
    user?: any;
    open: boolean;
    setOpen: any;
    actionRef: any;
    setUser?: any;
    // Được convert từ lead sang
    leadConvert?: boolean;
}

const CardHolderForm: React.FC<Props> = ({ user, open, setOpen, actionRef, setUser, leadConvert }) => {

    const formRef = useRef<ProFormInstance>();
    const [openUpload, setOpenUpload] = useState<boolean>(false);

    const onFinish = async (values: any) => {
        values.leadConvert = leadConvert;
        values.leadId = values.id;
        if (values.dateOfBirth) {
            values.dateOfBirth = dayjs(values.dateOfBirth).format('YYYY-MM-DD');
        }
        if (values.contractDate) {
            values.contractDate = dayjs(values.contractDate).format('YYYY-MM-DD');
        }
        if (values.identityDate) {
            values.identityDate = dayjs(values.identityDate).format('YYYY-MM-DD');
        }
        values.sallerId = values.sellerId;
        if (values.id && !values.leadConvert) {
            const response = await apiUserUpdate(values);
            if (response.succeeded) {
                message.success('Cập nhật thành công!');
                setOpen(false);
                actionRef.current?.reload();
                setUser?.(null);
                formRef.current?.resetFields();
            }
            return;
        }
    };

    useEffect(() => {
        if (user && open) {
            formRef.current?.setFields([
                {
                    name: 'id',
                    value: user.id
                },
                {
                    name: 'userName',
                    value: user.userName
                },
                {
                    name: 'name',
                    value: user.name
                },
                {
                    name: 'email',
                    value: user.email
                },
                {
                    name: 'phoneNumber',
                    value: user.phoneNumber
                },
                {
                    name: 'loyalty',
                    value: user.loyalty
                },
                {
                    name: 'gender',
                    value: user.gender
                },
                {
                    name: 'dateOfBirth',
                    value: user.dateOfBirth
                },
                {
                    name: 'avatar',
                    value: user.avatar
                },
                {
                    name: 'address',
                    value: user.address
                },
                {
                    name: 'tier',
                    value: user.tier
                },
                {
                    name: 'cardId',
                    value: user.cardId
                },
                {
                    name: 'identityNumber',
                    value: user.identityNumber
                },
                {
                    name: 'identityDate',
                    value: user.identityDate
                },
                {
                    name: 'identityAddress',
                    value: user.identityAddress
                },
                {
                    name: 'concerns',
                    value: user.concerns
                },
                {
                    name: 'personality',
                    value: user.personality
                },
                {
                    name: 'healthHistory',
                    value: user.healthHistory
                },
                {
                    name: 'familyCharacteristics',
                    value: user.familyCharacteristics
                },
                {
                    name: 'contractCode',
                    value: user.contractCode
                },
                {
                    name: 'contractDate',
                    value: user.contractDate
                },
                {
                    name: 'maxLoyalty',
                    value: user.maxLoyalty
                },
                {
                    name: 'branch',
                    value: user.branch
                },
                {
                    name: 'sellerId',
                    value: user.sellerId
                }
            ]);
        }
    }, [user, open]);

    return (
        <>
            <DrawerForm
                formRef={formRef}
                open={open}
                onOpenChange={setOpen}
                title="Chủ thẻ"
                onFinish={onFinish}
                width={1000}
            >
                <ProFormText name="id" hidden />
                <Row gutter={16}>
                    <Col md={8}>
                        <ProFormSelect name="sellerId" label="Trợ lý cá nhân" request={apiSalesWithSmOptions} showSearch rules={[
                            {
                                required: true
                            }
                        ]} disabled={leadConvert} />
                    </Col>
                    <Col md={8}>
                        <ProFormText name="name" label="Họ và tên" rules={[
                            {
                                required: true
                            }
                        ]} />
                    </Col>
                    <Col md={8}>
                        <ProFormText
                            name="email"
                            label="Email"
                            rules={[
                                {
                                    required: true,
                                },
                                {
                                    type: 'email'
                                }
                            ]}
                        />
                    </Col>
                    <Col md={6}>
                        <ProFormText name="phoneNumber" label="Số điện thoại" fieldProps={{
                            autoComplete: 'off'
                        }} />
                    </Col>
                    <Col md={6}>
                        <ProFormText name="contractCode" label="Số hợp đồng" tooltip="Số hợp đồng là tên đăng nhập" rules={[
                            {
                                required: true
                            }
                        ]} disabled={leadConvert} />
                    </Col>
                    <Col md={4}>
                        <ProFormDatePicker name="contractDate" width="lg" label="Ngày hợp đồng" rules={[
                            {
                                required: true
                            }
                        ]} fieldProps={{
                            format: {
                                type: 'mask',
                                format: 'DD-MM-YYYY'
                            }
                        }} />
                    </Col>
                    <Col md={4}>
                        <ProFormSelect label="Loại thẻ" name="cardId" request={apiCardOptions} showSearch rules={[
                            {
                                required: true
                            }
                        ]} />
                    </Col>
                    <Col md={4}>
                        <ProFormDigit label="Tổng điểm NP" name="maxLoyalty" rules={[
                            {
                                required: true
                            }
                        ]}
                            fieldProps={{
                                formatter: (value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ','),
                                parser: (value) => value?.replace(/(,*)/g, '') as unknown as number
                            }} />
                    </Col>
                    <Col md={3}>
                        <ProFormSelect label="Giới tính" name="gender" options={GENDER_OPTIONS} />
                    </Col>
                    <Col md={4}>
                        <ProFormDatePicker name="dateOfBirth" label="Ngày sinh" width="lg" fieldProps={{
                            format: {
                                type: 'mask',
                                format: 'DD-MM-YYYY'
                            }
                        }} />
                    </Col>
                    <Col md={4} xs={24}>
                        <ProFormSelect name="branchId" label="Chi nhánh" request={apiBranchOptions} rules={[
                            {
                                required: true
                            }
                        ]} />
                    </Col>
                    <Col md={13}>
                        <ProFormText name="address" label="Địa chỉ" />
                    </Col>
                    <Col md={8}>
                        <ProFormText name="avatar" label="Ảnh đại diện" fieldProps={{
                            suffix: <Button onClick={() => setOpenUpload(true)} size='small'>Tải lên</Button>
                        }} className='w-full' />
                    </Col>
                    <Col md={4}>
                        <ProFormText name="identityNumber" label="CMT/CCCD/Hộ chiếu" />
                    </Col>
                    <Col md={4}>
                        <ProFormDatePicker name="identityDate" label="Ngày cấp" width="lg" fieldProps={{
                            format: {
                                type: 'mask',
                                format: 'DD-MM-YYYY'
                            }
                        }} />
                    </Col>
                    <Col md={8}>
                        <ProFormText name="identityAddress" label="Nơi cấp" />
                    </Col>
                    <Col md={12}>
                        <ProFormTextArea name="concerns" label="Mối quan tâm" />
                    </Col>
                    <Col md={12}>
                        <ProFormTextArea name="personality" label="Đặc điểm tính cách" />
                    </Col>
                    <Col md={12}>
                        <ProFormTextArea name="healthHistory" label="Tiền sử sức khỏe" />
                    </Col>
                    <Col md={12}>
                        <ProFormTextArea name="familyCharacteristics" label="Đặc điểm gia đình" />
                    </Col>
                </Row>

            </DrawerForm>
            <WfUpload open={openUpload} onCancel={() => setOpenUpload(false)} onFinish={(value: string) => {
                formRef.current?.setFieldValue('avatar', value)
            }} />
        </>
    )
}

export default CardHolderForm