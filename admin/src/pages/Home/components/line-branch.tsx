import { apiLineChartMonth } from "@/services/order";
import { Column } from "@ant-design/charts";
import { Col } from "antd"
import { Dayjs } from "dayjs";
import { useEffect, useState } from "react";

type Props = {
    branchId: number;
    year?: Dayjs | null;
}

const LineBranch: React.FC<Props> = ({ branchId, year }) => {

    const [data, setData] = useState<any>();

    useEffect(() => {
        if (year) {
            apiLineChartMonth({
                year: year.year(),
                branchId: branchId
            }).then(response => setData(response));
        }
    }, [branchId, year])

    return (
        <Col xs={24} md={18}>
            <Column
                colorField={"amount"}
                legend={false}
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
                sizeField={50}
            />
        </Col>
    )
}

export default LineBranch;