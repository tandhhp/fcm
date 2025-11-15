import { apiTeamDetail, apiTeamUsers } from "@/services/users/team";
import { ManOutlined, UserOutlined, WomanOutlined } from "@ant-design/icons";
import { PageContainer, ProTable } from "@ant-design/pro-components"
import { history, useParams, useRequest } from "@umijs/max";
import { Button } from "antd";

const Index: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const { data } = useRequest(() => apiTeamDetail(id));
    return (
        <PageContainer title={data?.name} extra={<Button onClick={() => history.back()}>Quay lại</Button>}>
            <ProTable
                request={apiTeamUsers}
                rowKey={"id"}
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
                        title: <UserOutlined />,
                        dataIndex: 'avatar',
                        valueType: 'avatar',
                        width: 50,
                        search: false
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'name',
                        render: (dom, entity) => {
                            if (entity.gender === true) {
                                return <><WomanOutlined className="text-pink-500" /> {dom}</>
                            }
                            if (entity.gender === false) {
                                return <><ManOutlined className="text-blue-500" /> {dom}</>
                            }
                            return <>{dom}</>
                        }
                    },
                    {
                        title: 'Email',
                        dataIndex: 'email',
                    },
                    {
                        title: 'Số điện thoại',
                        dataIndex: 'phoneNumber',
                    }
                ]}
                params={{ teamId: id }}
            />
        </PageContainer>
    )
}

export default Index;