import { apiLineChartMonth } from "@/services/order";
import { Line } from "@ant-design/charts";
import { ProCard } from "@ant-design/pro-components";
import { FormattedNumber, useAccess } from "@umijs/max";
import { Col, DatePicker } from "antd"
import dayjs, { Dayjs } from "dayjs";
import { useEffect, useState } from "react";

const LineMonth : React.FC = () => {

    const access = useAccess();
    const [data, setData] = useState<any>();
    const [year, setYear] = useState<Dayjs | null>(dayjs())

    useEffect(() => {
        if (year) {
            apiLineChartMonth({
                year: year.year()
            }).then(response => setData(response));
        }
    }, [year])

    return (
        <Col xs={24} md={12} hidden={!access.canViewChart}>
            <ProCard title="Doanh thu theo tháng" headerBordered extra={<DatePicker.YearPicker onChange={(v) => setYear(v)} value={year} />} className="mb-4">
                <Line
                    height={400}
                    className="h-full"
                    data={data}
                    xField='month'
                    yField="amount"
                    axis={{
                        y: {
                            labelFormatter: (v: any) => `${v}`.replace(/\d{1,3}(?=(\d{3})+$)/g, (s) => `${s},`)
                        }
                    }}
                />
            </ProCard>
        </Col>
    )
}

export default LineMonth;