import { DeleteOutlined, InfoCircleOutlined, PlusOutlined } from "@ant-design/icons"
import { ModalForm, ProCard, ProForm, ProFormText, ProList } from "@ant-design/pro-components"
import { Button, Modal, Popconfirm, message } from "antd"
import { useState } from "react"

type Props = {
    dataSource: any[];
    setDataSource: any;
}

const Blogger: React.FC<Props> = ({ dataSource, setDataSource }) => {

    const [open, setOpen] = useState<boolean>(false);
    const onFinish = async (values: any) => {
        if (dataSource.find(x => x.id === values.id)) {
            message.warning('Data already exists!');
            return;
        }
        const newData = [values, ...dataSource];
        setDataSource(newData);
        message.success('Added!');
        setOpen(false);
    }

    const onConfirm = (id: string) => {
        setDataSource(dataSource.filter(x => x.id !== id));
        message.success('Deleted!');
    }

    return (
        <ProCard title="Blogger" extra={<a href='https://blogger.com/' target='_blank'><InfoCircleOutlined /></a>}>
            <ProFormText.Password name="bloggerApiKey" label="Blogger API Key" />
            <ProFormText.Password name="clientId" label="Client ID" />
            <ProList<{
                id: string;
                name: string;
            }>
                headerTitle="List Blogger"
                dataSource={dataSource}
                ghost
                toolBarRender={() => [
                    <Button size='small' icon={<PlusOutlined />} type='text' onClick={() => setOpen(true)} />
                ]}
                metas={{
                    title: {
                        dataIndex: 'id'
                    },
                    description: {
                        dataIndex: 'name'
                    },
                    actions: {
                        render: (dom, entity) => [
                            <Popconfirm title="Are you sure?" key="delete" onConfirm={() => onConfirm(entity.id)}>
                                <Button type="text" danger icon={<DeleteOutlined />} size="small"></Button>
                            </Popconfirm>
                        ]
                    }
                }}
            />

            <ModalForm open={open} onOpenChange={setOpen} title="Add List" onFinish={onFinish}>
                <ProFormText label="ID" name="id" rules={[
                    {
                        required: true
                    }
                ]} />
                <ProFormText label="Name" name="name" rules={[
                    {
                        required: true
                    }
                ]} />
            </ModalForm>
        </ProCard>
    )
}

export default Blogger