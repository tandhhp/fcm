import { apiBranchOptions } from "@/services/settings/branch";
import { apiUserDetail } from "@/services/user";
import { ManOutlined, UserOutlined, WomanOutlined } from "@ant-design/icons";
import { PageContainer, ProCard, ProDescriptions } from "@ant-design/pro-components";
import { useRequest } from "@umijs/max";
import { useParams, history } from "@umijs/max";
import { Alert, Avatar, Col, Divider, Empty, Image, Row, Spin, Tag, Typography } from "antd";
import dayjs from "dayjs";

const Index: React.FC = () => {
    const { id } = useParams<{ id: string }>();

    const { data, loading, error } = useRequest(() => apiUserDetail(id), {
        ready: !!id,
        refreshDeps: [id],
    });

    const { data: branchOptions } = useRequest(apiBranchOptions);

    const genderText = (gender?: boolean | null) => {
        if (gender === false) return "Nam";
        if (gender === true) return "Nữ";
        return "-";
    };

    const branchName = branchOptions?.find((item: any) => String(item.value) === String(data?.branchId))?.label || "-";

    const birthday = data?.dateOfBirth ? dayjs(data.dateOfBirth).format("DD/MM/YYYY") : "-";

    const createdDate = data?.createdDate || data?.createdAt;
    const createdDateText = createdDate ? dayjs(createdDate).format("DD/MM/YYYY HH:mm") : "-";

    const statusTag = () => {
        if (data?.status === 0) return <Tag color="success">Đang làm việc</Tag>;
        if (data?.status === 1) return <Tag color="error">Đã nghỉ việc</Tag>;
        return <Tag>Không xác định</Tag>;
    };

    return (
        <PageContainer
        onBack={() => history.back()}
        title={data?.name || "Thông tin tài khoản"}>
            {!id && <Alert type="warning" message="Không tìm thấy mã tài khoản trên URL." showIcon />}

            {id && loading && (
                <div className="py-10 text-center">
                    <Spin size="large" />
                </div>
            )}

            {id && error && !loading && (
                <Alert type="error" message="Không thể tải thông tin tài khoản." description="Vui lòng thử lại sau." showIcon />
            )}

            {id && !loading && !error && !data && (
                <Empty description="Không có dữ liệu tài khoản" />
            )}

            {id && !loading && !error && data && (
                <Row gutter={16}>
                    <Col md={8} xs={24}>
                        <ProCard title="Tổng quan" headerBordered>
                            <div className="flex items-center justify-center flex-col">
                                <div className="mb-4">
                                    {data?.avatar ? (
                                        <Image src={data.avatar} width={200} height={200} alt="Avatar" className="rounded-full object-cover" />
                                    ) : (
                                        <Avatar icon={<UserOutlined className="text-6xl" />} className="w-[200px] h-[200px] flex items-center justify-center" />
                                    )}
                                </div>
                                <Typography.Title level={4} className="!mb-1 text-center">
                                    {data?.name || "-"}
                                </Typography.Title>
                                <Typography.Text type="secondary">{data?.userName || "-"}</Typography.Text>
                                <div className="mt-3">{statusTag()}</div>
                                <Divider />
                                <div>Quyền: {data?.roles?.map((role: string) => role).join(", ") || "-"}</div>
                            </div>
                        </ProCard>
                    </Col>

                    <Col md={16} xs={24}>
                        <ProCard title="Thông tin chi tiết" headerBordered>
                            <ProDescriptions column={2} bordered size="small">
                                <ProDescriptions.Item label="Mã tài khoản">{data?.id || "-"}</ProDescriptions.Item>
                                <ProDescriptions.Item label="Ngày tạo">{createdDateText}</ProDescriptions.Item>
                                <ProDescriptions.Item label="Email">{data?.email || "-"}</ProDescriptions.Item>
                                <ProDescriptions.Item label="Số điện thoại">{data?.phoneNumber || "-"}</ProDescriptions.Item>
                                <ProDescriptions.Item label="Giới tính">
                                    {data?.gender === false && <ManOutlined className="text-blue-500 mr-1" />}
                                    {data?.gender === true && <WomanOutlined className="text-pink-500 mr-1" />}
                                    {genderText(data?.gender)}
                                </ProDescriptions.Item>
                                <ProDescriptions.Item label="Ngày sinh">{birthday}</ProDescriptions.Item>
                                <ProDescriptions.Item label="CCCD/CMND">{data?.identityNumber || "-"}</ProDescriptions.Item>
                                <ProDescriptions.Item label="Chi nhánh">{data?.branch?.name || "-"}</ProDescriptions.Item>
                                <ProDescriptions.Item label="Địa chỉ" span={2}>{data?.address || "-"}</ProDescriptions.Item>
                            </ProDescriptions>
                        </ProCard>
                    </Col>
                </Row>
            )}

        </PageContainer>
    )
}

export default Index;