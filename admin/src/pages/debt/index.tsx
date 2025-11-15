import { apiSalesListReport } from "@/services/user";
import { PageContainer, ProCard, ProTable } from "@ant-design/pro-components";
import { FormattedNumber } from "@umijs/max";
import { Col, DatePicker, Row } from "antd";
import dayjs, { Dayjs } from "dayjs";
import { useEffect, useState } from "react";
import AmountReport from "../Home/components/amount";

const DebtPage: React.FC = () => {

    const [sales, setSales] = useState<any>([]);
    const [year, setYear] = useState<Dayjs | null>(dayjs());

    useEffect(() => {
        if (year) {
            apiSalesListReport(year.year()).then(response => setSales(response));
        }
    }, [year]);

    return (
        <PageContainer>
            <AmountReport />
            <Row gutter={16}>
                <Col xs={24} md={24}>
                    <ProCard title={`Năm ${dayjs().year()}`} headerBordered extra={<DatePicker.YearPicker value={year} onChange={(value) => {
                        setYear(value);
                    }} />}>
                        <div className="flex items-center">
                            <div className="border-b w-40 p-2">
                                Họ và tên
                            </div>
                            <div className="flex-1 flex">
                                <div className="flex-1 border-b p-2">
                                    Tháng 1
                                </div>
                                <div className="flex-1 border-b p-2">
                                    Tháng 2
                                </div>
                                <div className="flex-1 border-b p-2">
                                    Tháng 3
                                </div>
                                <div className="flex-1 border-b p-2">
                                    Tháng 4
                                </div>
                                <div className="flex-1 border-b p-2">
                                    Tháng 5
                                </div>
                                <div className="flex-1 border-b p-2">
                                    Tháng 6
                                </div>
                                <div className="flex-1 border-b p-2">
                                    Tháng 7
                                </div>
                                <div className="flex-1 border-b p-2">
                                    Tháng 8
                                </div>
                                <div className="flex-1 border-b p-2">
                                    Tháng 9
                                </div>
                                <div className="flex-1 border-b p-2">
                                    Tháng 10
                                </div>
                                <div className="flex-1 border-b p-2">
                                    Tháng 11
                                </div>
                                <div className="flex-1 border-b p-2">
                                    Tháng 12
                                </div>
                            </div>
                        </div>
                        {
                            sales.map((sale: any) => (
                                <div key={sale.id} className="flex items-center">
                                    <div className="border-b p-2 w-40">
                                        {sale.name}
                                    </div>
                                    <div className="flex-1 flex">
                                        {
                                            sale.months.map((month: any) => (
                                                <div key={month.month} className="flex-1 border-b p-2">
                                                    <FormattedNumber value={month.amount} />
                                                </div>
                                            ))
                                        }
                                    </div>
                                </div>
                            ))
                        }
                    </ProCard>
                </Col>
            </Row>
        </PageContainer>
    )
}

export default DebtPage;