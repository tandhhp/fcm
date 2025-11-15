import { deleteContact, listContact } from "@/services/contact";
import { DeleteOutlined, EyeOutlined, FolderOutlined, MoreOutlined, StopOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProColumnType, ProTable } from "@ant-design/pro-components"
import { history, Link } from "@umijs/max";
import { Button, Dropdown, Popconfirm, Tooltip, message } from "antd";
import { useRef, useState } from "react";
import BlockContactModal from "./components/block-modal";

const ContactPage: React.FC = () => {

    const actionRef = useRef<ActionType>();
    const [contact, setContact] = useState<any>();
    const [openBlock, setOpenBlock] = useState<boolean>(false);

    const columns: ProColumnType<any>[] = [
        {
            title: 'STT',
            valueType: 'indexBorder',
            width: 50
        },
        {
            title: 'Họ và tên',
            dataIndex: 'name'
        },
        {
            title: 'Số điện thoại',
            dataIndex: 'phoneNumber'
        },
        {
            title: 'Email',
            dataIndex: 'email'
        },
        {
            title: 'Ngày liên hệ',
            dataIndex: 'createdDate',
            valueType: 'dateTime',
            search: false,
            width: 160
        },
        {
            title: 'Người giới thiệu',
            dataIndex: 'refName',
            search: false
        },
        {
            title: 'Ghi chú',
            dataIndex: 'note',
            search: false
        },
        {
            title: 'Tác vụ',
            valueType: 'option',
            render: (dom, entity) => [
                <Dropdown key="more" menu={{
                    items: [
                        {
                            key: 'view',
                            label: 'Chi tiết',
                            onClick: () => {
                                history.push(`/user/contact/activity/${entity.id}`);
                            },
                            icon: <EyeOutlined />
                        },
                        {
                            key: 'block',
                            label: 'Chặn liên hệ',
                            onClick: () => {
                                setContact(entity);
                                setOpenBlock(true);
                            },
                            icon: <StopOutlined />
                        }
                    ]
                }}>
                    <Button size="small" type="dashed" icon={<MoreOutlined />} />
                </Dropdown>,
                <Popconfirm key="delete" title="Bạn có chắc chắn muốn xóa?" onConfirm={async () => {
                    await deleteContact(entity.id);
                    message.success('Xóa thành công!');
                    actionRef.current?.reload();
                }}>
                    <Button type="primary" danger icon={<DeleteOutlined />} size="small"></Button>
                </Popconfirm>
            ],
            width: 60
        }
    ]

    return (
        <PageContainer>
            <ProTable
                scroll={{
                    x: true
                }}
                actionRef={actionRef}
                search={{
                    layout: 'vertical'
                }}
                request={listContact}
                columns={columns}
            />
            <BlockContactModal open={openBlock} contact={contact} reload={() => {
                actionRef.current?.reload();
            }} onOpenChange={setOpenBlock} />
        </PageContainer>
    )
}

export default ContactPage;