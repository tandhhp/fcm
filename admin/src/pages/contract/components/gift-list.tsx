import { DeleteOutlined, EditOutlined, PlusOutlined, SettingOutlined } from "@ant-design/icons";
import { ActionType, DrawerForm, DrawerFormProps, ProTable } from "@ant-design/pro-components";
import { Button, Popconfirm } from "antd";
import { useEffect, useRef, useState } from "react";
import GiftForm from "./gift-form";
import { apiGiftDelete, apiGiftList } from "@/services/event/gift";

type Props = DrawerFormProps & {
    data?: any;
    reload?: () => void;
}

const GiftList: React.FC<Props> = (props) => {

    const actionRef = useRef<ActionType>(null);
    const [giftFormOpen, setGiftFormOpen] = useState<boolean>(false);
    const [gift, setGift] = useState<any>(null);

    useEffect(() => {
        if (props.open && props.data) {
            actionRef.current?.reload();
        }
    }, [props.open, props.data]);

    const onDelete = async (id: string) => {
        await apiGiftDelete(id);
        actionRef.current?.reload();
        props.reload?.();
    }

    return (
        <DrawerForm {...props} title="Danh sách quà tặng" submitter={false} width={1000}>
            <ProTable
                headerTitle={
                    <Button type="primary" icon={<PlusOutlined />} onClick={() => {
                        setGift(null);
                        setGiftFormOpen(true);
                    }}>
                        Thêm quà tặng
                    </Button>
                }
                actionRef={actionRef}
                ghost
                request={apiGiftList}
                params={{
                    contractId: props.data?.id
                }}
                rowKey="id"
                search={false}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Tên quà tặng',
                        dataIndex: 'name',
                    },
                    {
                        title: 'Người tặng',
                        dataIndex: 'userName',
                    },
                    {
                        title: 'Ngày tạo',
                        dataIndex: 'createdAt',
                        valueType: 'dateTime',
                        width: 180
                    },
                    {
                        title: 'Giá trị',
                        dataIndex: 'amount',
                        valueType: 'digit'
                    },
                    {
                        title: 'Hạn sử dụng',
                        dataIndex: 'expiredDate',
                        valueType: 'date'
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        render: (_, record) => [
                            <Button key="edit" type="primary" icon={<EditOutlined />} size="small" onClick={() => {
                                setGift(record);
                                setGiftFormOpen(true);
                            }} />,
                            <Popconfirm title="Bạn có chắc chắn xoá quà tặng này?" okText="Xoá" cancelText="Huỷ" key="delete" onConfirm={() => onDelete(record.id)}>
                                <Button type="primary" danger icon={<DeleteOutlined />} size="small" />
                            </Popconfirm>
                        ],
                        width: 40,
                        align: 'center'
                    }
                ]}
            />

            <GiftForm open={giftFormOpen}
                contractId={props.data?.id}
                onOpenChange={setGiftFormOpen} data={gift} reload={() => actionRef.current?.reload()} />
        </DrawerForm>
    )
}

export default GiftList;