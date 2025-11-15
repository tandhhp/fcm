import { apiBranchOptions } from "@/services/settings/branch";
import { apiUserImport } from "@/services/user";
import { DownloadOutlined, UserAddOutlined } from "@ant-design/icons";
import { ModalForm, ProFormInstance, ProFormSelect, ProFormUploadDragger } from "@ant-design/pro-components"
import { Button, message } from "antd";
import { useRef, useState } from "react";

type Props = {
    reload?: () => void
}

const UserImport: React.FC<Props> = ({ reload }) => {

    const formRef = useRef<ProFormInstance>(null);
    const [open, setOpen] = useState<boolean>(false);

    const onFinish = async (values: any) => {
        const formData = new FormData();
        formData.append('file', values.file[0].originFileObj);
        formData.append('branchId', values.branchId);
        await apiUserImport(formData);
        message.success('Import người dùng thành công');
        formRef.current?.resetFields();
        setOpen(false);
        reload?.();
        return true;
    }

    return (
        <>
            <Button type="primary" icon={<UserAddOutlined />} onClick={() => setOpen(true)}>Import người dùng</Button>
            <ModalForm
                formRef={formRef}
                onFinish={onFinish}
                title="Import người dùng" open={open} onOpenChange={setOpen}>
                <div className="mb-4 flex justify-end">
                    <DownloadOutlined className="mr-2" />
                    <a href="https://docs.google.com/spreadsheets/d/1g8WHBFZXmbd_73JuqV41SoRUesg0kuxMA7FRkkVakS0/edit?usp=sharing" target="_blank" rel="noreferrer" className="text-blue-600">Tải file mẫu</a>
                </div>
                        <ProFormSelect name={"branchId"} label="Chi nhánh" request={apiBranchOptions} showSearch rules={[{ required: true, message: 'Vui lòng chọn chi nhánh' }]} initialValue={1} allowClear={false} />
                <ProFormUploadDragger
                    title="Tải file lên"
                    description="Kéo thả file vào khu vực này"
                    name="file" label="Tải lên file" max={1} accept=".xls,.xlsx"
                    fieldProps={{
                        beforeUpload: () => false
                    }}
                    rules={[{ required: true, message: 'Vui lòng chọn file' }]}>
                </ProFormUploadDragger>
            </ModalForm>
        </>
    )
}

export default UserImport;