import { apiMonthlySalesReport, apiSalesListReport } from "@/services/user";
import { PageContainer, ProCard, ProTable } from "@ant-design/pro-components";
import { FormattedNumber } from "@umijs/max";
import { Col, DatePicker, Row } from "antd";
import dayjs, { Dayjs } from "dayjs";
import { useEffect, useState } from "react";
import AmountReport from "../Home/components/amount";

const DebtPage: React.FC = () => {

    const [sales, setSales] = useState<any>([]);
    const [year, setYear] = useState<Dayjs | null>(dayjs());
    const [month, setMonth] = useState<Dayjs | null>(dayjs());
    const [monthlySales, setMonthlySales] = useState<any>([]);
    const [days, setDays] = useState<number[]>([]);

    useEffect(() => {
        if (year) {
            apiSalesListReport(year.year()).then(response => setSales(response));
        }
    }, [year]);

    useEffect(() => {
        if (year && month) {
            apiMonthlySalesReport(year.year(), month.month() + 1).then(response => {
                setMonthlySales(response);
            });
            const fistDay = dayjs(`${year.year()}-${month.month() + 1}-01`);
            const daysInMonth = fistDay.daysInMonth();
            const dayList = [];
            for (let i = 1; i <= daysInMonth; i++) {
                dayList.push(i);
            }
            setDays(dayList);
        }
    }, [year, month]);

    return (
        <PageContainer>
            <AmountReport />
            <Row gutter={16}>
                <Col xs={24} md={24}>
                    <ProCard
                        tabs={{
                            items: [
                                {
                                    key: 'year',
                                    label: 'Năm',
                                    children: (
                                        <div>
                                            <div className="mb-4 flex justify-end">
                                                <DatePicker.YearPicker value={year} onChange={(value) => {
                                                    setYear(value);
                                                }} />
                                            </div>
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
                                        </div>
                                    )
                                },
                                {
                                    key: 'month',
                                    label: 'Tháng',
                                    children: (
                                        <div className="overflow-auto">
                                            <div className="min-w-[4000px]">
                                                <div className="mb-4">
                                                    <DatePicker.MonthPicker value={month} onChange={(value) => {
                                                        setMonth(value);
                                                        setYear(value);
                                                    }} />
                                                </div>
                                                <div className="flex items-center bg-slate-200">
                                                    <div className="border-b w-64 p-2">
                                                        Họ và tên
                                                    </div>
                                                    <div className="flex-1 flex">
                                                        {days.map((day) => (
                                                            <div key={day} className="flex-1 border-b p-2">
                                                                {day}
                                                            </div>
                                                        ))}
                                                    </div>
                                                </div>
                                                {
                                                    monthlySales.map((sale: any) => (
                                                        <div key={sale.id} className="flex items-center">
                                                            <div className="border-b p-2 w-64">
                                                                {sale.name}
                                                            </div>
                                                            <div className="flex-1 flex">
                                                                {
                                                                    sale.days.map((day: any) => (
                                                                        <div key={day.day} className="flex-1 border-b border-r p-2">
                                                                            <FormattedNumber value={day.amount} />
                                                                        </div>
                                                                    ))
                                                                }
                                                            </div>
                                                        </div>
                                                    ))
                                                }
                                            </div>
                                        </div>
                                    )
                                }
                            ]
                        }}>

                    </ProCard>
                </Col>
            </Row>
        </PageContainer>
    )
}

export default DebtPage;