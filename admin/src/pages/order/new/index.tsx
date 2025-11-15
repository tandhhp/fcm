import { PageContainer, ProCard, ProForm, ProFormSelect } from "@ant-design/pro-components";
import { Divider } from "antd";

const NewOrderPage: React.FC = () => {

    return (
        <PageContainer>
            <ProCard>
            <ProForm>
                <Divider>Details</Divider>
                <ProFormSelect label="Product" />
            </ProForm>
            </ProCard>
        </PageContainer>
    )
}

export default NewOrderPage;