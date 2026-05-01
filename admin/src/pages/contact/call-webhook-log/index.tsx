import { apiCallWebhookLogs, apiCallWebhookLogsExport } from "@/services/call";
import { ExportOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProColumns, ProTable } from "@ant-design/pro-components";
import { Button } from "antd";
import dayjs from "dayjs";
import { useRef, useState } from "react";

const Index: React.FC = () => {
    const actionRef = useRef<ActionType>();
    const [loadingExport, setLoadingExport] = useState(false);
    const [filterOptions, setFilterOptions] = useState<any>({});

    const mapRequestParams = (params: any) => ({
        ...params,
        billsecFrom: params?.billsecRange ? params.billsecRange[0] : undefined,
        billsecTo: params?.billsecRange ? params.billsecRange[1] : undefined,
        durationFrom: params?.durationRange ? params.durationRange[0] : undefined,
        durationTo: params?.durationRange ? params.durationRange[1] : undefined,
        timeAnsweredFrom: params?.timeAnsweredRange ? params.timeAnsweredRange[0] : undefined,
        timeAnsweredTo: params?.timeAnsweredRange ? params.timeAnsweredRange[1] : undefined,
        timeEndedFrom: params?.timeEndedRange ? params.timeEndedRange[0] : undefined,
        timeEndedTo: params?.timeEndedRange ? params.timeEndedRange[1] : undefined,
        timeStartedFrom: params?.timeStartedRange ? params.timeStartedRange[0] : undefined,
        timeStartedTo: params?.timeStartedRange ? params.timeStartedRange[1] : undefined,
        receivedDateFrom: params?.receivedDateRange ? params.receivedDateRange[0] : undefined,
        receivedDateTo: params?.receivedDateRange ? params.receivedDateRange[1] : undefined,
        billsecRange: undefined,
        durationRange: undefined,
        timeAnsweredRange: undefined,
        timeEndedRange: undefined,
        timeStartedRange: undefined,
        receivedDateRange: undefined,
    });

    const onExport = async () => {
        setLoadingExport(true);
        const response = await apiCallWebhookLogsExport(mapRequestParams(filterOptions));
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', `call_webhook_logs_${dayjs().format('YYYYMMDD_HHmmss')}.xlsx`);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
        setLoadingExport(false);
    }

    const columns: ProColumns<any>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
            width: 50,
            align: 'center'
        },
        {
            title: 'Application',
            dataIndex: 'application',
            width: 120,
        },
        {
            title: 'CallId',
            dataIndex: 'callId',
            width: 180,
        },
        {
            title: 'CampaignUuid',
            dataIndex: 'campaignUuid',
            width: 180,
            hideInTable: true,
        },
        {
            title: 'Direction',
            dataIndex: 'direction',
            width: 100,
        },
        {
            title: 'Domain',
            dataIndex: 'domain',
            width: 180,
        },
        {
            title: 'DomainUuid',
            dataIndex: 'domainUuid',
            hideInTable: true,
        },
        {
            title: 'FromNumber',
            dataIndex: 'fromNumber',
            width: 130,
        },
        {
            title: 'ToNumber',
            dataIndex: 'toNumber',
            width: 130,
        },
        {
            title: 'Hotline',
            dataIndex: 'hotline',
            width: 120,
            hideInTable: true,
        },
        {
            title: 'LeadUuid',
            dataIndex: 'leadUuid',
            hideInTable: true,
        },
        {
            title: 'PressKey',
            dataIndex: 'pressKey',
            hideInTable: true,
        },
        {
            title: 'ReceiveDest',
            dataIndex: 'receiveDest',
            hideInTable: true,
        },
        {
            title: 'RecordingUrl',
            dataIndex: 'recordingUrl',
            hideInTable: true,
        },
        {
            title: 'RefId',
            dataIndex: 'refId',
            hideInTable: true,
        },
        {
            title: 'SipCallId',
            dataIndex: 'sipCallId',
            width: 220,
            hideInTable: true,
        },
        {
            title: 'SipHangupDisposition',
            dataIndex: 'sipHangupDisposition',
            width: 180,
            hideInTable: true,
        },
        {
            title: 'State',
            dataIndex: 'state',
            width: 120,
        },
        {
            title: 'Status',
            dataIndex: 'status',
            width: 120,
        },
        {
            title: 'Billsec',
            dataIndex: 'billsec',
            search: false,
            width: 100,
        },
        {
            title: 'Khoảng Billsec',
            dataIndex: 'billsecRange',
            valueType: 'digitRange',
            hideInTable: true,
        },
        {
            title: 'Duration',
            dataIndex: 'duration',
            search: false,
            width: 100,
        },
        {
            title: 'Khoảng Duration',
            dataIndex: 'durationRange',
            valueType: 'digitRange',
            hideInTable: true,
        },
        {
            title: 'Time Started',
            dataIndex: 'timeStarted',
            valueType: 'dateTime',
            search: false,
            width: 170,
        },
        {
            title: 'Khoảng Time Started',
            dataIndex: 'timeStartedRange',
            valueType: 'dateTimeRange',
            hideInTable: true,
        },
        {
            title: 'Time Answered',
            dataIndex: 'timeAnswered',
            valueType: 'dateTime',
            search: false,
            width: 170,
        },
        {
            title: 'Khoảng Time Answered',
            dataIndex: 'timeAnsweredRange',
            valueType: 'dateTimeRange',
            hideInTable: true,
        },
        {
            title: 'Time Ended',
            dataIndex: 'timeEnded',
            valueType: 'dateTime',
            search: false,
            width: 170,
        },
        {
            title: 'Khoảng Time Ended',
            dataIndex: 'timeEndedRange',
            valueType: 'dateTimeRange',
            hideInTable: true,
        },
        {
            title: 'ReceivedDate',
            dataIndex: 'receivedDate',
            valueType: 'dateTime',
            search: false,
            width: 170,
        },
        {
            title: 'Khoảng ReceivedDate',
            dataIndex: 'receivedDateRange',
            valueType: 'dateTimeRange',
            hideInTable: true,
        },
    ];

    return (
        <PageContainer
            extra={<Button type="primary" icon={<ExportOutlined />} onClick={onExport} loading={loadingExport}>Xuất excel</Button>}
        >
            <ProTable
                actionRef={actionRef}
                rowKey="id"
                request={(params) => {
                    setFilterOptions(params);
                    return apiCallWebhookLogs(mapRequestParams(params));
                }}
                columns={columns}
                search={{
                    layout: 'vertical',
                    defaultCollapsed: false,
                }}
                scroll={{
                    x: true,
                }}
                size="small"
            />
        </PageContainer>
    );
}

export default Index;
