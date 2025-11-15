import WfUpload from "@/components/file-explorer/upload";
import { apiGetCombo, apiHealthcareCreate, apiSaveComboHealthcare } from "@/services/healthcare";
import { UploadOutlined } from "@ant-design/icons";
import { ModalForm, ProFormDigit, ProFormInstance, ProFormText, ProFormTextArea } from "@ant-design/pro-components"
import { useParams } from "@umijs/max";
import { Button, message } from "antd"
import { useEffect, useRef, useState } from "react";

type HealthcareComboProps = {
    open: boolean;
    setOpen: any;
    id: string;
    reload: any;
}

const HealthcareCombo: React.FC<HealthcareComboProps> = ({ open, setOpen, reload, id: hId }) => {

    const formRef = useRef<ProFormInstance>();
    const { id } = useParams();
    const [upload, setUpload] = useState<boolean>(false);

    useEffect(() => {
        if (hId) {
            apiGetCombo(hId).then((response: any) => {
                if (response) {
                    formRef.current?.setFields([
                        {
                            name: 'point',
                            value: response.point
                        },
                        {
                            name: 'name',
                            value: response.name
                        },
                        {
                            name: 'description',
                            value: response.description
                        },
                        {
                            name: 'content',
                            value: response.content
                        },
                        {
                            name: 'catalogId',
                            value: response.catalogId
                        },
                        {
                            name: 'id',
                            value: response.id
                        },
                        {
                            name: 'thumbnail',
                            value: response.thumbnail
                        }
                    ])
                }
            });
        }
    }, [hId]);

    const onFinish = async (values: any) => {
        if (values.id) {
            await apiSaveComboHealthcare(values);
            message.success('Lưu thành công!');
            setOpen(false);
            reload();
            formRef.current?.resetFields();
            return;
        }
        values.parentId = id;
        const response = await apiHealthcareCreate(values);
        if (response.succeeded) {
            message.success('Lưu thành công!');
            setOpen(false);
            reload();
            formRef.current?.resetFields();
        }
    }

    return (
        <>
            <ModalForm
                open={open}
                onOpenChange={setOpen}
                onFinish={onFinish}
                formRef={formRef}
            >
                <ProFormText hidden name="catalogId" />
                <ProFormText hidden name="id" />
                <ProFormText name="name" label="Tên gói khám" rules={[
                    {
                        required: true
                    }
                ]} />
                <ProFormDigit name="point" label="Điểm" rules={[
                    {
                        required: true
                    }
                ]} />
                <ProFormTextArea name="description" label="Mô tả" />
                <ProFormText name="thumbnail" label="Ảnh đại diện" fieldProps={{
                    addonAfter: <Button icon={<UploadOutlined />} type="text" size="small" onClick={() => setUpload(true)}>Upload</Button>
                }} />
            </ModalForm>

            <WfUpload open={upload} onCancel={setUpload} onFinish={(values: any) => {
                formRef.current?.setFieldValue('thumbnail', values.url);
            }} />
        </>
    )
}

export default HealthcareCombo