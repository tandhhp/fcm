import { apiCallReportCdr } from "@/services/call";
import { ManOutlined, WomanOutlined } from "@ant-design/icons";
import { PageContainer, ProTable } from "@ant-design/pro-components"
import { Tag } from "antd";

const Index: React.FC = () => {
    return (
        <PageContainer>
            <ProTable 
            request={apiCallReportCdr}
            search={{
                layout: 'vertical'
            }}
            rowKey={"id"}
            columns={[
                {
                    title: '#',
                    valueType: 'indexBorder',
                    width: 30,
                    align: 'center'
                },
                {
                    title: 'Code',
                    dataIndex: 'username',
                    width: 60,
                },
                {
                    title: 'SDT',
                    dataIndex: 'callee_id_name',
                    width: 100,
                    render: (dom, record) => {
                        return <a href={`tel:${record.callee_id_name}`} className="text-yellow-500">{record.callee_id_name}</a>
                    }
                },
                {
                    title: 'Khoảng thời gian',
                    dataIndex: 'time_range',
                    valueType: 'dateRange',
                    hideInTable: true
                },
                {
                    title: 'Thời gian',
                    dataIndex: 'duration',
                    valueType: 'digit',
                    search: false,
                    render: (dom, record) => {
                        if (!record.duration) return '0 giây';
                        const minutes = Math.floor(record.duration / 60);
                        const seconds = record.duration % 60;
                        return `${minutes} phút ${seconds} giây`;
                    }
                },
                {
                    title: 'Nhà mạng',
                    dataIndex: 'network',
                    search: false,
                    valueEnum: {
                        'viettel': <Tag color="red" className="w-full text-center">Viettel</Tag>,
                        'mobi': <Tag color="blue" className="w-full text-center">Mobifone</Tag>,
                        'vina': <Tag color="green" className="w-full text-center">Vinaphone</Tag>,
                        "dnc": <Tag color="gray" className="w-full text-center">Không rõ</Tag>
                    },
                    width: 100
                },
                {
                    title: 'Gọi lúc',
                    dataIndex: 'time_started',
                    valueType: 'dateTime',
                    width: 170,
                    search: false,
                },
                {
                    title: 'Kết thúc lúc',
                    dataIndex: 'time_ended',
                    valueType: 'dateTime',
                    width: 170,
                    search: false,
                },
                {
                    title: 'Trạng thái',
                    dataIndex: 'status',
                    valueEnum: {
                        "busy-line": { text: 'Bận', status: 'Error' },
                        "not-available": { text: 'Không liên lạc được', status: 'Error' },
                        "answered": { text: 'Đã nghe máy', status: 'Success' },
                        "cancel": { text: 'Hủy cuộc gọi', status: 'Warning' },
                        "busy": { text: 'Bận', status: 'Error' },
                        "no-answered": { text: 'Không trả lời', status: 'Error' }
                    }
                },
                {
                    title: 'Chiều gọi',
                    dataIndex: 'direction',
                    valueEnum: {
                        "inbound": { text: 'Gọi đến', status: 'Processing' },
                        "outbound": { text: 'Gọi đi', status: 'Success' },
                    }
                },
                {
                    title: 'Telesales',
                    dataIndex: 'telesaleName',
                    search: false,
                    render: (dom, record) => {
                        if (!record.telesaleName) return <i>Chưa gán</i>;
                        if (record.telesaleGender === false) {
                            return <><ManOutlined className="text-blue-500"/> {record.telesaleName}</>
                        }
                        return <><WomanOutlined className="text-pink-500"/> {record.telesaleName}</>
                    }
                },
                {
                    title: 'Ghi âm',
                    dataIndex: 'recording_url',
                    search: false,
                    render: (dom, record) => {
                        if (!record.recording_url) return null;
                        return <audio controls>
                            <source src={record.recording_url} type="audio/mpeg" />
                            Your browser does not support the audio element.
                        </audio>
                    }
                }
            ]}
            scroll={{
                x: true
            }}
            />
        </PageContainer>
    )
}

export default Index;