import { apiCardCreate, apiCardList, apiCardUpdate } from "@/services/settings/card";
import { EditOutlined, PlusOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProColumnType, ProFormInstance, ProFormText, ProTable } from "@ant-design/pro-components"
import { Button, message } from "antd";
import { useEffect, useRef, useState } from "react";

const CardPage: React.FC = () => {

    const [open, setOpen] = useState<boolean>();
    const actionRef = useRef<ActionType>();
    const formRef = useRef<ProFormInstance>();
    const [card, setCard] = useState<any>();

    useEffect(() => {
        if (card) {
            formRef.current?.setFields([
                {
                    name: 'id',
                    value: card.id
                },
                {
                    name: 'code',
                    value: card.code
                },
                {
                    name: 'name',
                    value: card.name
                }
            ]);
        }
    }, [card]);

    const columns: ProColumnType<any>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
            width: 50
        },
        {
            title: 'Mã thẻ',
            dataIndex: 'code',
            width: 150,
        },
        {
            title: 'Tên thẻ',
            dataIndex: 'name',
            width: 200,
        },
        {
            title: 'Số hợp đồng',
            dataIndex: 'contractCount',
            width: 150,
            valueType: 'digit',
            search: false
        },
        {
            title: 'Ngày cập nhật',
            dataIndex: 'modifiedDate',
            valueType: 'fromNow',
            search: false,
        },
        {
            title: 'Tác vụ',
            render: (dom, entity) => [
                <Button key="edit" icon={<EditOutlined />} type="primary" size="small" onClick={() => {
                    setCard(entity);
                    setOpen(true);
                }}></Button>
            ],
            valueType: 'option',
            width: 60,
        }
    ];

    const onFinish = async (values: any) => {
        if (values.id) {
            await apiCardUpdate(values);
        } else {
            await apiCardCreate(values);
        }
        message.success('Thành công!');
        actionRef.current?.reload();
        setCard(null);
        formRef.current?.resetFields();
        setOpen(false);
    }

    return (
        <PageContainer extra={<Button type="primary" icon={<PlusOutlined />} onClick={() => setOpen(true)}>Thêm thẻ</Button>}>
            <ProTable
                actionRef={actionRef}
                search={false}
                columns={columns}
                request={apiCardList}
            />
            <ModalForm open={open} onOpenChange={setOpen} title="Thẻ" onFinish={onFinish} formRef={formRef}>
                <ProFormText hidden name="id" />
                <ProFormText name="code" label="Mã thẻ" rules={[
                    {
                        required: true,
                        message: 'Vui lòng nhập mã thẻ'
                    }
                ]} />
                <ProFormText name={"name"} label="Tên thẻ" rules={[
                    {
                        required: true,
                        message: 'Vui lòng nhập tên thẻ'
                    }
                ]} />
            </ModalForm>
        </PageContainer>
    )
}

export default CardPage;