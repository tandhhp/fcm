import { apiContractGiftDelete, apiContractGiftList } from "@/services/finances/contract";
import { DeleteOutlined, SettingOutlined } from "@ant-design/icons";
import { ActionType, DrawerForm, DrawerFormProps, ProTable } from "@ant-design/pro-components";
import { Button, Popconfirm } from "antd";
import { useEffect, useRef } from "react";

type Props = DrawerFormProps & {
    data?: any;
    reload?: () => void;
}

const GiftList: React.FC<Props> = (props) => {

    const actionRef = useRef<ActionType>(null);

    useEffect(() => {
        if (props.open && props.data) {
            actionRef.current?.reload();
        }
    }, [props.open, props.data]);

    const onDelete = async (id: string) => {
        await apiContractGiftDelete({
            giftId: id,
            contractId: props.data?.id
        });
        actionRef.current?.reload();
        props.reload?.();
    }

    return (
        <DrawerForm {...props} title="Danh sách quà tặng">
            <ProTable
                actionRef={actionRef}
                ghost
                request={apiContractGiftList}
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
                        dataIndex: 'createdByName',
                    },
                    {
                        title: 'Ngày tạo',
                        dataIndex: 'createdAt',
                        valueType: 'dateTime',
                        width: 180
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        render: (_, record) => [
                            <Popconfirm title="Bạn có chắc chắn xoá quà tặng này?" okText="Xoá" cancelText="Huỷ" key="delete" onConfirm={() => onDelete(record.id)}>
                                <Button type="primary" danger icon={<DeleteOutlined />} size="small" />
                            </Popconfirm>
                        ],
                        width: 40,
                        align: 'center'
                    }
                ]}
            />
        </DrawerForm>
    )
}

export default GiftList;