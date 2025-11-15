import MyCKEditor from "@/components/ckeditor";
import WfUpload from "@/components/file-explorer/upload";
import { CatalogType } from "@/constants";
import { apiLocations, apiTourContent, apiTourContentSave } from "@/services/catalog";
import { apiAmenities } from "@/services/tour";
import { DeleteOutlined, PlusOutlined, SaveOutlined } from "@ant-design/icons";
import { ProFormSelect } from "@ant-design/pro-components";
import { useParams } from "@umijs/max";
import { Button, Col, Form, Image, InputNumber, Row, message } from "antd";
import { useEffect, useState } from "react";

const CatalogContent: React.FC = () => {

    const [fileList, setFileList] = useState<string[]>([]);
    const { id } = useParams();
    const [form] = Form.useForm();
    const [loading, setLoading] = useState<boolean>(true);
    const [catalog, setCatalog] = useState<any>();

    const [previewOpen, setPreviewOpen] = useState(false);

    useEffect(() => {
        if (id) {
            setLoading(true);
            apiTourContent(id).then(response => {
                if (response.images) {
                    setFileList(response.images);
                }
                form.setFields([
                    {
                        name: 'images',
                        value: response.images?.join(',')
                    },
                    {
                        name: 'point',
                        value: response.point
                    },
                    {
                        name: 'location',
                        value: response.location
                    },
                    {
                        name: 'content',
                        value: response.content
                    }
                ]);
                const amenities = response.amenities as any[] || [];
                if (amenities.length > 0) {
                    form.setFieldValue('amenities', amenities.map(x => x.id));
                }
                setCatalog(response);
                setLoading(false)
            })
        } else {
            setLoading(false)
        }
    }, [id]);

    const handleCancel = () => setPreviewOpen(false);

    const onUpload = async (response: any) => {
        let newList = fileList;
        newList.push(response.url);
        setFileList(newList);
        form.setFieldValue('images', newList.join(','));
    }

    const onRemoveImage = async (url: string) => {
        const newList = fileList.filter(x => x !== url);
        setFileList(newList);
        form.setFieldValue('images', newList.join(','));
        message.success('Xóa thành công');
    }

    const onFinish = async (values: any) => {
        values.catalogId = id;
        values.amenities = values.amenities.join(',');
        await apiTourContentSave(values);
        message.success('Lưu thành công!');
    }

    return (
        <div>
            <Form layout="vertical" form={form} onFinish={onFinish}>
                <Form.Item name="images" label="Hình ảnh về tour">
                    <div className="flex gap-4 flex-wrap">
                        {
                            fileList.map((url, index) => (
                                <div key={index} className="relative">
                                    <Image src={url} alt="IMG" width={120} height={100} className="object-cover" />
                                    <Button icon={<DeleteOutlined />} size="small" danger type="primary" onClick={() => onRemoveImage(url)} style={{
                                        position: 'absolute',
                                        right: 0
                                    }} />
                                </div>
                            ))
                        }

                        <button type="button" className="btn-upload border w-28 h-28 hover:bg-slate-100 border-dashed" onClick={() => setPreviewOpen(true)}>
                            <PlusOutlined />
                            <div style={{ marginTop: 8 }}>Upload</div>
                        </button>
                    </div>
                </Form.Item>
                <MyCKEditor name='content' label="Nội dung" loading={loading} />
                <ProFormSelect request={apiAmenities} mode="multiple" label="Tiện ích" name="amenities" />
                <Row gutter={16}>
                    <Col md={4} hidden={!catalog?.parentId}>
                        <Form.Item label="Số điểm" name="point">
                            <InputNumber className="w-full" disabled={catalog?.type === CatalogType.Room} />
                        </Form.Item>
                    </Col>
                    <Col md={6}>
                        <ProFormSelect request={apiLocations} label="Địa điểm" name="location" showSearch />
                    </Col>
                </Row>
                <Form.Item>
                    <Button type="primary" htmlType="submit" icon={<SaveOutlined />}>Lưu lại</Button>
                </Form.Item>
            </Form>
            <WfUpload open={previewOpen} onCancel={handleCancel} onFinish={onUpload} />
        </div>
    )
}

export default CatalogContent;