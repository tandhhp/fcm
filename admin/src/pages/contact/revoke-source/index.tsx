import {
    ActionType,
    PageContainer,
    ProForm,
    ProFormInstance,
    ProFormSelect,
    ProTable
} from "@ant-design/pro-components";
import { apiContactRevokeSourceByCase, apiContactRevokeSourceList } from "@/services/contact";
import { apiSourceOptions } from "@/services/settings/source";
import { apiTeamOptions } from "@/services/users/team";
import { apiTelesalesOptions } from "@/services/user";
import { Button, Col, message, Modal, Row } from "antd";
import { useEffect, useRef, useState } from "react";

type RevokeFilter = {
    groupId?: number;
    teamId?: number;
    sourceId?: string | number;
    telesalesId?: string;
};

const Index: React.FC = () => {
    const actionRef = useRef<ActionType>();
    const formRef = useRef<ProFormInstance>();

    const [groupOptions, setGroupOptions] = useState<{ label: string; value: number }[]>([]);
    const [teamOptions, setTeamOptions] = useState<{ label: string; value: number }[]>([]);
    const [sourceOptions, setSourceOptions] = useState<{ label: string; value: string | number }[]>([]);
    const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
    const [searchFilter, setSearchFilter] = useState<RevokeFilter>({});
    const [loadingRevoke, setLoadingRevoke] = useState<boolean>(false);

    useEffect(() => {
        const fetchOptions = async () => {
            const options = await apiTeamOptions({});
            setGroupOptions(options || []);
            setTeamOptions(options || []);
        };
        fetchOptions();
    }, []);

    const fetchSourceOptions = async (groupId?: number) => {
        if (!groupId) {
            setSourceOptions([]);
            return;
        }
        const options = await apiSourceOptions({ teamId: groupId });
        setSourceOptions(options || []);
    };

    const handleRevoke = () => {
        if (selectedRowKeys.length === 0) {
            message.warning("Vui lòng chọn contact cần thu hồi");
            return;
        }
        Modal.confirm({
            title: "Xác nhận thu hồi nguồn",
            content: `Bạn có chắc muốn gỡ telesales khỏi ${selectedRowKeys.length} contact đã chọn?`,
            okText: "Xác nhận",
            cancelText: "Hủy",
            onOk: async () => {
                setLoadingRevoke(true);
                try {
                    const response = await apiContactRevokeSourceByCase({
                        contactIds: selectedRowKeys.map((x) => x.toString())
                    });
                    if (!response?.succeeded) {
                        message.error(response?.message || "Thu hồi nguồn thất bại");
                        return;
                    }
                    message.success(`Đã thu hồi ${response?.data?.revokedCount || 0} contact`);
                    setSelectedRowKeys([]);
                    actionRef.current?.reload();
                } finally {
                    setLoadingRevoke(false);
                }
            }
        });
    };

    return (
        <PageContainer>
            <ProForm formRef={formRef} submitter={false}>
                <Row gutter={16}>
                    <Col md={6} xs={24}>
                        <ProFormSelect
                            name="groupId"
                            label="Group"
                            options={groupOptions}
                            showSearch
                            fieldProps={{
                                onChange: (value: number) => {
                                    formRef.current?.setFieldValue("sourceId", undefined);
                                    fetchSourceOptions(value);
                                }
                            }}
                        />
                    </Col>
                    <Col md={6} xs={24}>
                        <ProFormSelect
                            name="teamId"
                            label="Team telesales"
                            options={teamOptions}
                            showSearch
                        />
                    </Col>
                    <Col md={6} xs={24}>
                        <ProFormSelect
                            name="sourceId"
                            label="Nguồn"
                            options={sourceOptions}
                            showSearch
                        />
                    </Col>
                    <Col md={6} xs={24}>
                        <ProFormSelect
                            name="telesalesId"
                            label="Telesales"
                            request={apiTelesalesOptions}
                            showSearch
                        />
                    </Col>
                    <Col md={24} xs={24}>
                        <div className="flex gap-2 pb-4">
                            <Button
                                type="primary"
                                onClick={async () => {
                                    const values = (await formRef.current?.validateFields()) || {};
                                    setSearchFilter(values);
                                    setSelectedRowKeys([]);
                                    actionRef.current?.reload();
                                }}
                            >
                                Tìm kiếm
                            </Button>
                            <Button
                                onClick={() => {
                                    formRef.current?.resetFields();
                                    setSearchFilter({});
                                    setSourceOptions([]);
                                    setSelectedRowKeys([]);
                                    actionRef.current?.reload();
                                }}
                            >
                                Đặt lại
                            </Button>
                            <Button type="primary" danger onClick={handleRevoke} loading={loadingRevoke}>
                                Thu hồi telesales ({selectedRowKeys.length})
                            </Button>
                        </div>
                    </Col>
                </Row>
            </ProForm>

            <ProTable
                rowKey="id"
                search={false}
                actionRef={actionRef}
                rowSelection={{
                    selectedRowKeys,
                    onChange: setSelectedRowKeys
                }}
                request={async (params) => {
                    const response = await apiContactRevokeSourceList({
                        ...params,
                        ...searchFilter
                    });
                    return {
                        data: response?.data || [],
                        total: response?.total || 0,
                        success: response?.succeeded ?? true
                    };
                }}
                columns={[
                    {
                        title: "#",
                        valueType: "indexBorder",
                        width: 50
                    },
                    {
                        title: "Họ tên",
                        dataIndex: "name"
                    },
                    {
                        title: "Số điện thoại",
                        dataIndex: "phoneNumber"
                    },
                    {
                        title: "Group",
                        dataIndex: "groupName"
                    },
                    {
                        title: "Team telesales",
                        dataIndex: "teamName"
                    },
                    {
                        title: "Nguồn",
                        dataIndex: "sourceName"
                    },
                    {
                        title: "Telesales",
                        dataIndex: "telesalesName"
                    },
                    {
                        title: "Ngày tạo",
                        dataIndex: "createdDate",
                        valueType: "dateTime"
                    }
                ]}
            />
        </PageContainer>
    );
};

export default Index;