import { ModalForm, ProFormSelect, ProFormText } from "@ant-design/pro-components"

type ModalLinkProps = {
    open: boolean;
    onOpenChange?: any;
    onFinish?: any;
    data?: CPN.Link;
};

const ModalLink: React.FC<ModalLinkProps> = (props) => {

    const onFinish = async (values: CPN.Link) => {
        props.onFinish(values);
    };

    return (
        <ModalForm
            onFinish={onFinish}
            open={props.open}
            onOpenChange={props.onOpenChange}
        >
            <ProFormText name="id" hidden />
            <ProFormText name="name" label="Name" />
            <ProFormText
                name="href"
                label="URL"
                rules={[
                    {
                        required: true,
                    },
                ]}
            />
            <ProFormSelect
                name="target"
                label="Target"
                allowClear
                options={[
                    {
                        value: '_blank',
                        label: 'Open in new tab',
                    },
                ]}
            />
        </ModalForm>
    );
}

export default ModalLink;