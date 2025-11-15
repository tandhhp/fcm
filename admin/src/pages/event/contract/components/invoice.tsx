import { apiContractInvoiceList } from "@/services/finances/contract";
import { DrawerForm, DrawerFormProps, ProTable } from "@ant-design/pro-components";

type Props = DrawerFormProps & {
    data?: any;
}

const ContractInvoice: React.FC<Props> = (props) => {
    return (
        <DrawerForm {...props} title="Hóa đơn hợp đồng" submitter={false} width={1000}>
            <ProTable
                request={apiContractInvoiceList}
                params={{
                    contractId: props.data?.id
                }}
                rowKey={"id"}
                search={false}
                ghost
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Mã phiếu thu',
                        dataIndex: 'invoiceNumber',
                        search: false
                    },
                    {
                        title: 'Ngày thu',
                        dataIndex: 'createdAt',
                        valueType: 'date',
                    },
                    {
                        title: 'Số tiền',
                        dataIndex: 'amount',
                        valueType: 'digit',
                        search: false
                    },
                    {
                        title: 'Phương thức',
                        dataIndex: 'paymentMethod',
                        valueEnum: {
                            0: { text: 'Chuyển khoản' },
                            1: { text: 'Thẻ' },
                            2: { text: 'Tiền mặt' }
                        },
                        search: false
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'status',
                        valueEnum: {
                            0: { text: 'Chờ duyệt', status: 'Processing' },
                            1: { text: 'Đã duyệt', status: 'Success' },
                            2: { text: 'Từ chối', status: 'Error' },
                            3: { text: 'Hủy', status: 'Default' },
                        },
                        search: false
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note',
                        search: false
                    }
                ]}
            />
        </DrawerForm>
    )
}

export default ContractInvoice;