import { apiCallHistories } from "@/services/call";
import { apiContactDetail } from "@/services/contact";
import { LeftOutlined } from "@ant-design/icons";
import { PageContainer, ProCard, ProDescriptions, ProTable } from "@ant-design/pro-components"
import { history, useParams, useRequest } from "@umijs/max";
import { Button, Col, Row } from "antd";

const Index: React.FC = () => {

    const { id } = useParams<{ id: string }>();
    const { data } = useRequest(() => apiContactDetail(id));

    return (
        <PageContainer title={data?.name} subTitle={data?.phoneNumber} extra={<Button icon={<LeftOutlined />} onClick={() => history.back()}>Quay lại</Button>}>
            <Row gutter={[16, 16]}>
                <Col xs={24} md={6}>
                    <ProCard title="Thông tin liên hệ" headerBordered>
                        <ProDescriptions column={1} bordered size="small">
                            <ProDescriptions.Item label="Họ và tên">{data?.name}</ProDescriptions.Item>
                            <ProDescriptions.Item label="Số điện thoại">{data?.phoneNumber}</ProDescriptions.Item>
                            <ProDescriptions.Item label="Email">{data?.email}</ProDescriptions.Item>
                            <ProDescriptions.Item label="Giới tính" valueEnum={{
                                true: { text: 'Nữ' },
                                false: { text: 'Nam' }
                            }}>{data?.gender}</ProDescriptions.Item>
                        </ProDescriptions>
                    </ProCard>
                </Col>
                <Col xs={24} md={18}>
                    <ProTable
                        request={apiCallHistories}
                        search={{
                            layout: 'vertical'
                        }}
                        rowKey="id"
                        columns={[
                            {
                                title: '#',
                                valueType: 'indexBorder',
                                width: 30,
                                align: 'center'
                            },
                            {
                                title: 'Ngày gọi',
                                dataIndex: 'createdDate',
                                valueType: 'dateTime',
                                search: false
                            },
                            {
                                title: 'Trạng thái',
                                dataIndex: 'callStatus',
                                search: false
                            },
                            {
                                title: 'Ngày theo dõi',
                                dataIndex: 'followUpDate',
                                valueType: 'dateTime',
                                search: false
                            },
                            {
                                title: 'Trạng thái bổ sung',
                                dataIndex: 'extraStatus',
                                search: false
                            },
                            {
                                title: 'Công việc',
                                dataIndex: 'job',
                                search: false
                            },
                            {
                                title: 'Thói quen du lịch',
                                dataIndex: 'travelHabit',
                                search: false
                            },
                            {
                                title: 'Tuổi',
                                dataIndex: 'age',
                                search: false
                            },
                            {
                                title: 'Ghi chú',
                                dataIndex: 'note',
                                search: false
                            }
                        ]}
                    />
                </Col>
            </Row>
        </PageContainer>
    )
}

export default Index;