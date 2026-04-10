import { apiSourceList, apiTypeOfDataOptions } from "@/services/settings/source";
import { ActionType, PageContainer, ProTable } from "@ant-design/pro-components"
import { useRef, useState } from "react";
import { EditOutlined, EyeOutlined, ImportOutlined, MoreOutlined, PlusOutlined, RetweetOutlined, SettingOutlined, UserDeleteOutlined } from "@ant-design/icons";
import { Button, Dropdown, Space } from "antd";
import SourceForm from "./components/form";
import { history, Link, useAccess } from "@umijs/max";
import ContactImport from "@/pages/contact/components/import";
import { apiTeamOptions } from "@/services/users/team";
import TransferModal from "./components/transfer-modal";

const Index: React.FC = () => {

    const access = useAccess();
    const actionRef = useRef<ActionType>(null);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [selectedRow, setSelectedRow] = useState<any>(null);
    const [openImport, setOpenImport] = useState<boolean>(false);
    const [openTransfer, setOpenTransfer] = useState<boolean>(false);

    return (
        <PageContainer extra={<Button type="primary" onClick={() => setOpenForm(true)} icon={<PlusOutlined />}>Thêm nguồn</Button>}>
            <ProTable
                actionRef={actionRef}
                rowKey={"id"}
                search={{
                    layout: 'vertical'
                }}
                headerTitle={(
                    <Space>
                        <Button icon={<ImportOutlined />} disabled={!access.canAdmin && !access.dot && !access.adminData} onClick={() => setOpenImport(true)}>Đổ dữ liệu</Button>
                        <Link to={`/contact/assign`}>
                            <Button icon={<UserDeleteOutlined />} type="primary" disabled={!access.dot && !access.canAdmin}>Chia nguồn</Button>
                        </Link>
                    </Space>
                )}
                request={apiSourceList}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30,
                        align: 'center'
                    },
                    {
                        title: 'Tên nguồn',
                        dataIndex: 'name'
                    },
                    {
                        title: 'Source',
                        dataIndex: 'sourceType',
                        valueType: 'select',
                        valueEnum: {
                            1: 'Cold Data',
                            2: 'Company',
                            3: 'Private',
                            4: 'Reference'
                        }
                    },
                    {
                        title: 'Type of Data',
                        dataIndex: 'typeOfDataId',
                        valueType: 'select',
                        request: apiTypeOfDataOptions
                    },
                    {
                        title: 'Group',
                        valueType: 'select',
                        dataIndex: 'teamId',
                        request: apiTeamOptions
                    },
                    {
                        title: 'Overwrite',
                        dataIndex: 'overwrite',
                        align: 'center',
                        search: false,
                        render: (_, entity) => {
                            return entity.overwrite ? 'Có' : 'Không';
                        }
                    },
                    {
                        title: 'Protected',
                        align: 'center',
                        search: false,
                        dataIndex: 'protected',
                        render: (_, entity) => {
                            return entity.protected ? 'Có' : 'Không';
                        }
                    },
                    {
                        title: 'Liên hệ',
                        dataIndex: 'contactCount',
                        search: false,
                        valueType: 'digit'
                    },
                    {
                        title: <SettingOutlined />,
                        valueType: 'option',
                        width: 50,
                        align: 'center',
                        render: (_, entity) => [
                            <Dropdown key={"more"} menu={{
                                items: [
                                    {
                                        key: 'view',
                                        label: 'Xem chi tiết',
                                        icon: <EyeOutlined />,
                                        onClick: () => {
                                            history.push(`/contact/source/center/${entity.id}`);
                                        }
                                    },
                                    {
                                        key: 'transfer',
                                        label: 'Chuyển nguồn',
                                        icon: <RetweetOutlined />,
                                        onClick: () => {
                                            setSelectedRow(entity);
                                            setOpenTransfer(true);
                                        }
                                    },
                                    {
                                        key: 'edit',
                                        label: 'Chỉnh sửa',
                                        onClick: () => {
                                            setSelectedRow(entity);
                                            setOpenForm(true);
                                        },
                                        icon: <EditOutlined />
                                    }
                                ]
                            }}>
                                <Button type="dashed" size="small" icon={<MoreOutlined />} />
                            </Dropdown>
                        ]
                    }
                ]}
            />
            <SourceForm open={openForm} onOpenChange={setOpenForm} data={selectedRow} reload={() => actionRef.current?.reload()} />
            <ContactImport open={openImport} onOpenChange={setOpenImport} reload={() => actionRef.current?.reload()} />
            <TransferModal 
                open={openTransfer} 
                onOpenChange={setOpenTransfer} 
                sourceId={selectedRow?.id}
                sourceName={selectedRow?.name}
                reload={() => actionRef.current?.reload()} 
            />
        </PageContainer>
    )
}

export default Index;