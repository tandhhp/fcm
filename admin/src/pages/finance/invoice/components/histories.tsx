import { apiInvoiceHistories } from "@/services/finances/invoice";
import { DrawerForm, DrawerFormProps, ProTable } from "@ant-design/pro-components";
import dayjs from "dayjs";

type Props = DrawerFormProps & {
    invoice?: any;
}

const InvoiceHistories: React.FC<Props> = ({ invoice, ...props }) => {
    return (
        <DrawerForm {...props}
            drawerProps={{
                destroyOnHidden: true
            }}
            title={`Lịch sử phiếu thu: ${invoice?.invoiceNumber || ''}`} width={800} submitter={false}>
            <ProTable
                search={false}
                rowKey="id"
                ghost
                request={apiInvoiceHistories}
                params={{ invoiceId: invoice?.id }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Ngày',
                        dataIndex: 'createdAt',
                        render: (_, record) => (
                            record.createdAt ? dayjs(record.createdAt).format('DD/MM/YYYY HH:mm') : ''
                        )
                    },
                    {
                        title: 'Người thực hiện',
                        dataIndex: 'userName'
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'status',
                        valueEnum: {
                            0: { text: 'Chờ duyệt', status: 'Default' },
                            1: { text: 'Đã duyệt', status: 'Success' },
                            2: { text: 'Từ chối', status: 'Error' },
                            3: { text: 'Hủy', status: 'Warning' },
                            4: { text: 'SA xác nhận', status: 'Processing' }
                        }
                    },
                    {
                        title: 'Hành động',
                        dataIndex: 'note'
                    }
                ]}
            />
        </DrawerForm>
    );
}

export default InvoiceHistories;