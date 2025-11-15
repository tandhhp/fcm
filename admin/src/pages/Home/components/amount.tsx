import { apiAmountReport } from "@/services/user";
import { ProCard } from "@ant-design/pro-components";
import { FormattedNumber, useAccess, useRequest } from "@umijs/max";
import { Statistic } from "antd";

const AmountReport: React.FC = () => {

    const access = useAccess();
    const { data } = useRequest(apiAmountReport);

    return (
        <div className="mb-4">
            <div className="grid grid-cols-2 gap-4 md:grid-cols-4">
                <ProCard loading={access.telesale || access.cx}>
                    <Statistic title="Tổng doanh thu" value={data?.total} />
                    <div className="font-medium text-green-500 border-t border-dashed pt-1 flex justify-end">+<FormattedNumber value={data?.totalPending} /> đ</div>
                </ProCard>
                <ProCard loading={access.telesale || access.cx}>
                    <Statistic title="Doanh thu năm" value={data?.year} />
                    <div className="font-medium text-green-500 border-t border-dashed pt-1 flex justify-end">+<FormattedNumber value={data?.yearPending} /> đ</div>
                </ProCard>
                <ProCard loading={access.telesale || access.cx}>
                    <Statistic title="Doanh thu tháng này" value={data?.current} />
                    <div className="font-medium text-green-500 border-t border-dashed pt-1 flex justify-end">+<FormattedNumber value={data?.currentPending} /> đ</div>
                </ProCard>
                <ProCard loading={access.telesale || access.cx}>
                    <Statistic title="Doanh thu tháng trước" value={data?.prev} />
                    <div className="font-medium text-green-500 border-t border-dashed pt-1 flex justify-end">+<FormattedNumber value={data?.prevPending} /> đ</div>
                </ProCard>
            </div>
        </div>
    )
}

export default AmountReport;