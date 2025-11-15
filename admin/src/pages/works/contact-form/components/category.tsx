import { saveArguments } from "@/services/work-content";
import { ModalForm, ProFormText, ProList } from "@ant-design/pro-components"
import { Button, message } from "antd";
import { useState } from "react";
import { PlusOutlined } from "@ant-design/icons";
import { FormattedMessage } from "@umijs/max";

const ContactFormCategory: React.FC<API.ContactForm> = (props) => {

    const [categories, setCategories] = useState<API.Option[]>(props.categories || []);
    const [open, setOpen] = useState<boolean>(false);

    const onFinish = async (values: API.Option) => {
        const body: API.ContactForm = { ...props };
        if (!body.categories) {
            body.categories = [];
        }
        values.label = values.value;
        body.categories.push(values);
        const response = await saveArguments(props.id, body);
        if (response.succeeded) {
            message.success('Saved!');
            setCategories(body.categories);
        }
    }

    return (
        <>
            <ProList
                toolBarRender={() => [
                    <Button type="primary" icon={<PlusOutlined />} key="add" onClick={() => setOpen(true)}>
                        <span><FormattedMessage id='general.new' /></span>
                    </Button>
                ]}
                dataSource={categories} />
            <ModalForm open={open} onOpenChange={setOpen} onFinish={onFinish} title={<FormattedMessage id='general.new' />}>
                <ProFormText name="value" label="Category" rules={[
                    {
                        required: true
                    }
                ]} />
            </ModalForm>
        </>
    )
}

export default ContactFormCategory