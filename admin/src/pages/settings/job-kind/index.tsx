import { apiJobKindList } from "@/services/settings/job-kind";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { useRef, useState } from "react";
import JobKindForm from "./components/form";
import { JobKind } from "@/typings/entity";
import { Button, Dropdown } from "antd";
import { EditOutlined, MoreOutlined, PlusOutlined } from "@ant-design/icons";

const Index: React.FC = () => {

    const actionRef = useRef<ActionType>();
    const [open, setOpen] = useState<boolean>(false);
    const [selectedJobKind, setSelectedJobKind] = useState<JobKind | undefined>(undefined);

    return (
        <PageContainer extra={<Button type="primary" icon={<PlusOutlined />} onClick={() => { setOpen(true); }}>Thêm loại công việc</Button>}>
            <ProTable
                actionRef={actionRef}
                request={apiJobKindList}
                rowKey="id"
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Tên',
                        dataIndex: 'name',
                    },
                    {
                        title: 'Trạng thái',
                        dataIndex: 'isActive',
                        valueType: 'select',
                        valueEnum: {
                            true: { text: 'Kích hoạt', status: 'Success' },
                            false: { text: 'Ngừng kích hoạt', status: 'Error' },
                        },
                        width: 140
                    },
                    {
                        title: 'Tác vụ',
                        valueType: 'option',
                        render: (text, record: any) => [
                            <Dropdown key={`more`} menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        icon: <EditOutlined />,
                                        onClick: () => {
                                            setSelectedJobKind(record);
                                            setOpen(true);
                                        }
                                    }
                                ]
                            }}>
                                <Button icon={<MoreOutlined />} type="dashed" size="small" />
                            </Dropdown>
                        ],
                        width: 80
                    }
                ]}
                search={{
                    layout: 'vertical'
                }}
            />
            <JobKindForm
                open={open}
                onOpenChange={setOpen} reload={() => actionRef.current?.reload()} data={selectedJobKind}
            />
        </PageContainer>
    )
}

export default Index;