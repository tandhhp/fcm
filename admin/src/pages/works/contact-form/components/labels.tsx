import { saveArguments } from "@/services/work-content";
import { ProForm, ProFormInstance, ProFormText } from "@ant-design/pro-components"
import { Space, message } from "antd";
import { useRef, useEffect } from "react";

const ContactFormLabels: React.FC<API.ContactForm> = (props) => {

    const { labels } = props;

    const formRef = useRef<ProFormInstance>();

    useEffect(() => {
        if (labels) {
            formRef.current?.setFields([
                {
                    name: 'name',
                    value: labels?.name,
                },
                {
                    name: 'email',
                    value: labels?.email,
                },
                {
                    name: 'phoneNumber',
                    value: labels?.phoneNumber,
                },
                {
                    name: 'address',
                    value: labels?.address,
                },
                {
                    name: 'note',
                    value: labels?.note,
                },
                {
                    name: 'submit',
                    value: labels?.submit,
                }
            ]);
        }
    }, [JSON.stringify(labels)]);

    const onFinish = async (values: API.ContactFormLabels) => {
        const body: API.ContactForm = { ...props };
        body.labels = values;
        const response = await saveArguments(props.id, body);
        if (response.succeeded) {
            message.success('Saved!');
        }
    }

    return (
        <ProForm onFinish={onFinish} formRef={formRef}>
            <Space>
                <ProFormText name="name" label="Name" />
                <ProFormText name="email" label="Email" />
                <ProFormText name="phoneNumber" label="Phone number" />
                <ProFormText name="address" label="Address" />
                <ProFormText name="note" label="Note" />
            </Space>
            <Space>
                <ProFormText name="category" label="Category" />
                <ProFormText name="appointment" label="Appointment" />
            </Space>
        </ProForm>
    )
}

export default ContactFormLabels