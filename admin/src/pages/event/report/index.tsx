import { apiEventSuReport, apiExportEventSuReport } from "@/services/event";
import { apiAttendanceOptions } from "@/services/event/attendance";
import { apiDotOptions, apiManagerOptions } from "@/services/role";
import { apiDirectorOptions, apiDosOptions } from "@/services/user";
import { ExportOutlined, ReloadOutlined } from "@ant-design/icons";
import { PageContainer, ProCard, ProForm, ProFormDatePicker, ProFormSelect } from "@ant-design/pro-components"
import { Link, useAccess } from "@umijs/max";
import { Avatar, Button, Empty, Spin, message } from "antd";
import dayjs from "dayjs";
import { useEffect, useMemo, useState } from "react";


type SUReportResult = {
    salesManagerName: string;
    salesReports: SUSalesReport[];
}

type SUSalesReport = {
    id: string;
    avatar?: string;
    salesName: string;
    attendances: SUAttendance[];
    totalKeyInCount: number;
    totalRate: number;
}

type SUAttendance = {
    attendanceId: number;
    count: number;
    name: string;
}

const Index: React.FC = () => {

    const access = useAccess();
    const [data, setData] = useState<SUReportResult[]>([]);
    const [fromDate, setFromDate] = useState<string>(dayjs().startOf('month').format('YYYY-MM-DD'));
    const [toDate, setToDate] = useState<string>(dayjs().endOf('month').format('YYYY-MM-DD'));
    const [loading, setLoading] = useState<boolean>(false);
    const [atendanceOptions, setAttendanceOptions] = useState<any[]>([]);
    const [directorId, setDirectorId] = useState<string>('');
    const [managerId, setManagerId] = useState<string>('');

    const summary = useMemo(() => {
        const totalSalesManager = data.length;
        const salesReports = data.flatMap((item) => item.salesReports ?? []);
        const totalSales = salesReports.length;
        const totalKeyIn = salesReports.reduce((sum, report) => sum + (report.totalKeyInCount || 0), 0);
        const totalRate = salesReports.reduce((sum, report) => sum + (report.totalRate || 0), 0);
        return {
            totalSalesManager,
            totalSales,
            totalKeyIn,
            totalRate,
        };
    }, [data]);

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
            const result = await apiEventSuReport({ fromDate, toDate, directorId, managerId });
            setData(result);
            setLoading(false);
        };
        fetchData();
    }, [fromDate, toDate, directorId, managerId]);

    const handleExport = async () => {
        try {
            setLoading(true);
            const blob = await apiExportEventSuReport({ fromDate, toDate, directorId, managerId });
            const url = window.URL.createObjectURL(new Blob([blob]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', `Bao_cao_su_kien_${dayjs().format('YYYYMMDD_HHmmss')}.xlsx`);
            document.body.appendChild(link);
            link.click();
            link.remove();
            window.URL.revokeObjectURL(url);
            message.success('Xuất dữ liệu thành công');
        } catch (error) {
            message.error('Xuất dữ liệu thất bại');
        } finally {
            setLoading(false);
        }
    };

    return (
        <PageContainer extra={(
            <Button type="primary"
                disabled={!access.em && !access.canAdmin && !access.dot}
                icon={<ExportOutlined />} onClick={handleExport}>Xuất dữ liệu</Button>
        )}>
            <ProCard title="Báo cáo sự kiện" headerBordered extra={(
                <Button icon={<ReloadOutlined />} onClick={() => {
                    setFromDate(dayjs().startOf('month').format('YYYY-MM-DD'));
                    setToDate(dayjs().endOf('month').format('YYYY-MM-DD'));
                    setDirectorId('');
                    setManagerId('');
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
                            autoFocus: false
                        }} />
                        <ProFormDatePicker name="toDate" label="Đến ngày" initialValue={toDate ? dayjs(toDate) : undefined} fieldProps={{
                            onChange: (date: any) => {
                                setToDate(date ? date.format('YYYY-MM-DD') : '');
                            },
                        }} />
                        <ProFormSelect name="directorId" label="Giám đốc" placeholder="Chọn Giám đốc" request={apiDirectorOptions}
                            fieldProps={{
                                onChange: (value: string) => {
                                    setDirectorId(value);
                                },
                                popupMatchSelectWidth: false
                            }}
                        />
                        <ProFormSelect name="managerId" label="Quản lý" placeholder="Chọn Quản lý" request={apiManagerOptions}
                            fieldProps={{
                                onChange: (value: string) => {
                                    setManagerId(value);
                                },
                                popupMatchSelectWidth: false
                            }} showSearch
                        />
                    </ProForm>
                </div>
                <div className="mb-4 rounded-xl border border-slate-200 bg-gradient-to-r from-cyan-50 via-white to-emerald-50 p-4">
                    <div className="grid grid-cols-1 gap-3 md:grid-cols-2 xl:grid-cols-4">
                        <div className="rounded-lg bg-white p-3 shadow-sm ring-1 ring-slate-100">
                            <div className="text-xs uppercase tracking-wide text-slate-500">Quản lý</div>
                            <div className="text-2xl font-semibold text-slate-800">{summary.totalSalesManager}</div>
                        </div>
                        <div className="rounded-lg bg-white p-3 shadow-sm ring-1 ring-slate-100">
                            <div className="text-xs uppercase tracking-wide text-slate-500">Nhân viên</div>
                            <div className="text-2xl font-semibold text-slate-800">{summary.totalSales}</div>
                        </div>
                        <div className="rounded-lg bg-white p-3 shadow-sm ring-1 ring-slate-100">
                            <div className="text-xs uppercase tracking-wide text-slate-500">Tổng keyin</div>
                            <div className="text-2xl font-semibold text-slate-800">{summary.totalKeyIn}</div>
                        </div>
                        <div className="rounded-lg bg-white p-3 shadow-sm ring-1 ring-slate-100">
                            <div className="text-xs uppercase tracking-wide text-slate-500">Tổng rate</div>
                            <div className="text-2xl font-semibold text-slate-800">{summary.totalRate.toFixed(2)}</div>
                        </div>
                    </div>
                </div>
                <Spin spinning={loading}>
                    {data.length === 0 ? (
                        <div className="rounded-xl border border-dashed border-slate-300 bg-slate-50 py-10">
                            <Empty description="Khong co du lieu trong khoang thoi gian da chon" />
                        </div>
                    ) : (
                        <div className="overflow-x-auto rounded-xl border border-slate-200 shadow-sm">
                            <div className="sticky top-0 z-10 flex min-w-[1366px] border-b bg-slate-100/95 px-3 py-2 text-sm font-semibold backdrop-blur">
                                <div className="w-36">Quản lý</div>
                                <div className="w-48">Nhân viên</div>
                                {
                                    atendanceOptions.map((option: any) => (
                                        <div key={option.value} className="flex-1 text-right">
                                            {option.label}
                                        </div>
                                    ))
                                }
                                <div className="w-28 text-right">Tổng</div>
                                <div className="w-28 text-right">Rate</div>
                            </div>
                            {
                                data.map((item, index) => {
                                    const managerAttendanceTotals = atendanceOptions.map((option: any) =>
                                        item.salesReports.reduce((sum, report) => {
                                            const matchedAttendance = report.attendances.find(
                                                (attendance) => String(attendance.attendanceId) === String(option.value)
                                            );
                                            return sum + (matchedAttendance?.count || 0);
                                        }, 0)
                                    );
                                    const managerTotalKeyIn = item.salesReports.reduce((sum, report) => sum + (report.totalKeyInCount || 0), 0);
                                    const managerTotalRate = item.salesReports.reduce((sum, report) => sum + (report.totalRate || 0), 0);

                                    return (
                                        <div key={index} className="min-w-[1366px] border-b last:border-b-0">
                                            <div className="flex">
                                                <div className="w-36 bg-slate-50 px-3 py-2 font-medium text-slate-700">
                                                    {item.salesManagerName}
                                                </div>
                                                <div className="flex-1">
                                                    {
                                                        item.salesReports.map((report: SUSalesReport, idx: number) => (
                                                            <div key={idx} className="flex border-b border-dashed px-3 py-2 text-sm last:border-b-0 odd:bg-white even:bg-slate-50/50 hover:bg-cyan-50/60">
                                                                <div className="w-48 font-medium text-slate-800">
                                                                    <Avatar size="small" className="mr-2" src={report.avatar} />
                                                                    <Link to={`/user/account/${report.id}`}>
                                                                        {report.salesName}
                                                                    </Link>
                                                                </div>
                                                                <div className="flex-1 flex">
                                                                    {report.attendances.map((attendance: SUAttendance, idx: number) => (
                                                                        <div key={idx} className="flex-1 text-right tabular-nums text-slate-700">
                                                                            {attendance.count}
                                                                        </div>
                                                                    ))}
                                                                </div>
                                                                <div className="w-28 text-right font-semibold tabular-nums text-cyan-700">
                                                                    {report.totalKeyInCount}
                                                                </div>
                                                                <div className="w-28 text-right font-medium tabular-nums text-slate-800">
                                                                    {report.totalRate.toFixed(2)}
                                                                </div>
                                                            </div>
                                                        ))
                                                    }
                                                    <div className="flex bg-orange-100/80 px-3 py-2 text-sm font-semibold text-slate-800">
                                                        <div className="w-48">Tổng quản lý</div>
                                                        <div className="flex-1 flex">
                                                            {managerAttendanceTotals.map((count: number, idx: number) => (
                                                                <div key={idx} className="flex-1 text-right tabular-nums text-slate-800">
                                                                    {count}
                                                                </div>
                                                            ))}
                                                        </div>
                                                        <div className="w-28 text-right tabular-nums text-cyan-700">
                                                            {managerTotalKeyIn}
                                                        </div>
                                                        <div className="w-28 text-right tabular-nums text-slate-900">
                                                            {managerTotalRate.toFixed(2)}
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    )
                                })
                            }
                        </div>
                    )}
                </Spin>
            </ProCard>
        </PageContainer>
    )
}

export default Index;