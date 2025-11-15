import ItemPerRowForm from "@/components/form/item-per-row";
import WorkSummary from "@/components/works/summary";
import { getArguments, saveArguments } from "@/services/work-content";
import { PageContainer, ProCard, ProForm, ProFormDigit, ProFormInstance, ProFormText } from "@ant-design/pro-components"
import { useParams } from "@umijs/max";
import { message, Row, Col } from "antd";
import { useRef, useEffect } from "react";

const ProductSpotlight: React.FC = () => {
    const { id } = useParams();
    const formRef = useRef<ProFormInstance>();

    useEffect(() => {
        getArguments(id).then((response) => {
            formRef.current?.setFields([
                {
                    name: 'title',
                    value: response.title,
                },
                {
                    name: 'itemPerRow',
                    value: response.itemPerRow,
                },
                {
                    name: 'pageSize',
                    value: response.pageSize,
                },
            ]);
        });
    }, [id]);

    const onFinish = async (values: CPN.ProductSpotlight) => {
        values.pageSize = Number(values.pageSize);
        const response = await saveArguments(id, values);
        if (response.succeeded) {
            message.success('Saved!');
        }
    };

    return (
        <PageContainer>
            <Row gutter={16}>
                <Col span={16}>
                    <ProCard>
                        <ProForm onFinish={onFinish} formRef={formRef}>
                            <ProFormText name="title" label="Title" />
                            <ProFormDigit name="pageSize" label="Page size" />
                            <ItemPerRowForm />
                        </ProForm>
                    </ProCard>
                </Col>
                <Col span={8}>
                    <WorkSummary />
                </Col>
            </Row>
        </PageContainer>
    );
}

export default ProductSpotlight