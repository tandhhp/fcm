import { apiListTopSales } from "@/services/user";
import { ProTable } from "@ant-design/pro-components"

const TopSales: React.FC = () => {
    return (
        <ProTable
            ghost
            request={apiListTopSales}
            columns={[
                {
                    title: '#',
                    valueType: 'indexBorder',
                    width: 30
                },
                {
                    title: 'Họ & tên',
                    dataIndex: 'key'
                },
                {
                    title: 'Doanh số',
                    dataIndex: 'amount',
                    valueType: 'digit',
                    width: 100
                }
            ]}
            className="mb-4"
            search={false}
            headerTitle="Top Tháng"
        />
    )
}

export default TopSales;