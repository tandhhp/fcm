import { apiSmChartData } from "@/services/user";
import { ProCard } from "@ant-design/pro-components";
import { useRequest } from "@umijs/max";
import { Column } from "@ant-design/charts";

const SmChart: React.FC = () => {

    const { data, loading } = useRequest(apiSmChartData);

    return !loading && (
        <>
            <ProCard className="mb-4" title="Doanh thu SM" headerBordered>
                <Column
                    height={400}
                    data={data}
                    group={true}
                    xField="name"
                    yField="value"
                    colorField="type"
                    axis={{
                        y: {
                            labelFormatter: (v: any) => `${v}`.replace(/\d{1,3}(?=(\d{3})+$)/g, (s) => `${s},`)
                        }
                    }}
                />
            </ProCard>
        </>
    )
}

export default SmChart;