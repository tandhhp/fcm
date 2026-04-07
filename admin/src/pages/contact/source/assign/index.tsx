import { apiCallOptions } from "@/services/call";
import { apiBranchOptions } from "@/services/settings/branch";
import { apiSourceByTeamAndTypeOfData, apiSourceContactAssign, apiSourceContactList, apiSourceOptions, apiSourceTeamOptions } from "@/services/settings/source";
import { apiCallCenterOptions } from "@/services/users/call-center";
import { apiTeamOptions, apiTeamUsers } from "@/services/users/team";
import { UserOutlined } from "@ant-design/icons";
import { ActionType, PageContainer, ProCard, ProForm, ProFormInstance, ProFormSelect, ProFormText, ProTable } from "@ant-design/pro-components"
import { Button, Col, Divider, message, Popconfirm, Row } from "antd";
import { useEffect, useRef, useState } from "react";

const Index: React.FC = () => {

    const formRef = useRef<ProFormInstance>(null);
    const formAssignRef = useRef<ProFormInstance>(null);
    const actionContactRef = useRef<ActionType>(null);
    const [sourceOptions, setSourceOptions] = useState<any[]>([]);
    const [teamOptions, setTeamOptions] = useState<any[]>([]);
    const [teamId, setTeamId] = useState<number>();
    const [typeOfData, setTypeOfData] = useState<number>(1);
    const [callCenterId, setCallCenterId] = useState<number>();
    const [teamAssignOptions, setTeamAssignOptions] = useState<any[]>([]);
    const [teamIds, setTeamIds] = useState<number[]>([]);
    const [usersInTeam, setUsersInTeam] = useState<any[]>([]);
    const [sourceIds, setSourceIds] = useState<number[]>([]);
    const [callStatusId, setCallStatusId] = useState<number>();
    const [extraStatus, setExtraStatus] = useState<string>();
    const [selectedTelesalesIds, setSelectedTelesalesIds] = useState<string[]>([]);
    const [contactPerAgent, setContactPerAgent] = useState<number>(0);
    const [phoneNumber, setPhoneNumber] = useState<string>('');
    const [loadingSubmit, setLoadingSubmit] = useState<boolean>(false);

    useEffect(() => {
        const fetchUsersInTeam = async () => {
            if (teamIds.length > 0) {
                const response = await apiTeamUsers({ teamIds: teamIds.join(',') });
                setUsersInTeam(response.data);
            }
        };
        fetchUsersInTeam();
    }, [teamIds]);

    useEffect(() => {
        const fetchTeamOptions = async () => {
            const options = await apiSourceTeamOptions({ typeOfData });
            setTeamOptions(options);
        }
        fetchTeamOptions();
    }, [typeOfData]);

    useEffect(() => {
        const fetchTeamAssignOptions = async () => {
            if (callCenterId) {
                const options = await apiTeamOptions({ callCenterId });
                setTeamAssignOptions(options);
            }
        }
        fetchTeamAssignOptions();
    }, [callCenterId]);

    useEffect(() => {
        const fetchSourceOptions = async () => {
            if (teamId && typeOfData) {
                const options = await apiSourceByTeamAndTypeOfData({ teamId, typeOfData });
                setSourceOptions(options);
            }
        }
        fetchSourceOptions();
    }, [teamId, typeOfData]);

    const handleAssign = async () => {
        if (selectedTelesalesIds.length === 0) {
            message.warning('Vui lòng chọn ít nhất một nhân viên để chia dữ liệu');
            return;
        }
        if (contactPerAgent <= 0) {
            message.warning('Vui lòng nhập số lượng liên hệ cho mỗi nhân viên');
            return;
        }
        const contactCount = contactPerAgent * selectedTelesalesIds.length;
        setLoadingSubmit(true);
        await apiSourceContactAssign({
            sourceIds,
            typeOfData,
            callStatusId,
            extraStatus,
            contactCount,
            teleIds: selectedTelesalesIds
        });
        message.success('Chia dữ liệu thành công');
        setLoadingSubmit(false);
    }

    return (
        <PageContainer>
            <div className="max-w-7xl mx-auto">
                <ProCard title="Chia dữ liệu" headerBordered>
                    <Divider orientation="left">Nguồn dữ liệu</Divider>
                    <ProForm formRef={formRef} submitter={false}>
                        <Row gutter={16}>
                            <Col md={6} xs={24}>
                                <ProFormSelect name="branchId" label="Chi nhánh" request={apiBranchOptions} allowClear={false} rules={[
                                    {
                                        required: true
                                    }
                                ]} initialValue={1} />
                            </Col>
                            <Col md={6} xs={24}>
                                <ProFormSelect name="typeOfData" label="Loại dữ liệu" initialValue={1}
                                    options={[
                                        {
                                            label: 'Contact New',
                                            value: 1
                                        },
                                        {
                                            label: 'Contact Old',
                                            value: 2
                                        },
                                        {
                                            label: 'Contact for Start Case',
                                            value: 3
                                        }
                                    ]}
                                    onChange={(value: number) => {
                                        formRef.current?.setFieldValue('sourceId', null);
                                        formRef.current?.setFieldValue('teamId', null);
                                        setTypeOfData(value);
                                        actionContactRef.current?.reload();
                                    }}
                                    allowClear={false} rules={[
                                        {
                                            required: true
                                        }]} />
                            </Col>
                            <Col md={12} xs={24}>
                                <ProFormSelect
                                    onChange={(value: number) => {
                                        formRef.current?.setFieldValue('sourceIds', null);
                                        setTeamId(value);
                                    }}
                                    name="teamId" label="Group" options={teamOptions} showSearch />
                            </Col>
                            <Col span={24}>
                                <ProFormSelect name="sourceIds" label="Nguồn dữ liệu" mode="multiple"
                                    options={sourceOptions}
                                    onChange={(value: number[]) => setSourceIds(value)}
                                />
                            </Col>
                            <Col md={6} xs={24}>
                                <ProFormSelect name="callStatusId"
                                    onChange={(value: number) => {
                                        setCallStatusId(value);
                                        actionContactRef.current?.reload();
                                    }}
                                    label="Trạng thái cuộc gọi" request={apiCallOptions} showSearch />
                            </Col>
                            <Col xs={24} md={6}>
                                <ProFormSelect name={`extraStatus`} label="Extra Status" options={[
                                    {
                                        label: 'Có tiền',
                                        value: 'Có tiền'
                                    },
                                    {
                                        label: 'Có thói quen',
                                        value: 'Có thói quen'
                                    },
                                    {
                                        label: 'Có cả 2',
                                        value: 'Có cả 2'
                                    }
                                ]}
                                    onChange={(value: string) => {
                                        setExtraStatus(value);
                                        actionContactRef.current?.reload();
                                    }}
                                />
                            </Col>
                            <Col md={6} xs={24}>
                                <ProFormText name="phoneNumber" 
                                tooltip="Nhấn Enter để tìm kiếm"
                                label="Số điện thoại" fieldProps={{
                                    onPressEnter: (e) => {
                                        setPhoneNumber(e.currentTarget.value);
                                        actionContactRef.current?.reload();
                                    }
                                }} />
                            </Col>
                        </Row>
                    </ProForm>
                    <ProTable
                        search={false}
                        headerTitle="Danh sách liên hệ"
                        ghost
                        rowKey="id"
                        size="small"
                        request={apiSourceContactList}
                        params={{
                            typeOfData: typeOfData,
                            teamId: teamId,
                            sourceIds: sourceIds.join(','),
                            callStatusId: callStatusId,
                            extraStatus: extraStatus,
                            phoneNumber: phoneNumber
                        }}
                        pagination={{
                            pageSize: 10
                        }}
                        actionRef={actionContactRef}
                        columns={[
                            {
                                title: '#',
                                valueType: 'indexBorder',
                                width: 30,
                                align: 'center'
                            },
                            {
                                title: 'Họ tên',
                                dataIndex: 'name'
                            },
                            {
                                title: 'Số điện thoại',
                                dataIndex: 'phoneNumber'
                            },
                            {
                                title: 'Ngày tạo',
                                dataIndex: 'createdDate',
                                valueType: 'date',
                            },
                            {
                                title: 'Souce Name',
                                dataIndex: 'sourceId',
                                valueType: 'select',
                                request: apiSourceOptions,
                            },
                            {
                                title: 'Group',
                                dataIndex: 'teamId',
                                valueType: 'select',
                                fieldProps: {
                                    options: teamOptions
                                },
                            },
                            {
                                title: 'Telesales',
                                dataIndex: 'teleName'
                            },
                            {
                                title: 'Trạng thái cuộc gọi',
                                dataIndex: 'callStatusId',
                                valueType: 'select',
                                request: apiCallOptions,
                            }
                        ]}
                    />
                    <Divider orientation="left">Chia dữ liệu</Divider>
                    <ProForm submitter={false} formRef={formAssignRef}>
                        <Row gutter={16}>
                            <Col md={4} xs={24}>
                                <ProFormSelect name="callCenterId" label="Call Center"
                                    onChange={(value: number) => {
                                        formAssignRef.current?.setFieldValue('teamIds', null);
                                        setCallCenterId(value);
                                    }}
                                    request={apiCallCenterOptions} />
                            </Col>
                            <Col md={20} xs={24}>
                                <ProFormSelect name="teamIds"
                                    onChange={(value: any) => {
                                        setTeamIds(value);
                                    }}
                                    label="Nhóm" mode="multiple" options={teamAssignOptions} showSearch />
                            </Col>
                        </Row>
                    </ProForm>
                    <ProTable
                        headerTitle="Agents" rowKey="id" search={false}
                        ghost
                        pagination={false}
                        className="mb-4"
                        rowSelection={{
                            onChange(selectedRowKeys) {
                                setSelectedTelesalesIds(selectedRowKeys as string[]);
                            },
                        }}
                        size="small"
                        columns={[
                            {
                                title: <UserOutlined />,
                                dataIndex: 'avatar',
                                valueType: 'avatar',
                                width: 32
                            },
                            {
                                title: 'Tài khoản',
                                dataIndex: 'userName'
                            },
                            {
                                title: 'Họ và tên',
                                dataIndex: 'name'
                            },
                            {
                                title: 'Line code',
                                dataIndex: 'lineCode'
                            },
                            {
                                title: 'Team',
                                dataIndex: 'teamId',
                                valueType: 'select',
                                fieldProps: {
                                    options: teamAssignOptions
                                }
                            },
                            {
                                title: 'SDT',
                                dataIndex: 'phoneNumber'
                            },
                            {
                                title: 'email',
                                dataIndex: 'email'
                            }
                        ]}
                        dataSource={usersInTeam} />
                    <div className="mb-4">
                        <div className="grid md:grid-cols-3 gap-4">
                            <div className="border p-2 rounded bg-slate-100">
                                SL Agent: <b>{selectedTelesalesIds.length}</b>
                            </div>
                            <div className="border rounded flex items-center gap-2 pl-2">
                                SL Liên hệ/Agent:
                                <input type="number" className="border-l flex-1 p-2"
                                    onChange={(e) => setContactPerAgent(Number(e.target.value))}
                                />
                            </div>
                            <div className="border p-2 rounded bg-slate-100">
                                SL Liên hệ: <b>{contactPerAgent * selectedTelesalesIds.length}</b>
                            </div>
                        </div>
                    </div>
                    <div>
                        <Popconfirm title="Bạn có chắc chắn muốn chia dữ liệu cho các nhân viên này không?" onConfirm={handleAssign}>
                            <Button type="primary" block loading={loadingSubmit}>
                                Chia dữ liệu
                            </Button>
                        </Popconfirm>
                    </div>
                </ProCard>
            </div>
        </PageContainer>
    )
}

export default Index;