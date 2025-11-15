import { apiContactImport } from "@/services/contact";
import { apiSourceOptions } from "@/services/settings/source";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormSelect, ProFormUploadDragger } from "@ant-design/pro-components";
import { message } from "antd";
import { useRef } from "react";

type Props = ModalFormProps & {
    reload?: () => void;
}

const ContactImport: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);

    const onFinish = async (values: any) => {
        const formData = new FormData();
        formData.append('file', values.file[0].originFileObj);
        formData.append('sourceId', values.sourceId);
        await apiContactImport(formData);
        message.success('Nhập dữ liệu thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Nhập dữ liệu danh bạ" formRef={formRef} onFinish={onFinish}>
            <div className="mb-2 flex justify-end">
                <a href="https://docs.google.com/spreadsheets/d/1goezVrEivPWb7czSx_-BuDC4qa166X3eGuShz_l9b9U/edit?gid=0#gid=0" target="_blank" rel="noreferrer" className="text-blue-600 underline">Tải mẫu file Excel tại đây</a>
            </div>
            <ProFormSelect name={"sourceId"} label="Nguồn danh bạ" rules={[{ required: true }]} request={apiSourceOptions} showSearch />
            <ProFormUploadDragger name={"file"} label="File Excel" rules={[{ required: true }]} max={1} fieldProps={{
                accept: ".xlsx",
                beforeUpload: () => false
            }}
                title={"Kéo thả file vào đây hoặc click để chọn file"}
                description={"Chỉ hỗ trợ file định dạng .xlsx"}
            />
        </ModalForm>
    )
}

export default ContactImport;