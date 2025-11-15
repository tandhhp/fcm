import { apiContactBlacklist } from "@/services/contact";
import { PageContainer, ProTable } from "@ant-design/pro-components"

const Index: React.FC = () => {
    return (
        <PageContainer>
            <ProTable 
            search={{
                layout: 'vertical'
            }}
            request={apiContactBlacklist}
            columns={[
                {
                    title: '#',
                    valueType: 'indexBorder',
                    width: 30,
                    align: 'center'
                },
                {
                    title: 'Họ và tên',
                    dataIndex: 'name'
                },
                {
                    title: 'Số điện thoại',
                    dataIndex: 'phoneNumber'
                },
                {
                    title: 'Email',
                    dataIndex: 'email'
                },
                {
                    title: 'Địa chỉ',
                    dataIndex: 'address',
                    search: false
                },
                {
                    title: 'Ngày tạo',
                    dataIndex: 'createdDate',
                    search: false,
                    valueType: 'dateTime'
                },
                {
                    title: 'Ghi chú',
                    dataIndex: 'note',
                    search: false
                }
            ]}
            />
        </PageContainer>
    )
}

export default Index;