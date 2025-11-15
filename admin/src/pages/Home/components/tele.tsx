import { apiKeyinByTelesale } from "@/services/user";
import { ProTable } from "@ant-design/pro-components";
import { useAccess } from "@umijs/max";

const TeleReport: React.FC = () => {

    const access = useAccess();

    return access.telesaleManager && (
        <>
            <ProTable
                headerTitle="Số lượng Key-In theo Tele"
                scroll={{
                    x: true
                }}
                search={false}
                request={apiKeyinByTelesale}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Tài khoản',
                        dataIndex: 'userName',
                        minWidth: 100
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'name',
                        minWidth: 150
                    },
                    {
                        title: 'SDT',
                        dataIndex: 'phoneNumber',
                        minWidth: 100
                    },
                    {
                        title: 'SL Key-In',
                        dataIndex: 'leadCount',
                        valueType: 'digit'
                    }
                ]}
            />
        </>
    )
}

export default TeleReport;