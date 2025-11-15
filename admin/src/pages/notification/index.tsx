import React, { useRef, useState } from "react";
import { ActionType, PageContainer, ProList } from "@ant-design/pro-components";
import { List, Typography, Button, Space, Card, Row, Col, Tag, Popconfirm, message } from "antd";
import { apiNotificationDelete, apiNotificationDetail, apiNotificationMyList } from "@/services/notification";
import dayjs from "dayjs";
import { DeleteOutlined } from "@ant-design/icons";

const NotificationPage: React.FC = () => {
    const [selectedNotification, setNotifications] = useState<any>();
    const [selectedId, setSelectedId] = useState<string | null>(null);
    const actionRef = useRef<ActionType>();

    const handleSelect = async (id: string) => {
        setSelectedId(id);
        const response = await apiNotificationDetail(id);
        setNotifications(response.data);
    };

    return (
        <PageContainer>
            <Row gutter={[16, 16]}>
                <Col xs={24} md={8}>
                    <Card title="Danh sách thông báo" bordered={false} style={{ height: "100%" }}>
                        <ProList<any>
                            actionRef={actionRef}
                            rowKey="id"
                            request={apiNotificationMyList}
                            itemLayout="horizontal"
                            locale={{ emptyText: "Không có thông báo" }}
                            ghost
                            pagination={{
                                pageSize: 10,
                                showSizeChanger: false,
                                size: "small"
                            }}
                            renderItem={item => (
                                <List.Item
                                    style={{
                                        background: item.id === selectedId ? "#e6f7ff" : undefined,
                                        cursor: "pointer",
                                        borderRadius: 6,
                                        marginBottom: 8,
                                    }}
                                    onClick={() => handleSelect(item.id)}
                                    actions={[
                                        <Popconfirm title="Bạn có chắc chắn muốn xóa?" key="delete" onConfirm={async () => {
                                            await apiNotificationDelete(item.id);
                                            message.success("Xóa thông báo thành công");
                                            setSelectedId(null);
                                            actionRef.current?.reload();
                                            setNotifications(null);
                                        }}>
                                            <Button type="dashed" danger icon={<DeleteOutlined />} size="small" />
                                        </Popconfirm>
                                    ]}
                                >
                                    <List.Item.Meta
                                        title={
                                            <Space>
                                                <Typography.Text strong={!item.read}>{item.title}</Typography.Text>
                                                {!item.isRead && <Tag color="red">Mới</Tag>}
                                            </Space>
                                        }
                                        description={dayjs(item.createdAt).format("DD/MM/YYYY HH:mm:ss")}
                                    />
                                </List.Item>
                            )}
                        />
                    </Card>
                </Col>
                <Col xs={24} md={16}>
                    <Card title="Chi tiết thông báo" bordered={false} style={{ height: "100%" }}>
                        {selectedNotification ? (
                            <>
                                <Typography.Title level={4}>{selectedNotification?.title}</Typography.Title>
                                <Typography.Paragraph>{selectedNotification?.content}</Typography.Paragraph>
                                <Typography.Text type="secondary">
                                    Ngày gửi: {dayjs(selectedNotification?.createdAt).format("DD/MM/YYYY HH:mm:ss")}
                                </Typography.Text>
                            </>
                        ) : (
                            <Typography.Text>Chọn một thông báo để xem chi tiết</Typography.Text>
                        )}
                    </Card>
                </Col>
            </Row>
        </PageContainer>
    );
};

export default NotificationPage;