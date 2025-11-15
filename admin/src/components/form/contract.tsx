import { apiContractCreate, apiContractDetail, apiContractLeadOptions, apiContractUpdate } from "@/services/finances/contract";
import { apiEventToOptions } from "@/services/event";
import { apiKeyInOptions, apiManagerOptions, apiSalesManagerOptions } from "@/services/role";
import { apiCardOptions } from "@/services/settings/card";
import { apiSalesOptions } from "@/services/user";
import { ModalForm, ModalFormProps, ProFormDatePicker, ProFormDigit, ProFormInstance, ProFormSelect, ProFormText } from "@ant-design/pro-components";
import { Col, message, Row } from "antd";
import { useEffect, useRef, useState } from "react";
import { apiSourceOptions } from "@/services/settings/source";

type Props = ModalFormProps & {
    reload?: () => void;
    data?: any;
}

const ContractForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>();
    const [salesManagerId, setSalesManagerId] = useState<string | null>(null);
    const [salesOptions, setSalesOptions] = useState<any[]>([]);
    const [keyInOptions, setKeyInOptions] = useState<any[]>([]);
    const [teamKeyInId, setTeamKeyInId] = useState<string | null>(null);

    useEffect(() => {
        if (props.open && props.data) {
            apiContractDetail(props.data.id).then((res) => {
                const data = res.data;
                formRef.current?.setFields([
                    {
                        name: 'id',
                        value: data.id
                    },
                    {
                        name: 'leadId',
                        value: data.leadId
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
                        name: 'sourceId',
                        value: data.sourceId
                    },
                    {
                        name: 'code',
                        value: data.code
                    },
                    {
                        name: 'cardId',
                        value: data.cardId
                    },
                    {
                        name: 'amount',
                        value: data.amount
                    },
                    {
                        name: 'teamKeyInId',
                        value: data.teamKeyInId
                    },
                    {
                        name: 'keyInId',
                        value: data.keyInId
                    },
                    {
                        name: 'createdDate',
                        value: data.createdDate
                    }
                ]);
                setSalesManagerId(data.salesManagerId);
                setTeamKeyInId(data.teamKeyInId);
            });
        }
    }, [props.open, props.data]);

    const onFinish = async (values: any) => {
        if (props.data) {
            await apiContractUpdate(values);
        } else {
            await apiContractCreate(values);
        }
        message.success('Chốt deal thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    useEffect(() => {
        if (salesManagerId) {
            apiSalesOptions({ salesManagerId }).then((res) => {
                setSalesOptions(res);
            });
        }
    }, [salesManagerId]);

    useEffect(() => {
        if (teamKeyInId) {
            apiKeyInOptions({ teamKeyInId }).then((res) => {
                setKeyInOptions(res);
            });
        }
    }, [teamKeyInId]);

    return (
        <>
            <ModalForm {...props} title="Cài đặt hợp đồng" formRef={formRef} onFinish={onFinish}>
                <ProFormText name="id" hidden />
                <Row gutter={16}>
                    <Col md={18} xs={24}>
                        <ProFormSelect name={"leadId"} label="SDT/CCCD" rules={[{ required: true }]}
                            showSearch
                            request={apiContractLeadOptions}
                        />
                    </Col>
                    <Col md={6} xs={24}>
                        <ProFormDatePicker name="createdDate" label="Ngày chốt" width="md" rules={[{ required: true }]} />
                    </Col>
                    <Col md={12} xs={24}>
                        <ProFormSelect name="teamKeyInId" label="Team Key-In" request={apiManagerOptions} showSearch
                            onChange={(value: string) => {
                                setTeamKeyInId(value);
                                formRef.current?.setFieldsValue({ keyInId: undefined });
                                setKeyInOptions([]);
                            }}
                        />
                    </Col>
                    <Col md={12} xs={24}>
                        <ProFormSelect name="keyInId" label="Key-In" options={keyInOptions} showSearch />
                    </Col>
                    <Col md={8} xs={24}>
                        <ProFormSelect name={"salesManagerId"} label="Sales Manager" rules={[{ required: true }]}
                            showSearch
                            onChange={(value: string) => setSalesManagerId(value)}
                            request={apiSalesManagerOptions} />
                    </Col>
                    <Col md={8} xs={24}>
                        <ProFormSelect name={"salesId"} label="Sales" rules={[{ required: true }]} showSearch options={salesOptions} />
                    </Col>
                    <Col md={8} xs={24}>
                        <ProFormSelect name={"toById"} label="T.O" rules={[{ required: true }]} request={apiEventToOptions} showSearch />
                    </Col>
                    <Col md={6} xs={24}>
                        <ProFormSelect name={"sourceId"} label="Nguồn" rules={[{ required: true }]} request={apiSourceOptions} />
                    </Col>
                    <Col md={6} xs={24}>
                        <ProFormText name="code" label="Số hợp đồng" rules={[
                            { required: true, message: 'Vui lòng nhập số hợp đồng' }
                        ]} />
                    </Col>
                    <Col md={6} xs={24}>
                        <ProFormSelect name="cardId" label="Thẻ" request={apiCardOptions} />
                    </Col>
                    <Col md={6} xs={24}>
                        <ProFormDigit name="amount" label="GTHĐ" fieldProps={{
                            formatter: (value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ','),
                            parser: (value) => value?.replace(/(,*)/g, '') as unknown as number
                        }}
                            rules={[
                                { required: true, message: 'Vui lòng nhập GTHĐ' }
                            ]}
                        />
                    </Col>
                </Row>
            </ModalForm>
        </>
    );
}

export default ContractForm;