import { Button, Input, Spin, Empty, Space } from 'antd';
import { SendOutlined, DeleteOutlined } from '@ant-design/icons';
import { useEffect, useRef, useState } from 'react';
import ChatMessage from './ChatMessage';
import { apiAIChatCompletion, ChatMessage as IChatMessage } from '@/services/ai';
import styles from './ChatAssistant.less';

interface ChatProps {
  onClose?: () => void;
}

const ChatAssistant: React.FC<ChatProps> = ({ onClose }) => {
  const [messages, setMessages] = useState<
    (IChatMessage & { timestamp: Date })[]
  >([]);
  const [input, setInput] = useState('');
  const [loading, setLoading] = useState(false);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  const handleSendMessage = async () => {
    if (!input.trim()) return;

    const userMessage: IChatMessage = { role: 'user', content: input };
    const newMessages = [
      ...messages,
      { ...userMessage, timestamp: new Date() },
    ];
    setMessages(newMessages);
    setInput('');
    setLoading(true);

    try {
      const response = await apiAIChatCompletion(
        newMessages.map(({ timestamp, ...msg }) => msg)
      );
      const assistantMessage: IChatMessage = {
        role: 'assistant',
        content: response.data || 'Có lỗi xảy ra, vui lòng thử lại.',
      };
      setMessages([
        ...newMessages,
        { ...assistantMessage, timestamp: new Date() },
      ]);
    } catch (error) {
      console.error('Chat error:', error);
      const errorMessage: IChatMessage = {
        role: 'assistant',
        content: 'Xin lỗi, không thể kết nối với AI. Vui lòng thử lại sau.',
      };
      setMessages([
        ...newMessages,
        { ...errorMessage, timestamp: new Date() },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const handleClearChat = () => {
    setMessages([]);
    setInput('');
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      handleSendMessage();
    }
  };

  return (
    <div className={styles.chatContainer}>
      <div className={styles.messagesArea}>
        {messages.length === 0 ? (
          <Empty
            description="Bắt đầu cuộc trò chuyện"
            style={{ marginTop: '40px' }}
          />
        ) : (
          messages.map((msg, index) => (
            <ChatMessage
              key={index}
              type={msg.role}
              content={msg.content}
              timestamp={msg.timestamp}
            />
          ))
        )}
        {loading && (
          <div className={styles.loadingContainer}>
            <Spin size="small" />
            <span className={styles.loadingText}>AI đang suy nghĩ...</span>
          </div>
        )}
        <div ref={messagesEndRef} />
      </div>

      <div className={styles.inputArea}>
        <Space.Compact style={{ width: '100%' }}>
          <Input.TextArea
            value={input}
            onChange={(e) => setInput(e.target.value)}
            onKeyPress={handleKeyPress}
            placeholder="Nhập tin nhắn của bạn... (Shift + Enter để xuống dòng)"
            rows={3}
            disabled={loading}
            className={styles.input}
            maxLength={1000}
          />
        </Space.Compact>
        <div className={styles.buttonGroup}>
          <Button
            type="primary"
            icon={<SendOutlined />}
            onClick={handleSendMessage}
            loading={loading}
            disabled={!input.trim() || loading}
            className={styles.sendButton}
          >
            Gửi
          </Button>
          <Button
            icon={<DeleteOutlined />}
            onClick={handleClearChat}
            disabled={messages.length === 0 || loading}
            className={styles.clearButton}
          >
            Xóa
          </Button>
        </div>
      </div>
    </div>
  );
};

export default ChatAssistant;
