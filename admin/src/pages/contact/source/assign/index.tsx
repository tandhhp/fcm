import { apiCallOptions } from "@/services/call";
import { apiBranchOptions } from "@/services/settings/branch";
import { apiSourceByTeamAndTypeOfData, apiSourceTeamOptions } from "@/services/settings/source";
import { apiCallCenterOptions } from "@/services/users/call-center";
import { apiTeamOptions, apiTeamUsers } from "@/services/users/team";
import { UserOutlined } from "@ant-design/icons";
import { PageContainer, ProCard, ProForm, ProFormInstance, ProFormSelect, ProTable } from "@ant-design/pro-components"
import { Button, Col, Divider, Popconfirm, Row } from "antd";
import { useEffect, useRef, useState } from "react";

const Index: React.FC = () => {

    const formRef = useRef<ProFormInstance>(null);
    const formAssignRef = useRef<ProFormInstance>(null);
    const [sourceOptions, setSourceOptions] = useState<any[]>([]);
    const [teamOptions, setTeamOptions] = useState<any[]>([]);
    const [teamId, setTeamId] = useState<number>();
    const [typeOfData, setTypeOfData] = useState<number>();
    const [callCenterId, setCallCenterId] = useState<number>();
    const [teamAssignOptions, setTeamAssignOptions] = useState<any[]>([]);
    const [teamIds, setTeamIds] = useState<number[]>([]);
    const [usersInTeam, setUsersInTeam] = useState<any[]>([]);

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

    return (
        <PageContainer>
            <div className="max-w-7xl mx-auto">
                <ProCard title="Chia dữ liệu" headerBordered>
                    <Divider orientation="left">Nguồn dữ liệu</Divider>
                    <ProForm formRef={formRef} submitter={false}>
                        <Row gutter={16}>
                            <Col md={6} xs={24}>
                                <ProFormSelect name="branchId" label="Chi nhánh" request={apiBranchOptions} showSearch allowClear={false} rules={[
                                    {
                                        required: true
                                    }
                                ]} />
                            </Col>
                            <Col md={6} xs={24}>
                                <ProFormSelect name="typeOfData" label="Loại dữ liệu"
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
                                    name="teamId" label="Nhóm" options={teamOptions} showSearch />
                            </Col>
                            <Col span={24}>
                                <ProFormSelect name="sourceIds" label="Nguồn dữ liệu" mode="multiple"
                                    options={sourceOptions}
                                />
                            </Col>
                            <Col md={6} xs={24}>
                                <ProFormSelect name="callStatusId" label="Trạng thái cuộc gọi" request={apiCallOptions} showSearch />
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
                                ]} />
                            </Col>
                        </Row>
                    </ProForm>
                    <ProTable
                        search={false}
                        headerTitle="Danh sách liên hệ"
                        ghost
                        rowKey="id"
                        size="small"
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
                        headerTitle="Danh sách nhân viên" rowKey="id" search={false}
                        ghost
                        className="mb-4"
                        rowSelection={{}}
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
                                title: 'name',
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
                    <div>
                        <Popconfirm title="Bạn có chắc chắn muốn chia dữ liệu cho các nhân viên này không?" onConfirm={() => {
                        }}>
                            <Button type="primary" block>Chia dữ liệu</Button>
                        </Popconfirm>
                    </div>
                </ProCard>
            </div>
        </PageContainer>
    )
}

export default Index;