import { apiSourceCreate, apiSourceDetail, apiSourceUpdate, apiTypeOfDataOptions, apiTypeOfDataSources } from "@/services/settings/source";
import { apiTeamOptions } from "@/services/users/team";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormSelect, ProFormText } from "@ant-design/pro-components";
import { Col, message, Row } from "antd";
import { useEffect, useRef, useState } from "react";

type Props = ModalFormProps & {
    data?: any;
    reload?: () => void;
}

const SourceForm: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);
    const [sourceType, setSourceType] = useState<number>();

    const [typeOfDataOptions, setTypeOfDataOptions] = useState([]);

    useEffect(() => {
        const fetchTypeOfDataOptions = async () => {
            const res = await apiTypeOfDataOptions({ sourceType });
            setTypeOfDataOptions(res);
        };
        fetchTypeOfDataOptions();
    }, [sourceType]);

    useEffect(() => {
        if (props.data) {
            apiSourceDetail(props.data.id).then(res => {
                const data = res.data;
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
                        name: 'sourceType',
                        value: data.sourceType
                    },
                    {
                        name: 'typeOfDataId',
                        value: data.typeOfDataId
                    },
                    {
                        name: 'overwrite',
                        value: data.overwrite
                    },
                    {
                        name: 'protected',
                        value: data.protected
                    }
                ])
            });
        }
    }, [props.data, props.open]);

    const onFinish = async (values: any) => {
        if (props.data) {
            await apiSourceUpdate(values);
        } else {
            await apiSourceCreate(values);
        }
        message.success('Lưu thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Source Form" formRef={formRef} onFinish={onFinish}
        modalProps={{
            destroyOnHidden: true
        }}
        >
            <ProFormText name="id" hidden />
            <Row gutter={16}>
                <Col xs={24} md={12}>
                    <ProFormSelect name="sourceType" label="Source" request={apiTypeOfDataSources} rules={[
                        {
                            required: true
                        }
                    ]} allowClear={false}
                        onChange={(value: number) => setSourceType(value)}
                    />
                </Col>
                <Col xs={24} md={12}>
                    <ProFormSelect name="typeOfDataId" label="Type of Data" options={typeOfDataOptions} rules={[
                        {
                            required: true
                        }
                    ]} />
                </Col>
                <Col xs={24} md={12}>
                    <ProFormSelect name="overwrite" label="Overwrite" options={[
                        { label: 'Có', value: true },
                        { label: 'Không', value: false }
                    ]} />
                </Col>
                <Col xs={24} md={12}>
                    <ProFormSelect name="protected" label="Protected" options={[
                        { label: 'Có', value: true },
                        { label: 'Không', value: false }
                    ]} />
                </Col>
            </Row>
            <ProFormSelect name="teamId" label="Group" request={apiTeamOptions} showSearch />
            <ProFormText name="name" label="Tên nguồn" rules={[{ required: true, message: 'Vui lòng nhập tên nguồn' }]} />
        </ModalForm>
    )
}

export default SourceForm;