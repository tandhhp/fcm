import { apiCouponDelete, apiCouponList } from "@/services/finances/coupon";
import { DeleteOutlined, SettingOutlined } from "@ant-design/icons";
import { ActionType, ProTable } from "@ant-design/pro-components";
import { Button, Drawer, DrawerProps, message, Popconfirm } from "antd"
import { useRef } from "react";

type Props = DrawerProps & {
    data?: any;
    reload?: () => void;
}

const Coupon: React.FC<Props> = ({ data, reload, ...drawerProps }) => {

    const actionRef = useRef<ActionType>(null);

    const onDelete = async (id: string) => {
        await apiCouponDelete(id);
        message.success('Xóa phiếu quy đổi thành công');
        actionRef.current?.reload();
        reload?.();
    }

    return (
        <Drawer title="Phiếu quy đổi" {...drawerProps} destroyOnHidden={true} width={800}>
            <ProTable request={apiCouponList}
                actionRef={actionRef}
                params={{
                    contractId: data?.id
                }}
                ghost
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
                        title: 'Tên phiếu quy đổi',
                        dataIndex: 'name',
                    },
                    {
                        title: 'Ngày tạo',
                        dataIndex: 'createdDate',
                        valueType: 'date',
                    },
                    {
                        title: 'Giá trị (VNĐ)',
                        dataIndex: 'discount',
                        valueType: 'digit',
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 70,
                        align: 'center',
                        render: (_, record) => [
                            <Popconfirm key="delete" title="Bạn có chắc chắn muốn xóa phiếu quy đổi này?"
                                onConfirm={() => onDelete(record.id)}
                                okText="Xóa" cancelText="Hủy">
                                <Button size="small" type="primary" icon={<DeleteOutlined />} danger />
                            </Popconfirm>
                        ]
                    }
                ]}
            />
        </Drawer>
    )
}

export default Coupon;