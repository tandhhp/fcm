import { apiDeleteCombo, apiHealthcareContent, apiHealthcareContentSave, apiHealthcareList } from "@/services/healthcare";
import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";
import { ActionType, ProCard, ProColumnType, ProForm, ProFormInstance, ProFormText, ProTable } from "@ant-design/pro-components"
import { Link, useParams } from "@umijs/max";
import { Button, Col, Popconfirm, Row, message } from "antd"
import { useEffect, useRef, useState } from "react";
import HealthcareCombo from "./healthcare";
import MyCKEditor from "@/components/ckeditor";

const HospitalContent: React.FC = () => {

    const { id } = useParams();
    const formRef = useRef<ProFormInstance>();
    const [open, setOpen] = useState<boolean>(false);
    const [record, setRecord] = useState<any>();
    const actionRef = useRef<ActionType>();
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        if (id) {
            setLoading(true);
            apiHealthcareContent(id).then((response: any) => {
                if (response) {
                    formRef.current?.setFields([
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
                    ])
                }
                setLoading(false);
            })
        }
    }, [id]);

    const onFinish = async (values: any) => {
        values.catalogId = id;
        const response = await apiHealthcareContentSave(values);
        if (response.succeeded) {
            message.success('Lưu thành công!');
        }
    }

    const columns: ProColumnType<any>[] = [
        {
            title: 'STT',
            valueType: 'indexBorder',
            width: 50
        },
        {
            title: 'Tên gói khám',
            dataIndex: 'name'
        },
        {
            title: 'Điểm',
            dataIndex: 'point',
            valueType: 'digit',
            width: 100
        },
        {
            title: 'Ngày cập nhật',
            dataIndex: 'modifiedDate',
            valueType: 'fromNow',
            width: 150
        },
        {
            title: 'Lượt xem',
            dataIndex: 'viewCount',
            valueType: 'digit',
            width: 100
        },
        {
            title: 'Tác vụ',
            valueType: 'option',
            render: (dom, entity) => [
                <Link key="edit" to={`/catalog/hospital/package/${entity.catalogId}`}>
                    <Button type="primary" size="small" icon={<EditOutlined />} />
                </Link>,
                <Popconfirm key="delete" title="Xác nhận xóa" onConfirm={async () => {
                    await apiDeleteCombo(entity.id);
                    message.success('Xóa thành công!');
                    actionRef.current?.reload();
                }}>
                    <Button type="primary" danger size="small" icon={<DeleteOutlined />} />
                </Popconfirm>
            ],
            width: 100
        }
    ]

    return (
        <>
            <ProCard
                ghost
                size="small"
                extra={<Button icon={<PlusOutlined />} type="primary" onClick={() => {
                    setRecord(null);
                    setOpen(true);
                }}>Thêm gói khám</Button>}
            >
                <ProTable
                    actionRef={actionRef}
                    search={false}
                    ghost
                    columns={columns}
                    request={async () => {
                        const response = await apiHealthcareList(id);
                        return {
                            data: response,
                            total: response.length
                        }
                    }}
                    pagination={false}
                />
                <ProForm onFinish={onFinish} formRef={formRef} >
                    <MyCKEditor label="Nội dung" name="content" />
                    <Row gutter={16}>
                        <Col span={24}>
                            <ProFormText name="location" label="Địa điểm" />
                        </Col>
                    </Row>
                </ProForm>
            </ProCard>
            <HealthcareCombo open={open} setOpen={setOpen} reload={() => actionRef.current?.reload()} id={record?.catalogId} />
        </>
    )
}

export default HospitalContent