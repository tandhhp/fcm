import { FormTag } from "@/components/form";
import ItemPerRowForm from "@/components/form/item-per-row";
import WorkSummary from "@/components/works/summary";
import { getArguments, saveArguments } from "@/services/work-content";
import { ProFormInstance, PageContainer, ProCard, ProForm, ProFormText, ProFormDigit } from "@ant-design/pro-components";
import { useParams } from "@umijs/max";
import { message, Row, Col } from "antd";
import { useRef, useEffect } from "react";

const ProductLister : React.FC = () => {
    const formRef = useRef<ProFormInstance>();
    const { id } = useParams();

    useEffect(() => {
        if (id) {
            getArguments(id).then((response: CPN.ProductLister) => {
                formRef.current?.setFields([
                    {
                        name: 'title',
                        value: response.title,
                    },
                    {
                        name: 'itemPerRow',
                        value: response.itemPerRow
                    },
                    {
                        name: 'pageSize',
                        value: response.pageSize
                    },
                ]);
            });
        }
    }, [id]);

    const onFinish = async (values: CPN.ProductLister) => {
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
                            <ItemPerRowForm />
                            <ProFormDigit label="Page size" name="pageSize" />
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

export default ProductLister