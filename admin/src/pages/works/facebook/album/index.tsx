import { getArguments, saveArguments } from "@/services/work-content";
import { PageContainer, ProCard, ProForm, ProFormInstance, ProFormText } from "@ant-design/pro-components"
import { useParams } from "@umijs/max";
import { message } from "antd";
import { useEffect, useRef } from "react";

const FacebookAlbumPage: React.FC = () => {

    const { id } = useParams();
    const formRef = useRef<ProFormInstance>()

    useEffect(() => {
        getArguments(id).then(response => {
            formRef.current?.setFields([
                {
                    name: 'albumId',
                    value: response.albumId
                }
            ])
        })
    }, []);

    const onFinish = async (values: any) => {
        const response = await saveArguments(id, values);
        if (response.succeeded) {
            message.success('Saved!');
        }
    }

    return (
        <PageContainer>
            <ProCard>
                <ProForm onFinish={onFinish} formRef={formRef}>
                    <ProFormText name="albumId" label="Id" />
                </ProForm>
            </ProCard>
        </PageContainer>
    )
}

export default FacebookAlbumPage