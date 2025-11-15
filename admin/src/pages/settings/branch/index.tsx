import { apiBranchList } from "@/services/settings/branch";
import { BuildOutlined, MoreOutlined, SettingOutlined } from "@ant-design/icons";
import { PageContainer, ProTable } from "@ant-design/pro-components"
import { history } from "@umijs/max";
import { Button, Dropdown } from "antd";

const Index: React.FC = () => {
    return (
        <PageContainer>
            <ProTable
                rowKey="id"
                search={{
                    layout: 'vertical'
                }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                    },
                    {
                        title: 'Chi nhánh',
                        dataIndex: 'name'
                    },
                    {
                        title: 'Xã/Phường',
                        dataIndex: 'districtName',
                        search: false
                    },
                    {
                        title: 'Phòng ban',
                        dataIndex: 'departmentCount',
                        search: false
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        render: (text, record) => [
                            <Dropdown key={`more`} menu={{
                                items: [
                                    {
                                        key: 'room',
                                        label: 'Quản lý phòng',
                                        onClick: () => {
                                            history.push(`/settings/branch/room/${record.id}`);
                                        },
                                        icon: <BuildOutlined />
                                    }
                                ]
                            }}>
                                <Button type="dashed" size="small" icon={<MoreOutlined />} />
                            </Dropdown>
                        ],
                        width: 50,
                        align: 'center'
                    }
                ]}
                request={apiBranchList}
            />
        </PageContainer>
    )
}

export default Index;