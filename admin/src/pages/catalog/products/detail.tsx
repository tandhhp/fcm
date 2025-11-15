import { ProCard, ProForm, ProFormDigit, ProFormInstance, ProFormText } from "@ant-design/pro-components"
import { useParams } from "@umijs/max"
import { queryProduct, saveProduct } from "@/services/catalog";
import { Button, Col, Row, Image, message } from "antd";
import { useEffect, useRef, useState } from "react";
import { DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import WfUpload from "@/components/file-explorer/upload";
import MyCKEditor from "@/components/ckeditor";

const ProductDetail: React.FC = () => {

    const { id } = useParams();
    const formRef = useRef<ProFormInstance>();
    const [fileList, setFileList] = useState<string[]>([]);
    const [previewOpen, setPreviewOpen] = useState(false);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        setLoading(true);
        queryProduct(id).then(response => {
            if (response) {
                formRef.current?.setFields([
                    {
                        name: 'point',
                        value: response.point
                    },
                    {
                        name: 'brand',
                        value: response.brand
                    },
                    {
                        name: 'galleries',
                        value: response.galleries?.join(',')
                    },
                    {
                        name: 'sku',
                        value: response.sku
                    },
                    {
                        name: 'content',
                        value: response.content
                    },
                    {
                        name: 'summary',
                        value: response.summary
                    },
                    {
                        name: 'cert1Name',
                        value: response.cert1Name
                    },
                    {
                        name: 'cert1File',
                        value: response.cert1File
                    },
                    {
                        name: 'cert2Name',
                        value: response.cert2Name
                    },
                    {
                        name: 'cert2File',
                        value: response.cert2File
                    }
                ])
                if (response.galleries) {
                    setFileList(response.galleries);
                }
            }
            setLoading(false);
        })
    }, [id]);

    const onFinish = async (values: any) => {
        values.catalogId = id;
        const response = await saveProduct(values);
        if (response.succeeded) {
            message.success('Saved');
        }
    }

    const onRemoveImage = async (url: string) => {
        const newList = fileList.filter(x => x !== url);
        setFileList(newList);
        formRef.current?.setFieldValue('galleries', newList.join(','));
        message.success('Xóa thành công');
    }

    const handleCancel = () => setPreviewOpen(false);
    
    const onUpload = async (response: any) => {
        let newList = fileList;
        newList.push(response.url);
        setFileList(newList);
        formRef.current?.setFieldValue('galleries', newList.join(','));
    }

    return (
        <>
            <ProForm onFinish={onFinish} formRef={formRef}>
                <ProForm.Item name="galleries" label="Hình ảnh">
                    <div className="flex gap-4 flex-wrap">
                        {
                            fileList.map((url, index) => (
                                <div key={index} className="relative">
                                    <Image src={url} alt="IMG" width={100} height={100} className="object-cover" />
                                    <Button icon={<DeleteOutlined />} size="small" danger type="primary" onClick={() => onRemoveImage(url)} style={{
                                        position: 'absolute',
                                        right: 0
                                    }} />
                                </div>
                            ))
                        }

                        <button type="button" className="btn-upload border border-dashed h-[100px] w-[100px] rounded hover:border-blue-500" onClick={() => setPreviewOpen(true)}>
                            <PlusOutlined />
                            <div style={{ marginTop: 8 }}>Upload</div>
                        </button>
                    </div>
                </ProForm.Item>
                <MyCKEditor name="summary" label="Tóm tắt" loading={loading} />
                <MyCKEditor name="content" label="Nội dung" loading={loading} />

                <Row gutter={16}>
                    <Col span={8}>
                        <ProFormDigit name="point" label="Điểm" rules={[
                            {
                                required: true
                            }
                        ]} />
                    </Col>
                    <Col span={8}>
                        <ProFormText name="brand" label="Thương hiệu" />
                    </Col>
                    <Col span={8}>
                        <ProFormText name="sku" label="SKU" />
                    </Col>
                    <Col md={8}>
                        <ProFormText name="cert1Name" label="Tên chứng nhận 1" />
                    </Col>
                    <Col md={16}>
                        <ProFormText name="cert1File" label="File chứng nhận 1" />
                    </Col>
                    <Col md={8}>
                        <ProFormText name="cert2Name" label="Tên chứng nhận 2" />
                    </Col>
                    <Col md={16}>
                        <ProFormText name="cert2File" label="File chứng nhận 2" />
                    </Col>
                </Row>
            </ProForm>
            <WfUpload open={previewOpen} onCancel={handleCancel} onFinish={onUpload} />
        </>
    )
}

export default ProductDetail