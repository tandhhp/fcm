import { apiEventOptions } from "@/services/event";
import { apiSourceOptions } from "@/services/settings/source";
import { apiLeadFeedback } from "@/services/user";
import { PageContainer, ProTable } from "@ant-design/pro-components"
import dayjs from "dayjs";

const FeedbackPage: React.FC = () => {
    return (
        <PageContainer>
            <ProTable
                scroll={{
                    x: true
                }}
                request={apiLeadFeedback}
                search={{
                    layout: 'vertical'
                }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'name'
                    },
                    {
                        title: 'SDT',
                        dataIndex: 'phoneNumber'
                    },
                    {
                        title: 'Năm sinh',
                        dataIndex: 'dateOfBirth',
                        search: false,
                        valueType: 'dateYear',
                        minWidth: 100,
                        width: 100,
                    },
                    {
                        title: 'Nghề nghiệp',
                        dataIndex: 'jobTitle',
                        search: false,
                    },
                    {
                        title: 'Yêu thích',
                        dataIndex: 'interestLevel',
                        search: false
                    },
                    {
                        title: 'Người T.O',
                        dataIndex: 'toName',
                        search: false
                    },
                    {
                        title: 'Lý do từ chối',
                        dataIndex: 'rejectReason',
                        search: false
                    },
                    {
                        title: 'Tài chính',
                        dataIndex: 'financialSituation',
                        search: false
                    },
                    {
                        title: 'Nguồn',
                        dataIndex: 'sourceId',
                        search: false,
                        valueType: 'select',
                        request: apiSourceOptions
                    },
                    {
                        title: 'Ngày sự kiện',
                        dataIndex: 'eventDate',
                        valueType: 'date',
                        search: false,
                        render: (_, entity) => entity.eventDate ? dayjs(entity.eventDate).format('DD-MM-YYYY') : '-',
                        width: 120
                    },
                    {
                        title: 'Khung giờ',
                        dataIndex: 'eventId',
                        search: false,
                        valueType: 'select',
                        request: apiEventOptions
                    },
                    {
                        title: 'Bàn',
                        dataIndex: 'tableName',
                        search: false
                    },
                    {
                        title: 'Checkin',
                        dataIndex: 'checkinTime',
                        search: false,
                        valueType: 'time'
                    },
                    {
                        title: 'Checkout',
                        dataIndex: 'checkoutTime',
                        search: false,
                        valueType: 'time'
                    }
                ]}
            />
        </PageContainer>
    )
}

export default FeedbackPage;