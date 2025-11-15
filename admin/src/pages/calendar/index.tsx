import { apiCalendarData } from "@/services/calendar";
import { PageContainer, ProCard, ProForm, ProFormSelect } from "@ant-design/pro-components";
import { useAccess } from "@umijs/max";
import { useRequest } from "@umijs/max";
import { Calendar, CalendarProps } from "antd";
import dayjs, { Dayjs } from "dayjs";
import { Fragment, useEffect, useState } from "react";
import Event from "./components/event";
import { CalendarOutlined, UserAddOutlined } from "@ant-design/icons";
import { apiBranchOptions } from "@/services/settings/branch";

const CalendarPage: React.FC = () => {

    const [month, setMonth] = useState<number>(dayjs().month() + 1); // month is 0-indexed in dayjs
    const [year, setYear] = useState<number>(dayjs().year());
    const { data, refresh } = useRequest(() => apiCalendarData({ month, year, branchId: 1 }));
    const access = useAccess();
    const [openEvent, setOpenEvent] = useState<boolean>(false);
    const [selectedDate, setSelectedDate] = useState<Dayjs | null>(null);

    useEffect(() => {
        refresh();
    }, [month, year]);

    const dateCellRender = (value: Dayjs) => {
        const listData = (data ?? []).filter((item: any) => item.day === value.date());
        if (listData.length === 0) {
            return <Fragment />; // No data for this date
        }
        if (listData[0].eventCount) {
            return (
                <div className="px-2">
                    {listData[0].eventCount > 0 && (
                        <div className="flex justify-between items-center bg-green-100 py-1 px-2 rounded font-semibold mb-1" onClick={() => {
                            setSelectedDate(value);
                            setOpenEvent(true);
                        }}>
                            <div><CalendarOutlined className="text-green-500" /> sự kiện</div>
                            <div className="w-5 h-5 rounded bg-green-500 text-white flex items-center justify-center text-xs">
                                {listData[0].eventCount}
                            </div>
                        </div>
                    )}
                </div>
            );
        }
        return (
            <ul className="events">
                {listData[0]?.items.map((item: any, idx: number) => (
                    <li key={idx}>
                        {item.content}
                    </li>
                ))}
            </ul>
        );
    };

    const cellRender: CalendarProps<Dayjs>['cellRender'] = (current, info) => {
        if (current.month() + 1 !== month || current.year() !== year) {
            return <Fragment />; // Only render cells for the current month and year
        }
        if (info.type === 'date') return dateCellRender(current);
        return info.originNode;
    };

    return (
        <PageContainer extra={(
            <ProForm submitter={false}>
                <ProFormSelect request={apiBranchOptions} initialValue={1} allowClear={false} name={`branchId`} formItemProps={{
                    className: 'mb-0'
                }} disabled={!access.canAdmin} />
            </ProForm>
        )}>
            <ProCard title={<div className="font-bold">Lịch làm việc</div>} headerBordered>
                <Calendar fullscreen cellRender={cellRender} onChange={(date) => {
                    setMonth(date.month() + 1); // month is 0-indexed in dayjs
                    setYear(date.year());
                }} />
            </ProCard>
            <Event open={openEvent} onClose={() => setOpenEvent(false)} selectedDate={selectedDate} />
        </PageContainer>
    )
}
export default CalendarPage;