import { graphFacebook } from "@/services/setting"
import { ProCard, ProForm, ProFormText } from "@ant-design/pro-components"
import { Alert, Divider, Typography, message } from "antd"
import { useState } from "react"

const GraphApiExplorer: React.FC = () => {

    const [data, setData] = useState<string>('Unknow');

    const onFinish = async (values: any) => {
        const response = await graphFacebook(values.query);
        setData(JSON.stringify(response));
    }

    return (
        <ProCard title="Graph API Explorer" headerBordered>
            <Typography.Title level={5}>Results</Typography.Title>
            <Alert type="info" banner message={data}></Alert>
            <Divider />
            <ProForm onFinish={onFinish}>
                <ProFormText label="Try to get" name="query" initialValue="me?fields=id,name" />
            </ProForm>
        </ProCard>
    )
}

export default GraphApiExplorer