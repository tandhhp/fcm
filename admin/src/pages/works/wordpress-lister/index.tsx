import { getArguments, saveArguments } from "@/services/work-content"
import { PageContainer, ProCard, ProForm, ProFormInstance, ProFormText } from "@ant-design/pro-components"
import { useParams } from "@umijs/max"
import { message } from "antd";
import { useEffect, useRef } from "react";

const WordPressLister: React.FC = () => {

    const { id } = useParams();
    const formRef = useRef<ProFormInstance>();

    useEffect(() => {
        getArguments(id).then(response => {
            if (response) {
                formRef.current?.setFields([
                    {
                        name: 'domain',
                        value: response.domain
                    }
                ])
            }
        })
    }, []);

    const onFinish = async (values: any) => {
        if (values.domain.substr(values.domain.length - 1) === '/') {
            values.domain = values.domain.slice(0, -1);
        }
        const response = await saveArguments(id, values);
        if (response.succeeded) {
            message.success('Saved!');
        }
    }

    return (
        <PageContainer>
            <ProCard>
                <ProForm onFinish={onFinish} formRef={formRef}>
                    <ProFormText name="domain" label="Domain" rules={[
                        {
                            required: true
                        }
                    ]} />
                </ProForm>
            </ProCard>
        </PageContainer>
    )
}

export default WordPressLister