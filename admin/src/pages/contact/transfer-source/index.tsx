import {
    ActionType,
    PageContainer,
    ProForm,
    ProFormInstance,
    ProFormSelect,
    ProFormText,
    ProFormUploadDragger,
    ProTable
} from "@ant-design/pro-components";
import {
    apiContactTransferByCase,
    apiContactTransferByFile,
    apiContactTransferBySearch,
    apiContactTransferSourceList
} from "@/services/contact";
import { apiSourceOptions } from "@/services/settings/source";
import { apiTeamOptions, apiTeamUsers } from "@/services/users/team";
import { apiTelesalesOptions } from "@/services/user";
import { Button, Col, Divider, message, Row, Tabs } from "antd";
import { useEffect, useRef, useState } from "react";

type TransferFilter = {
    groupId?: number;
    sourceId?: number;
    phoneNumber?: string;
    name?: string;
    telesalesId?: string;
};

type TransferDestination = {
    groupId?: number;
    sourceId?: number;
    teamId?: number;
    telesalesId?: string;
};

const Index: React.FC = () => {
    const actionRef = useRef<ActionType>();
    const searchFormRef = useRef<ProFormInstance>();
    const destinationFormRef = useRef<ProFormInstance>();
    const fileFormRef = useRef<ProFormInstance>();

    const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
    const [searchFilter, setSearchFilter] = useState<TransferFilter>({});
    const [groupOptions, setGroupOptions] = useState<{ label: string; value: number }[]>([]);
    const [searchSourceOptions, setSearchSourceOptions] = useState<{ label: string; value: number }[]>([]);
    const [destinationSourceOptions, setDestinationSourceOptions] = useState<{ label: string; value: number }[]>([]);
    const [teamOptions, setTeamOptions] = useState<{ label: string; value: number }[]>([]);
    const [destinationTelesalesOptions, setDestinationTelesalesOptions] = useState<{ label: string; value: string }[]>([]);
    const [loadingBySearch, setLoadingBySearch] = useState<boolean>(false);
    const [loadingByCase, setLoadingByCase] = useState<boolean>(false);

    useEffect(() => {
        const fetchTeamOptions = async () => {
            const data = await apiTeamOptions({});
            setGroupOptions(data || []);
            setTeamOptions(data || []);
        };
        fetchTeamOptions();
    }, []);

    const fetchSourceOptions = async (teamId?: number, setOptions?: (options: any[]) => void) => {
        if (!teamId) {
            setOptions?.([]);
            return;
        }
        const data = await apiSourceOptions({ teamId });
        setOptions?.(data || []);
    };

    const fetchTelesalesByTeam = async (teamId?: number) => {
        if (!teamId) {
            setDestinationTelesalesOptions([]);
            return;
        }
        const response = await apiTeamUsers({ teamId });
        const options = (response?.data || []).map((item: any) => ({
            label: `${item.name} - ${item.userName}`,
            value: item.id
        }));
        setDestinationTelesalesOptions(options);
    };

    const handleTransferBySearch = async () => {
        const destination = await destinationFormRef.current?.validateFields();
        if (!destination?.sourceId) {
            message.warning("Vui lòng chọn nguồn đích");
            return;
        }
        setLoadingBySearch(true);
        try {
            const response = await apiContactTransferBySearch({
                filter: searchFilter,
                destination
            });
            if (!response?.succeeded) {
                message.error(response?.message || "Chuyển nguồn thất bại");
                return;
            }
            message.success(`Chuyển nguồn thành công ${response?.data?.transferredCount || 0} contact`);
            setSelectedRowKeys([]);
            actionRef.current?.reload();
        } finally {
            setLoadingBySearch(false);
        }
    };

    const handleTransferByCase = async () => {
        if (selectedRowKeys.length === 0) {
            message.warning("Vui lòng chọn contact cần chuyển");
            return;
        }
        const destination = await destinationFormRef.current?.validateFields();
        if (!destination?.sourceId) {
            message.warning("Vui lòng chọn nguồn đích");
            return;
        }
        setLoadingByCase(true);
        try {
            const response = await apiContactTransferByCase({
                contactIds: selectedRowKeys,
                destination
            });
            if (!response?.succeeded) {
                message.error(response?.message || "Chuyển nguồn thất bại");
                return;
            }
            message.success(`Chuyển nguồn thành công ${response?.data?.transferredCount || 0} contact`);
            setSelectedRowKeys([]);
            actionRef.current?.reload();
        } finally {
            setLoadingByCase(false);
        }
    };

    return (
        <PageContainer>
            <Tabs
                items={[
                    {
                        key: "search-transfer",
                        label: "Chuyển nguồn theo tìm kiếm",
                        children: (
                            <>
                                <ProForm
                                    formRef={searchFormRef}
                                    submitter={false}
                                >
                                    <Row gutter={16}>
                                        <Col md={8} xs={24}>
                                            <ProFormSelect
                                                name="groupId"
                                                label="Group"
                                                options={groupOptions}
                                                showSearch
                                                fieldProps={{
                                                    onChange: (value: number) => {
                                                        searchFormRef.current?.setFieldValue("sourceId", undefined);
                                                        fetchSourceOptions(value, setSearchSourceOptions);
                                                    }
                                                }}
                                            />
                                        </Col>
                                        <Col md={8} xs={24}>
                                            <ProFormSelect
                                                name="sourceId"
                                                label="Nguồn"
                                                options={searchSourceOptions}
                                                showSearch
                                            />
                                        </Col>
                                        <Col md={8} xs={24}>
                                            <ProFormSelect
                                                name="telesalesId"
                                                label="Telesales"
                                                request={apiTelesalesOptions}
                                                showSearch
                                            />
                                        </Col>
                                        <Col md={8} xs={24}>
                                            <ProFormText name="phoneNumber" label="Số điện thoại" />
                                        </Col>
                                        <Col md={8} xs={24}>
                                            <ProFormText name="name" label="Họ tên khách" />
                                        </Col>
                                        <Col md={8} xs={24}>
                                            <div className="pt-7 flex gap-2">
                                                <Button
                                                    type="primary"
                                                    onClick={async () => {
                                                        const values = (await searchFormRef.current?.validateFields()) || {};
                                                        setSearchFilter(values);
                                                        setSelectedRowKeys([]);
                                                        actionRef.current?.reload();
                                                    }}
                                                >
                                                    Tìm kiếm
                                                </Button>
                                                <Button
                                                    onClick={() => {
                                                        searchFormRef.current?.resetFields();
                                                        setSearchFilter({});
                                                        setSelectedRowKeys([]);
                                                        setSearchSourceOptions([]);
                                                        actionRef.current?.reload();
                                                    }}
                                                >
                                                    Đặt lại
                                                </Button>
                                            </div>
                                        </Col>
                                    </Row>
                                </ProForm>

                                <ProTable
                                    rowKey="id"
                                    actionRef={actionRef}
                                    search={false}
                                    rowSelection={{
                                        selectedRowKeys,
                                        onChange: setSelectedRowKeys
                                    }}
                                    request={async (params) => {
                                        const response = await apiContactTransferSourceList({
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
                                            dataIndex: "groupName",
                                            search: false
                                        },
                                        {
                                            title: "Nguồn",
                                            dataIndex: "sourceName",
                                            search: false
                                        },
                                        {
                                            title: "Telesales",
                                            dataIndex: "telesalesName",
                                            search: false
                                        },
                                        {
                                            title: "Ngày tạo",
                                            dataIndex: "createdDate",
                                            valueType: "dateTime",
                                            search: false
                                        }
                                    ]}
                                />

                                <Divider orientation="left">Đích đến contact</Divider>
                                <ProForm formRef={destinationFormRef} submitter={false}>
                                    <Row gutter={16}>
                                        <Col md={6} xs={24}>
                                            <ProFormSelect
                                                name="groupId"
                                                label="Group đích"
                                                options={groupOptions}
                                                showSearch
                                                fieldProps={{
                                                    onChange: (value: number) => {
                                                        destinationFormRef.current?.setFieldValue("sourceId", undefined);
                                                        fetchSourceOptions(value, setDestinationSourceOptions);
                                                    }
                                                }}
                                            />
                                        </Col>
                                        <Col md={6} xs={24}>
                                            <ProFormSelect
                                                name="sourceId"
                                                label="Nguồn đích"
                                                rules={[{ required: true, message: "Vui lòng chọn nguồn đích" }]}
                                                options={destinationSourceOptions}
                                                showSearch
                                            />
                                        </Col>
                                        <Col md={6} xs={24}>
                                            <ProFormSelect
                                                name="teamId"
                                                label="Team đích"
                                                options={teamOptions}
                                                showSearch
                                                fieldProps={{
                                                    onChange: (value: number) => {
                                                        destinationFormRef.current?.setFieldValue("telesalesId", undefined);
                                                        fetchTelesalesByTeam(value);
                                                    }
                                                }}
                                            />
                                        </Col>
                                        <Col md={6} xs={24}>
                                            <ProFormSelect
                                                name="telesalesId"
                                                label="Telesales đích"
                                                options={destinationTelesalesOptions}
                                                showSearch
                                            />
                                        </Col>
                                    </Row>
                                </ProForm>

                                <div className="flex gap-2">
                                    <Button type="primary" onClick={handleTransferBySearch} loading={loadingBySearch}>
                                        Transfer by search
                                    </Button>
                                    <Button onClick={handleTransferByCase} loading={loadingByCase}>
                                        Transfer by case ({selectedRowKeys.length})
                                    </Button>
                                </div>
                            </>
                        )
                    },
                    {
                        key: "file-transfer",
                        label: "Chuyển nguồn theo file",
                        children: (
                            <ProForm
                                formRef={fileFormRef}
                                layout="vertical"
                                onFinish={async (values) => {
                                    if (!values.file?.[0]?.originFileObj) {
                                        message.warning("Vui lòng chọn file Excel");
                                        return false;
                                    }
                                    const formData = new FormData();
                                    formData.append("file", values.file[0].originFileObj);
                                    if (values.groupId) formData.append("groupId", values.groupId);
                                    if (values.sourceId) formData.append("sourceId", values.sourceId);
                                    if (values.teamId) formData.append("teamId", values.teamId);
                                    if (values.telesalesId) formData.append("telesalesId", values.telesalesId);
                                    const response = await apiContactTransferByFile(formData);
                                    if (!response?.succeeded) {
                                        message.error(response?.message || "Chuyển nguồn theo file thất bại");
                                        return false;
                                    }
                                    message.success(`Chuyển nguồn thành công ${response?.data?.transferredCount || 0} contact`);
                                    fileFormRef.current?.resetFields();
                                    return true;
                                }}
                                submitter={{
                                    searchConfig: {
                                        submitText: "Transfer theo file"
                                    }
                                }}
                            >
                                <Row gutter={16}>
                                    <Col md={6} xs={24}>
                                        <ProFormSelect
                                            name="groupId"
                                            label="Group đích"
                                            options={groupOptions}
                                            showSearch
                                            fieldProps={{
                                                onChange: (value: number) => {
                                                    fileFormRef.current?.setFieldValue("sourceId", undefined);
                                                    fetchSourceOptions(value, setDestinationSourceOptions);
                                                }
                                            }}
                                        />
                                    </Col>
                                    <Col md={6} xs={24}>
                                        <ProFormSelect
                                            name="sourceId"
                                            label="Nguồn đích"
                                            rules={[{ required: true, message: "Vui lòng chọn nguồn đích" }]}
                                            options={destinationSourceOptions}
                                            showSearch
                                        />
                                    </Col>
                                    <Col md={6} xs={24}>
                                        <ProFormSelect
                                            name="teamId"
                                            label="Team đích"
                                            options={teamOptions}
                                            showSearch
                                            fieldProps={{
                                                onChange: (value: number) => {
                                                    fileFormRef.current?.setFieldValue("telesalesId", undefined);
                                                    fetchTelesalesByTeam(value);
                                                }
                                            }}
                                        />
                                    </Col>
                                    <Col md={6} xs={24}>
                                        <ProFormSelect
                                            name="telesalesId"
                                            label="Telesales đích"
                                            options={destinationTelesalesOptions}
                                            showSearch
                                        />
                                    </Col>
                                </Row>

                                <div className="mb-3 text-gray-600">File gồm 2 cột: Họ và tên khách, Số điện thoại.</div>
                                <ProFormUploadDragger
                                    name="file"
                                    label="File Excel"
                                    max={1}
                                    rules={[{ required: true, message: "Vui lòng chọn file Excel" }]}
                                    fieldProps={{
                                        accept: ".xlsx",
                                        beforeUpload: () => false
                                    }}
                                    title="Kéo thả file vào đây hoặc bấm để chọn file"
                                    description="Chỉ hỗ trợ định dạng .xlsx"
                                />
                            </ProForm>
                        )
                    }
                ]}
            />
        </PageContainer>
    );
};

export default Index;