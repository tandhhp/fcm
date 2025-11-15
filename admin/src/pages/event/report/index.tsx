import { apiEventSuReport } from "@/services/event";
import { apiAttendanceOptions } from "@/services/event/attendance";
import { apiDotOptions } from "@/services/role";
import { apiDosOptions } from "@/services/user";
import { ExportOutlined, ReloadOutlined } from "@ant-design/icons";
import { PageContainer, ProCard, ProForm, ProFormDatePicker, ProFormSelect } from "@ant-design/pro-components"
import { Button, Spin } from "antd";
import dayjs from "dayjs";
import { useEffect, useState } from "react";


type SUReportResult = {
    salesManagerName: string;
    salesReports: SUSalesReport[];
}

type SUSalesReport = {
    salesName: string;
    attendances: SUAttendance[];
}

type SUAttendance = {
    attendanceId: number;
    count: number;
    name: string;
}

const Index: React.FC = () => {

    const [data, setData] = useState<SUReportResult[]>([]);
    const [fromDate, setFromDate] = useState<string>(dayjs().startOf('month').format('YYYY-MM-DD'));
    const [toDate, setToDate] = useState<string>(dayjs().endOf('month').format('YYYY-MM-DD'));
    const [loading, setLoading] = useState<boolean>(false);
    const [atendanceOptions, setAttendanceOptions] = useState<any[]>([]);
    const [dotId, setDotId] = useState<string>('');
    const [dosId, setDosId] = useState<string>('');

    useEffect(() => {
        const fetchAttendanceOptions = async () => {
            const result = await apiAttendanceOptions();
            setAttendanceOptions(result);
        };
        fetchAttendanceOptions();
    }, []);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            const result = await apiEventSuReport({ fromDate, toDate, dosId, dotId });
            setData(result);
            setLoading(false);
        };
        fetchData();
    }, [fromDate, toDate, dosId, dotId]);

    return (
        <PageContainer extra={(
            <Button type="primary" icon={<ExportOutlined />} disabled>Xuất dữ liệu</Button>
        )}>
            <ProCard title="Báo cáo sự kiện" headerBordered extra={(
                <Button icon={<ReloadOutlined />} onClick={() => {
                    setFromDate(dayjs().startOf('month').format('YYYY-MM-DD'));
                    setToDate(dayjs().endOf('month').format('YYYY-MM-DD'));
                    setDosId('');
                    setDotId('');
                }}>
                    Làm mới
                </Button>
            )}>
                <div className="mb-4">
                    <ProForm layout="inline" submitter={false}>
                        <ProFormDatePicker name="fromDate" label="Từ ngày" initialValue={fromDate ? dayjs(fromDate) : undefined} fieldProps={{
                            onChange: (date: any) => {
                                setFromDate(date ? date.format('YYYY-MM-DD') : '');
                            },
                        }} />
                        <ProFormDatePicker name="toDate" label="Đến ngày" initialValue={toDate ? dayjs(toDate) : undefined} fieldProps={{
                            onChange: (date: any) => {
                                setToDate(date ? date.format('YYYY-MM-DD') : '');
                            },
                        }} />
                        <ProFormSelect name="dosId" label="DOS" placeholder="Chọn DOS" request={apiDosOptions}
                            fieldProps={{
                                onChange: (value: string) => {
                                    setDosId(value);
                                }
                            }}
                        />
                        <ProFormSelect name="dotId" label="DOT" placeholder="Chọn DOT" request={apiDotOptions}
                            fieldProps={{
                                onChange: (value: string) => {
                                    setDotId(value);
                                }
                            }}
                        />
                    </ProForm>
                </div>
                <Spin spinning={loading}>
                    <div className="overflow-x-auto">
                        <div className="p-1 border-b font-semibold flex bg-slate-100 min-w-[1366px]">
                            <div className="w-32">Sales Manager</div>
                            <div className="w-40">Sales</div>
                            {
                                atendanceOptions.map((option: any) => (
                                    <div key={option.value} className="flex-1">
                                        {option.label}
                                    </div>
                                ))
                            }
                            <div className="flex-1">
                                Tổng
                            </div>
                        </div>
                        {
                            data.map((item, index) => (
                                <div key={index} className="border-b hover:bg-slate-50 min-w-[1366px]">
                                    <div className="flex">
                                        <div className="p-1 w-32 flex items-center">{item.salesManagerName}</div>
                                        <div className="flex-1">
                                            {
                                                item.salesReports.map((report: SUSalesReport, idx: number) => (
                                                    <div key={idx} className="border-b last:border-0 flex border-dashed">
                                                        <div className="w-40 p-1">{report.salesName}</div>
                                                        <div className="flex-1 flex">
                                                            {report.attendances.map((attendance: SUAttendance, idx: number) => (
                                                                <div key={idx} className="flex-1 p-1">
                                                                    {attendance.count}
                                                                </div>
                                                            ))}
                                                        </div>
                                                    </div>
                                                ))
                                            }
                                        </div>
                                    </div>
                                </div>
                            ))
                        }
                    </div>
                </Spin>
            </ProCard>
        </PageContainer>
    )
}

export default Index;