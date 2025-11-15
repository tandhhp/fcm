import WfUpload from "@/components/file-explorer/upload";
import { apiAchievementAdd, apiAchievementListById } from "@/services/user";
import { GifOutlined, UploadOutlined } from "@ant-design/icons";
import { ModalForm, ProFormInstance, ProFormText } from "@ant-design/pro-components";
import { useParams } from "@umijs/max";
import { Button, Empty, message, Space } from "antd";
import { useEffect, useRef, useState } from "react";

const Achievement: React.FC = () => {
    const [data, setData] = useState<any[]>([]);
    const { id } = useParams();
    const [open, setOpen] = useState<boolean>(false);
    const [upload, setUpload] = useState<boolean>(false);
    const formRef = useRef<ProFormInstance>();

    const reload = () => {
        if (id) {
            apiAchievementListById(id).then(response => {
                setData(response);
            })
        }
    }

    useEffect(() => {
        reload()
    }, [id]);

    if (data.length === 0) {
        return <Empty />
    }

    return (
        <>
            <div className="flex justify-end mb-4">
                <Button type="primary" icon={<GifOutlined />} onClick={() => setOpen(true)}>Tặng thành tựu</Button>
            </div>
            <div className="grid md:grid-cols-4 2xl:grid-cols-6 grid-cols-3">
                {
                    data.map((value: any) => (
                        <div key={value.id}>
                            <div className="flex flex-col items-center justify-center rounded bg-white p-3 text-center">
                                <img src={value.icon} alt="icon" className="mb-3 w-20 md:w-32 h-20 md:h-32 object-cover" />
                                <div className="text-sm text-primary font-medium">{value.name}</div>
                            </div>
                        </div>
                    ))
                }
            </div>
            <ModalForm open={open} onOpenChange={setOpen} title="Tặng thành tựu" formRef={formRef} onFinish={async (values) => {
                values.userId = id;
                await apiAchievementAdd(values);
                message.success('Thêm thành công!');
                setOpen(false);
                reload();

            }}>
                <ProFormText name="name" label="Tên thành tựu" rules={[
                    {
                        required: true
                    }
                ]} />
                <ProFormText name="icon" label="Ảnh thành tựu" rules={[
                    {
                        required: true
                    }
                ]} fieldProps={{
                    suffix: (
                        <Space>
                    <Button icon={<UploadOutlined />} onClick={() => setUpload(true)}>Upload</Button>
                </Space>
                    )
                }} />
            </ModalForm>
            <WfUpload open={upload} onCancel={setUpload} onFinish={(values: any) => {
                formRef.current?.setFieldValue('icon', values.url);
            }} />
        </>
    )
}

export default Achievement;