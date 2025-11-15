import { ProForm, ProFormText } from "@ant-design/pro-components"

const Basic: React.FC = () => {
    return (
        <ProForm>
            <ProFormText label="Name" name="name" rules={[
                {
                    required: true
                }
            ]} />
        </ProForm>
    )
}
export default Basic