import { apiCallStatusDetails } from "@/services/call";
import { DrawerForm, DrawerFormProps, ProTable } from "@ant-design/pro-components"

type Props = DrawerFormProps & {
    data?: any;
}

const ReportDetail: React.FC<Props> = ({ data, ...rest }) => {
    return (
        <DrawerForm submitter={false} {...rest} title="Chi tiết báo cáo" drawerProps={{
            destroyOnHidden: true
        }}>
            <ProTable
                request={apiCallStatusDetails}
                params={{
                    teleId: data?.teleId,
                    CallStatusId: data?.callStatusId
                }}
                ghost
                rowKey="id"
                pagination={false}
                search={{
                    layout: 'vertical',
                    filterType: 'light'
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
                        title: 'Ngày gọi',
                        dataIndex: 'createdDate',
                        valueType: 'dateTime',
                        search: false
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note'
                    }
                ]}
            />
        </DrawerForm>
    )
}

export default ReportDetail;