import WorkSummary from "@/components/works/summary";
import { getArguments, saveArguments } from "@/services/work-content";
import { PageContainer, ProCard, ProForm, ProFormInstance, ProFormText } from "@ant-design/pro-components"
import { useParams } from "@umijs/max";
import { Col, Row, message } from "antd"
import { useRef, useEffect } from "react";

const ShopeeProduct: React.FC = () => {
    const formRef = useRef<ProFormInstance>();
    const { id } = useParams();

    useEffect(() => {
        if (id) {
            getArguments(id).then((response: CPN.ShopeeProduct) => {
                formRef.current?.setFields([
                    {
                        name: 'urlSuffix',
                        value: response.urlSuffix,
                    },
                    {
                        name: 'groupId',
                        value: response.groupId
                    },
                    {
                        name: 'title',
                        value: response.title
                    },
                ]);
            });
        }
    }, [id]);

    const onFinish = async (values: CPN.ShopeeProduct) => {
        const response = await saveArguments(id, values);
        if (response.succeeded) {
            message.success('Saved');
        }
    };

    return (
        <PageContainer>
            <Row gutter={16}>
                <Col md={16}>
                    <ProCard>
                        <ProForm onFinish={onFinish} formRef={formRef}>
                            <ProFormText label="Title" name="title" />
                            <ProFormText label="Group Id" name="groupId" />
                            <ProFormText label="Url Suffix" name="urlSuffix" />
                        </ProForm>
                    </ProCard>
                </Col>
                <Col md={8}>
                    <WorkSummary />
                </Col>
            </Row>
        </PageContainer>
    );
}

export default ShopeeProduct