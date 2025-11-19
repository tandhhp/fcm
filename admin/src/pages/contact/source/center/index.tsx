import { apiContactUnassignedList, deleteContact } from "@/services/contact";
import { DeleteOutlined, ImportOutlined, LeftOutlined, SettingOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { history, useAccess, useParams } from "@umijs/max";
import { Button, message, Popconfirm, Space } from "antd";
import { useRef, useState } from "react";
import { apiSourceOptions } from "@/services/settings/source";

const Index: React.FC = () => {

    const { id } = useParams<{ id: string }>();
    const access = useAccess();
    const actionRef = useRef<ActionType>();

    const onDelete = async (id: string) => {
        await deleteContact(id);
        message.success('Xoá liên hệ thành công');
        actionRef.current?.reload();
    }

    return (
        <PageContainer extra={<Button icon={<LeftOutlined />} onClick={() => history.back()}>Quay lại</Button>}>
            <ProTable
                actionRef={actionRef}
                request={apiContactUnassignedList}
                params={{
                    sourceId: id
                }}
                rowKey={"id"}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'name',
                        search: false,
                    },
                    {
                        title: 'Số điện thoại',
                        dataIndex: 'phoneNumber',
                    },
                    {
                        title: 'Email',
                        dataIndex: 'email',
                        search: false,
                    },
                    {
                        title: 'Người tạo',
                        dataIndex: 'creatorName',
                        search: false
                    },
                    {
                        title: 'Ngày tạo',
                        valueType: 'date',
                        dataIndex: 'createdDate',
                        search: false,
                        width: 100
                    },
                    {
                        title: 'Nguồn liên hệ',
                        dataIndex: 'sourceId',
                        valueType: 'select',
                        request: apiSourceOptions                        
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note',
                        search: false,
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 50,
                        render: (text, record) => [
                            <Popconfirm key="delete" title="Xoá liên hệ?" onConfirm={() => onDelete(record.id)}>
                                <Button icon={<DeleteOutlined />} type="primary" danger size="small"></Button>
                            </Popconfirm>
                        ]
                    }
                ]}
                search={{
                    layout: 'vertical'
                }}
            />
        </PageContainer>
    )
}

export default Index;