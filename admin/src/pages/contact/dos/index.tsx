import { apiUserListDos } from "@/services/user";
import { SettingOutlined, UsergroupAddOutlined } from "@ant-design/icons";
import { PageContainer, ProTable } from "@ant-design/pro-components"
import { Link } from "@umijs/max";
import { Button } from "antd";

const Index: React.FC = () => {
    return (
        <PageContainer>
            <ProTable
                request={apiUserListDos}
                rowKey={"id"}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Tên',
                        dataIndex: 'name'
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 50,
                        render: (text, record) => [
                            <Link key={"tele"} to={`/contact/dos/tele/${record.id}`}>
                                <Button type="primary" size="small" icon={<UsergroupAddOutlined />}>
                                    Chi tiết
                                </Button>
                            </Link>
                        ]
                    }
                ]}
                search={false}
            />
        </PageContainer>
    )
}

export default Index;