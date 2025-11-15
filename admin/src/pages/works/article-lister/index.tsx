import { getArguments, saveArguments } from "@/services/work-content";
import { PageContainer, ProCard, ProForm, ProFormInstance, ProFormText } from "@ant-design/pro-components"
import { useParams } from "@umijs/max";
import { message } from "antd";
import { useRef, useEffect } from "react";

const ArticleLister: React.FC = () => {
    const { id } = useParams();
    const formRef = useRef<ProFormInstance>();

    useEffect(() => {
        getArguments(id).then((response) => {
            formRef.current?.setFields([
                {
                    name: 'name',
                    value: response.name,
                }
            ]);
        });
    }, [id]);

    const onFinish = async (values: any) => {
        const response = await saveArguments(id, values);
        if (response.succeeded) {
            message.success('Saved!');
        } else {
            message.error(response.errors[0].description);
        }
    };

    return (
        <PageContainer>
            <ProCard>
                <ProForm onFinish={onFinish} formRef={formRef}>
                    <ProFormText name="name" label="Name" />
                </ProForm>
            </ProCard>
        </PageContainer>
    )
}

export default ArticleLister