import MyCKEditor from "@/components/ckeditor";
import { apiItineraryDelete, apiItineraryList, apiItinernaryAdd, apiItinernaryUpadte, apiSaveTourPoint, apiTourContent } from "@/services/catalog";
import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, ProCard, ProForm, ProFormDigit, ProFormInstance, ProFormText, ProList } from "@ant-design/pro-components";
import { useParams } from "@umijs/max";
import { Button, Popconfirm } from "antd";
import { message } from "antd/es";
import { useEffect, useRef, useState } from "react";

const Itinerary: React.FC = () => {

    const [open, setOpen] = useState<boolean>(false);
    const { id } = useParams();
    const actionRef = useRef<ActionType>();
    const [itinerary, setItinerary] = useState<any>();
    const formRef = useRef<ProFormInstance>();
    const formContentRef = useRef<ProFormInstance>();
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        if (id) {
            apiTourContent(id).then(response => {
                formContentRef.current?.setFields([
                    {
                        name: 'point',
                        value: response.point
                    }
                ])
            })
        }
    }, [id]);

    useEffect(() => {
        if (itinerary) {
            setLoading(true);
            formRef.current?.setFields([
                {
                    name: 'id',
                    value: itinerary.id
                },
                {
                    name: 'title',
                    value: itinerary.title
                },
                {
                    name: 'content',
                    value: itinerary.content
                }
            ]);
            setLoading(false);
        } else {
            formRef.current?.resetFields();
            setLoading(false);
        }
    }, [itinerary]);

    const onFinish = async (values: any) => {
        values.catalogId = id;
        if (values.id) {
            await apiItinernaryUpadte(values);
            message.success('Cập nhật thành công!');
        } else {
            await apiItinernaryAdd(values);
            message.success('Thêm thành công!');
        }
        formRef.current?.resetFields();
        actionRef.current?.reload();
        setItinerary(null);
        setOpen(false);
    }

    const onDelete = async (id: string) => {
        await apiItineraryDelete(id);
        message.success('Xóa thành công!');
        actionRef.current?.reload();
    }

    return (
        <>
            <ProCard title="Thông tin gói" bordered headerBordered className="mb-4">
                <ProForm formRef={formContentRef} onFinish={async (values: any) => {
                    await apiSaveTourPoint(values);
                    message.success('Lưu thành công!');
                }}>
                    <ProFormText name="catalogId" hidden initialValue={id} />
                    <ProFormDigit name="point" label="Điểm" />
                </ProForm>
            </ProCard>
            <ProCard
                title="Hành trình" bordered headerBordered
                extra={<Button type="primary" icon={<PlusOutlined />} onClick={() => setOpen(true)}>Tạo hành trình</Button>}
            >
                <ProList
                    headerTitle="Danh sách hành trình"
                    ghost
                    actionRef={actionRef}
                    request={() => apiItineraryList(id)}
                    metas={{
                        title: {
                            dataIndex: 'title'
                        },
                        description: {
                            dataIndex: 'content',
                            render: (text, row: any) => <div className="line-clamp-2" dangerouslySetInnerHTML={{ __html: row.content }} />
                        },
                        actions: {
                            render: (text, row: any) => [
                                <Button type="primary" key="edit" size="small" icon={<EditOutlined />} onClick={() => {
                                    setItinerary(row);
                                    setOpen(true);
                                }} />,
                                <Popconfirm key="del" title="Bạn có chắc chắn muốn xóa?" onConfirm={() => onDelete(row.id)}>
                                    <Button size="small" type="primary" danger icon={<DeleteOutlined />} />
                                </Popconfirm>
                            ]
                        }
                    }}
                />
            </ProCard>
            <ModalForm
                width={1000}
                formRef={formRef}
                open={open} onOpenChange={setOpen} title="Hành trình" onFinish={onFinish}>
                <ProFormText name="id" hidden />
                <ProFormText name="title" label="Tiêu đề" />
                <MyCKEditor name="content" label="Nội dung" loading={loading} open={open} />
            </ModalForm>
        </>
    )
}

export default Itinerary;