import WfUpload from "@/components/file-explorer/upload";
import { queryProductImage, saveProductImage } from "@/services/catalog";
import { DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { ProCard } from "@ant-design/pro-components"
import { useParams } from "@umijs/max";
import { Button, message, Image, Form } from "antd";
import { useEffect, useState } from "react";

const ProductImage: React.FC = () => {

    const { id } = useParams();

    const [previewOpen, setPreviewOpen] = useState(false);
    const [fileList, setFileList] = useState<string[]>([]);

    useEffect(() => {
        queryProductImage(id).then(response => {
            if (response) {
                setFileList(response.images);
            }
        })
    }, []);

    const handleCancel = () => setPreviewOpen(false);

    const onFinish = async (url: string) => {
        let newList = fileList;
        newList.push(url);
        const response = await saveProductImage(newList, id);
        if (response.succeeded) {
            setFileList(newList);
            message.success('Saved!');
        } else {
            message.error(response.errors[0].description);
        }
    }

    const onRemoveImage = async (url: string) => {
        const newList = fileList.filter(x => x !== url);
        const response = await saveProductImage(newList, id);
        if (response.succeeded) {
            setFileList(newList);
            message.success('Saved!');
        } else {
            message.error(response.errors[0].description);
        }
    }

    return (
        <>
            <Form.Item label="Hình ảnh" name="images">
                <div className="flex gap-4 flex-wrap">
                    {
                        fileList.map((url, index) => (
                            <div className="btn-upload p-2" key={index}>
                                <Image src={url} alt="IMG" />
                                <Button icon={<DeleteOutlined />} size="small" danger onClick={() => onRemoveImage(url)} style={{
                                    position: 'absolute',
                                    right: 6
                                }} />
                            </div>
                        ))
                    }

                    <button type="button" className="btn-upload" onClick={() => setPreviewOpen(true)}>
                        <PlusOutlined />
                        <div style={{ marginTop: 8 }}>Upload</div>
                    </button>
                </div>
                <WfUpload open={previewOpen} onCancel={handleCancel} onFinish={onFinish} />
            </Form.Item>
        </>
    )
}

export default ProductImage