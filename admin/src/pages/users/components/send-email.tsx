import MyCKEditor from "@/components/ckeditor";
import { apiCardHolderOptions, apiSendEmails } from "@/services/user";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormSelect, ProFormText } from "@ant-design/pro-components"
import { message } from "antd";
import { useRef } from "react";

const SendEmailComponent: React.FC<ModalFormProps> = (props) => {

    const formRef = useRef<ProFormInstance>();

    return (
        <ModalForm {...props} title="Gửi Email hàng loạt"
            formRef={formRef}
            onFinish={async (values) => {
                await apiSendEmails(values);
                message.success('Gửi thành công!');
                formRef.current?.resetFields();
            }}
        >
            <ProFormSelect name="userIds" label="Gửi tới"
                request={apiCardHolderOptions}
                showSearch
                mode="multiple"
                rules={[
                    {
                        required: true
                    }
                ]}
            />
            <ProFormText name="subject" label="Tiêu đề"
                rules={[
                    {
                        required: true
                    }
                ]} />
            <MyCKEditor name="body" label="Nội dung" loading={false}
                rules={[
                    {
                        required: true
                    }
                ]} />
        </ModalForm>
    )
}

export default SendEmailComponent;