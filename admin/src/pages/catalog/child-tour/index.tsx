import { addCatalog, listCatalog } from "@/services/catalog";
import { EditOutlined, DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, ProColumnType, ProFormText, ProFormTextArea, ProTable } from "@ant-design/pro-components";
import { useParams, history } from "@umijs/max";
import { Button, Popconfirm, message } from "antd";
import { useRef, useState } from "react";
import { CatalogType } from "@/constants";
import FormCatalogList from "@/components/form/catalog-list";
import FormCatalogType from "@/components/form/catalog-type";
import { FormattedMessage } from "@umijs/max";

const ChildTour: React.FC<{
    resetKey: any;
}> = ({ resetKey }) => {

    const { id } = useParams();
    const actionRef = useRef<ActionType>();
    const [open, setOpen] = useState<boolean>(false);

    const columns: ProColumnType<any>[] = [
        {
            title: 'STT',
            valueType: 'indexBorder',
            width: 50
        },
        {
            title: 'Tên gói nghỉ dưỡng',
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
                <Button key="edit" type="primary" size="small" icon={<EditOutlined />} onClick={() => {
                    resetKey();
                    history.push(`/catalog/${entity.id}`)
                }} />,
                <Popconfirm key="delete" title="Xác nhận xóa" onConfirm={async () => {
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
            <div className="flex justify-end"><Button type="primary" icon={<PlusOutlined />} onClick={() => setOpen(true)}>Tạo gói nghỉ dưỡng</Button></div>
            <ProTable
                scroll={{
                    x: true
                }}
                actionRef={actionRef}
                search={false}
                ghost
                columns={columns}
                request={async (params, sort) => {
                    return await listCatalog({
                        parentId: id,
                        ...params
                    }, sort)
                }}
                pagination={false}
            />

            <ModalForm
                open={open}
                onOpenChange={setOpen}
                onFinish={async (values: any) => {
                    addCatalog(values).then(response => {
                        if (response) {
                            message.success('Tạo thành công!');
                            actionRef.current?.reload();
                        }
                        setOpen(false);
                    });
                }}
                title={<FormattedMessage id="general.new" />}
            >
                <ProFormText
                    name="name"
                    label="Tên"
                    rules={[
                        {
                            required: true,
                        },
                    ]}
                />
                <FormCatalogType label='Type' name='type' initialValue={CatalogType.Tour} disabled hidden />
                <FormCatalogList label="Parent" name="parentId" initialValue={id} hidden />
                <ProFormTextArea label="Mô tả" name="description" />
            </ModalForm>
        </>
    )
}

export default ChildTour;