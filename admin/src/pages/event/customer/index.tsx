import { apiLeadAllowDuplicate, apiLeadCheckinList } from "@/services/users/lead";
import { EditOutlined, EyeOutlined, ManOutlined, MoreOutlined, SettingOutlined, WomanOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { Button, Dropdown, message, Switch } from "antd";
import { useRef, useState } from "react";
import LeadDetail from "./components/detail";
import LeadForm from "@/components/form/lead-form";

const Index: React.FC = () => {

    const actionRef = useRef<ActionType>(null);
    const [openDetail, setOpenDetail] = useState<boolean>(false);
    const [selectedRow, setSelectedRow] = useState<any>(null);
    const [openLeadForm, setOpenLeadForm] = useState<boolean>(false);

    return (
        <PageContainer>
            <ProTable
                actionRef={actionRef}
                rowKey={"id"}
                request={apiLeadCheckinList}
                scroll={{
                    x: true
                }}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Khóa',
                        dataIndex: 'duplicated',
                        search: false,
                        width: 70,
                        render: (_, entity) => (
                            <Switch size="small" checked={!entity.duplicated} onClick={async () => {
                                await apiLeadAllowDuplicate(entity.id);
                                message.success('Cập nhật thành công');
                                actionRef.current?.reload();
                            }} />
                        ),
                        align: 'center'
                    },
                    {
                        title: 'Họ và tên',
                        dataIndex: 'name',
                        render: (dom, entity) => (
                            <div
                            className="text-blue-500 font-semibold hover:text-blue-700 cursor-pointer"
                            onClick={() => {
                                setSelectedRow(entity);
                                setOpenDetail(true);
                            }}
                            >{entity.gender === false && (<ManOutlined className='text-blue-500' />)}{entity.gender === true && (<WomanOutlined className='text-red-500' />)} {dom}</div>
                        ),
                        minWidth: 180,
                        width: 180
                    },
                    {
                        title: 'Điện thoại',
                        dataIndex: 'phoneNumber',
                        width: 100
                    },
                    {
                        title: 'Số CCCD',
                        dataIndex: 'identityNumber',
                        width: 100
                    },
                    {
                        title: 'Năm sinh',
                        dataIndex: 'dateOfBirth',
                        width: 100,
                        valueType: 'dateYear',
                        search: false
                    },
                    {
                        title: 'Ngày tạo',
                        dataIndex: 'createdDate',
                        valueType: 'date',
                        width: 100,
                        search: false
                    },
                    {
                        title: 'Ghi chú',
                        dataIndex: 'note',
                        search: false,
                        minWidth: 200
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 50,
                        render: (dom, entity) => (
                            <Dropdown key={"action"} menu={{
                                items: [
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        icon: <EditOutlined />,
                                        onClick: () => {
                                            setSelectedRow(entity);
                                            setOpenLeadForm(true);
                                        }
                                    },
                                    {
                                        key: 'detail',
                                        label: 'Xem chi tiết',
                                        icon: <EyeOutlined />,
                                        onClick: () => {
                                            setSelectedRow(entity);
                                            setOpenDetail(true);
                                        }
                                    }
                                ]
                            }}>
                                <Button type="dashed" icon={<MoreOutlined />} size="small" />
                            </Dropdown>
                        )
                    }
                ]}
                search={{
                    layout: 'vertical'
                }}
            />
            <LeadDetail open={openDetail} data={selectedRow} onOpenChange={setOpenDetail} />
            <LeadForm open={openLeadForm} onOpenChange={setOpenLeadForm} data={selectedRow} reload={() => actionRef.current?.reload()} />
        </PageContainer>
    )
}
export default Index;