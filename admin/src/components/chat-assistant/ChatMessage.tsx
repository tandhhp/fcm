import { Avatar, Space } from 'antd';
import { RobotOutlined, UserOutlined } from '@ant-design/icons';
import styles from './ChatMessage.less';

type MessageType = 'user' | 'assistant';

interface ChatMessageProps {
  type: MessageType;
  content: string;
  timestamp?: Date;
}

const ChatMessage: React.FC<ChatMessageProps> = ({ type, content, timestamp }) => {
  const isUser = type === 'user';

  return (
    <div className={`${styles.messageContainer} ${isUser ? styles.userMessage : styles.assistantMessage}`}>
      <Space direction="horizontal" align="flex-start" className={styles.messageContent}>
        {!isUser && (
          <Avatar
            size={32}
            icon={<RobotOutlined />}
            style={{ backgroundColor: '#1890ff' }}
          />
        )}
        <div className={styles.messageBubble}>
          <p className={styles.text}>{content}</p>
          {timestamp && (
            <span className={styles.timestamp}>
              {timestamp.toLocaleTimeString('vi-VN', {
                hour: '2-digit',
                minute: '2-digit',
              })}
            </span>
          )}
        </div>
        {isUser && (
          <Avatar
            size={32}
            icon={<UserOutlined />}
            style={{ backgroundColor: '#87d068' }}
          />
        )}
      </Space>
    </div>
  );
};

export default ChatMessage;
