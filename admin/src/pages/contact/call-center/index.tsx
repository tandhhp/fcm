import { PageContainer, ProColumns, ProTable } from "@ant-design/pro-components"
import { apiCallCenterList } from "@/services/users/call-center"

type CallCenterListItem = {
    id: number;
    code: string;
    name: string;
    teamCount: number;
}

const Index: React.FC = () => {
    const columns: ProColumns<CallCenterListItem>[] = [
        {
            title: 'STT',
            dataIndex: 'id',
            valueType: 'indexBorder',
            width: 60,
        },
        {
            title: 'Mã',
            dataIndex: 'code',
            search: false,
        },
        {
            title: 'Tên',
            dataIndex: 'name',
        },
        {
            title: 'Số lượng nhóm',
            dataIndex: 'teamCount',
            search: false,
            width: 150,
        },
    ];

    return (
        <PageContainer>
            <ProTable<CallCenterListItem>
                columns={columns}
                search={{
                    layout: 'vertical'
                }}
                rowKey="id"
                request={apiCallCenterList}
            />
        </PageContainer>
    )
}

export default Index;