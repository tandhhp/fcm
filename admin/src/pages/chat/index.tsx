import React, { useState, useRef, useEffect } from 'react';
import { PageContainer, ProCard, ProForm, ProFormTextArea, ProList } from "@ant-design/pro-components";
import {
    Button,
    Avatar,
    Space,
    Typography,
    Spin,
    message,
    Tooltip,
    Empty,
    Divider,
    Alert
} from 'antd';
import {
    UserOutlined,
    RobotOutlined,
    ClearOutlined,
    CopyOutlined
} from '@ant-design/icons';
import './style.css';
import { Link, useRequest } from '@umijs/max';
import { getSetting } from '@/services/setting';
import { SETTING_NAME } from '@/utils/constants';
import { apiAmountReport } from '@/services/user';
import OpenAI from 'openai';

const { Text, Paragraph } = Typography;

interface Message {
    id: string;
    content: string | null;
    isUser: boolean;
    timestamp: Date;
}

const ChatWithAI: React.FC = () => {
    const [messages, setMessages] = useState<Message[]>([]);
    const [inputValue, setInputValue] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const messagesEndRef = useRef<HTMLDivElement>(null);
    const inputRef = useRef<any>(null);
    const { data: setting } = useRequest(() => getSetting(SETTING_NAME.CHATGPT));
    const openai = new OpenAI({
        apiKey: '',
        dangerouslyAllowBrowser: true
    });

    const { data: amountData } = useRequest(apiAmountReport);

    // Auto scroll to bottom when new messages are added
    const scrollToBottom = () => {
        messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
    };

    useEffect(() => {
        scrollToBottom();
    }, [messages]);

    // Simulate AI response (replace with actual API call)
    const getAIResponse = async (question: string): Promise<string | null> => {
        // Simulate API delay
        const completion = await openai.chat.completions.create({
            model: "gpt-4o-mini",
            messages: [
                { role: "system", content: "Bạn là trợ lý AI, chỉ trả lời dựa trên dữ liệu cung cấp." },
                { role: "user", content: `Dữ liệu:\n${JSON.stringify(amountData)}\n\nCâu hỏi: ${question}` },
            ],
        });
        return completion.choices[0].message.content;
    };

    const handleSendMessage = async () => {
        if (!inputValue.trim() || isLoading) return;

        const userMessage: Message = {
            id: Date.now().toString(),
            content: inputValue,
            isUser: true,
            timestamp: new Date(),
        };

        setMessages(prev => [...prev, userMessage]);
        setInputValue('');
        setIsLoading(true);

        try {
            const aiResponse = await getAIResponse(inputValue);

            const aiMessage: Message = {
                id: (Date.now() + 1).toString(),
                content: aiResponse,
                isUser: false,
                timestamp: new Date(),
            };

            setMessages(prev => [...prev, aiMessage]);
        } catch (error) {
            message.error('Có lỗi xảy ra khi gửi tin nhắn');
        } finally {
            setIsLoading(false);
            inputRef.current?.focus();
        }
    };

    const handleKeyPress = (e: React.KeyboardEvent) => {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            handleSendMessage();
        }
    };

    const clearChat = () => {
        setMessages([]);
        message.success('Đã xóa lịch sử chat');
    };

    const copyMessage = (content: string) => {
        navigator.clipboard.writeText(content);
        message.success('Đã sao chép tin nhắn');
    };

    const formatTime = (date: Date) => {
        return date.toLocaleTimeString('vi-VN', {
            hour: '2-digit',
            minute: '2-digit'
        });
    };

    return (
        <PageContainer
            title="Chat với AI"
            content="Trò chuyện với AI để được hỗ trợ và tư vấn"
            extra={[
                <Button
                    key="clear"
                    danger
                    icon={<ClearOutlined />}
                    onClick={clearChat}
                    disabled={messages.length === 0}
                >
                    Xóa chat
                </Button>
            ]}
        >
            <div className='md:flex gap-4'>
                <div className='flex-1'>
                    <ProCard className="chat-container" title="Trò chuyện" headerBordered>
                        {
                            !setting && (
                                <Alert
                                    message={
                                        <div>
                                            Cài đặt ChatGPT chưa được cấu hình. Vui lòng vào
                                            <Link to="/settings" className='ml-1 font-bold underline text-red-500'>Cài đặt</Link> để cấu hình ChatGPT.
                                        </div>
                                    }
                                    type="warning"
                                    showIcon
                                    className='mb-4'
                                    closable
                                />
                            )
                        }
                        <div className="chat-messages">
                            {messages.length === 0 ? (
                                <Empty
                                    description="Chưa có tin nhắn nào"
                                    image={Empty.PRESENTED_IMAGE_SIMPLE}
                                >
                                    <Text type="secondary">
                                        Bắt đầu cuộc trò chuyện bằng cách gửi tin nhắn đầu tiên
                                    </Text>
                                </Empty>
                            ) : (
                                messages.map((message: any) => (
                                    <div
                                        key={message.id}
                                        className={`message-item ${message.isUser ? 'user-message' : 'ai-message'}`}
                                    >
                                        <div className="message-avatar">
                                            <Avatar
                                                icon={message.isUser ? <UserOutlined /> : <RobotOutlined />}
                                                style={{
                                                    backgroundColor: message.isUser ? '#1890ff' : '#52c41a',
                                                }}
                                            />
                                        </div>
                                        <div className="message-content">
                                            <div className="message-header">
                                                <Text strong>
                                                    {message.isUser ? 'Bạn' : 'AI Assistant'}
                                                </Text>
                                                <Text type="secondary" className="message-time">
                                                    {formatTime(message.timestamp)}
                                                </Text>
                                            </div>
                                            <div className="message-bubble">
                                                <Paragraph className="message-text">
                                                    {message.content}
                                                </Paragraph>
                                                <Tooltip title="Sao chép">
                                                    <Button
                                                        type="text"
                                                        size="small"
                                                        icon={<CopyOutlined />}
                                                        onClick={() => copyMessage(message.content)}
                                                        className="copy-button"
                                                    />
                                                </Tooltip>
                                            </div>
                                        </div>
                                    </div>
                                ))
                            )}

                            {isLoading && (
                                <div className="message-item ai-message">
                                    <div className="message-avatar">
                                        <Avatar
                                            icon={<RobotOutlined />}
                                            style={{ backgroundColor: '#52c41a' }}
                                        />
                                    </div>
                                    <div className="message-content">
                                        <div className="message-header">
                                            <Text strong>AI Assistant</Text>
                                        </div>
                                        <div className="message-bubble">
                                            <Space>
                                                <Spin size="small" />
                                                <Text type="secondary">Đang trả lời...</Text>
                                            </Space>
                                        </div>
                                    </div>
                                </div>
                            )}

                            <div ref={messagesEndRef} />
                        </div>
                        <Divider />
                        <ProForm onFinish={handleSendMessage} loading={isLoading} disabled={isLoading}>
                            <ProFormTextArea
                                ref={inputRef}
                                fieldProps={{
                                    value: inputValue,
                                    onChange: (e) => setInputValue(e.target.value),
                                    onKeyPress: handleKeyPress,
                                    autoFocus: false
                                }}
                                placeholder="Nhập tin nhắn của bạn..."
                                disabled={isLoading}
                            />
                        </ProForm>
                    </ProCard>
                </div>
                <div className='md:w-96'>
                    <ProCard title="Danh mục" headerBordered>
                        <ProList
                            dataSource={[
                                {
                                    title: 'Doanh thu'
                                }
                            ]}
                            metas={{
                                avatar: {
                                    valueType: 'indexBorder'
                                },
                                title: {
                                    dataIndex: 'title'
                                }
                            }}
                        />
                    </ProCard>
                </div>
            </div>
        </PageContainer>
    );
};

export default ChatWithAI;