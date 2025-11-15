import { FormTag } from "@/components/form";
import WorkSummary from "@/components/works/summary";
import { getArguments, saveArguments } from "@/services/work-content";
import { PageContainer, ProCard, ProForm, ProFormInstance, ProFormText } from "@ant-design/pro-components"
import { useParams } from "@umijs/max";
import { message, Row, Col } from "antd";
import { useRef, useEffect } from "react";

const ProductPicker: React.FC = () => {
    const formRef = useRef<ProFormInstance>();
    const { id } = useParams();

    useEffect(() => {
        if (id) {
            getArguments(id).then((response: CPN.ProductPicker) => {
                const tagId = response.tagIds && response.tagIds.length > 0 ? response.tagIds[0] : null;
                formRef.current?.setFields([
                    {
                        name: 'title',
                        value: response.title,
                    },
                    {
                        name: 'tagId',
                        value: tagId
                    },
                ]);
            });
        }
    }, [id]);

    const onFinish = async (values: any) => {
        values.tagIds = [values.tagId]
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
                            <FormTag name="tagId" label="Tag" />
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

export default ProductPicker