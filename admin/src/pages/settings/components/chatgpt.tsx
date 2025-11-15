import { ProFormTextArea } from "@ant-design/pro-components";

type Props = {
    data: any;
}

const ChatGPTSetting: React.FC<Props> = ({ data }) => {
    return (
        <>
            <ProFormTextArea
                label="ChatGPT API Key"
                name="apiKey"
                initialValue={data?.apiKey}
                rules={[{
                    required: true,
                }]}
            />
        </>
    )
}

export default ChatGPTSetting;