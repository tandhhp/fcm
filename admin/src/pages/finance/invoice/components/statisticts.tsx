import { apiInvoiceStatistics } from "@/services/finances/invoice";
import { ProCard, Statistic } from "@ant-design/pro-components";
import { FormattedNumber, useRequest } from "@umijs/max";

const InvoiceStatistics: React.FC = () => {

    const { data } = useRequest(apiInvoiceStatistics);

    return (
        <div className="mb-4 grid grid-cols-1 md:grid-cols-3 gap-4">
            <ProCard>
                <Statistic title="Hôm nay" value={data?.dailyCount} layout="vertical" />
                <div className="border-t border-dashed mt-2 pt-2">
                    Tổng thu: <b className="text-green-500"><FormattedNumber value={data?.dailyAmount} /> đ</b>
                </div>
            </ProCard>
            <ProCard>
                <Statistic title="Tuần này" value={data?.weeklyCount} layout="vertical" />
                <div className="border-t border-dashed mt-2 pt-2">
                    Tổng thu: <b className="text-green-500"><FormattedNumber value={data?.weeklyAmount} /> đ</b>
                </div>
            </ProCard>
            <ProCard>
                <Statistic title="Tháng này" value={data?.monthlyCount} layout="vertical" />
                <div className="border-t border-dashed mt-2 pt-2">
                    Tổng thu: <b className="text-green-500"><FormattedNumber value={data?.monthlyAmount} /> đ</b>
                </div>
            </ProCard>
        </div>
    )
}

export default InvoiceStatistics;