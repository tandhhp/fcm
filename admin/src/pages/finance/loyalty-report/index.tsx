import { apiReportLoyalty } from "@/services/order";
import { PageContainer, ProTable } from "@ant-design/pro-components"

const Index: React.FC = () => {
    return (
        <PageContainer>
            <ProTable
                rowKey="id"
                request={(params) => apiReportLoyalty(params)}
                columns={[
                    {
                        title: 'Mã khách hàng',
                        dataIndex: 'userName',
                        width: 200,
                        search: false
                    },
                    {
                        title: 'Từ ngày',
                        dataIndex: 'fromDate',
                        valueType: 'date',
                        hideInTable: true,
                    },
                    {
                        title: 'Đến ngày',
                        dataIndex: 'toDate',
                        valueType: 'date',
                        hideInTable: true,
                    },
                    {
                        title: 'Tên khách hàng',
                        dataIndex: 'name',
                        width: 150
                    },
                    {
                        title: 'Số HD',
                        dataIndex: 'contractCode',
                        width: 150
                    },
                    {
                        title: 'Tổng điểm hợp đồng',
                        dataIndex: 'maxLoyalty',
                        search: false
                    },
                    {
                        title: 'Why Now',
                        dataIndex: 'whynow',
                        search: false
                    },
                    {
                        title: 'Số điểm nạp thêm trong kỳ',
                        dataIndex: 'deposit',
                        search: false
                    },
                    {
                        title: 'Số điểm sử dụng trong kỳ',
                        dataIndex: 'withdraw',
                        search: false
                    },
                    {
                        title: 'Số điểm còn lại cuối kỳ',
                        dataIndex: 'loyalty',
                        search: false,
                    }
                ]}
                search={{
                    layout: 'vertical'
                }}
                pagination={{
                    pageSize: 10,
                }}
            />
        </PageContainer>
    )
}

export default Index;