import { apiAttendanceList } from "@/services/event/attendance";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import AttendanceForm from "./components/form";
import { useRef, useState } from "react";
import { EditOutlined, MoreOutlined, SettingOutlined } from "@ant-design/icons";
import { Button, Dropdown } from "antd";

const Index: React.FC = () => {

    const actionRef = useRef<ActionType>(null);

    const [selectedRow, setSelectedRow] = useState<any>(null);
    const [openForm, setOpenForm] = useState<boolean>(false);


    return (
        <PageContainer>
            <ProTable
                request={apiAttendanceList}
                rowKey="id"
                search={{
                    layout: 'vertical'
                }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Tên trạng thái',
                        dataIndex: 'name'
                    },
                    {
                        title: 'SU Rate',
                        dataIndex: 'suRate',
                        valueType: 'digit',
                        search: false,
                        width: 200
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'isActive',
                        valueEnum: {
                            true: { text: 'Kích hoạt', status: 'Success' },
                            false: { text: 'Vô hiệu hóa', status: 'Error' }
                        },
                        width: 120
                    },
                    {
                        title: 'Thứ tự',
                        dataIndex: 'sortOrder',
                        valueType: 'digit',
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 60,
                        render: (text, record) => [
                            <Dropdown key={"more"} menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        onClick: () => {
                                            setSelectedRow(record);
                                            setOpenForm(true);
                                        },
                                        icon: <EditOutlined />
                                    }
                                ]
                            }}>
                                <Button type="dashed" icon={<MoreOutlined />} size="small" />
                            </Dropdown>
                        ],
                        align: 'center'
                    }
                ]}
            />
            <AttendanceForm open={openForm} onOpenChange={setOpenForm} reload={() => actionRef.current?.reload()} data={selectedRow} />
        </PageContainer>
    )
}

export default Index;