import { getTelegram, saveTelegram, testTelegram } from '@/services/setting';
import {
  PageContainer,
  ProCard,
  ProForm,
  ProFormInstance,
  ProFormText,
  ProFormTextArea,
} from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { Col, message, Row } from 'antd';
import { useEffect, useRef } from 'react';

const Telegram: React.FC = () => {
  const { id } = useParams();
  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    getTelegram(id).then((response) => {
      if (response) {
        formRef.current?.setFields([
          {
            name: 'token',
            value: response.token,
          },
          {
            name: 'chatId',
            value: response.chatId,
          },
        ]);
      }
    });
  }, [id]);

  const onFinish = async (values: API.Telegam) => {
    const response = await saveTelegram(id, values);
    if (response.succeeded) {
      message.success('Saved!');
    }
  };

  const onTest = async (values: any) => {
    const response = await testTelegram(values);
    if (response.succeeded) {
      message.success('Sended');
    } else {
      message.error(response.errors[0].description)
    }
  };

  return (
    <PageContainer>
      <Row gutter={16}>
        <Col span={16}>
          <ProCard>
            <ProForm onFinish={onFinish} formRef={formRef}>
              <ProFormText.Password
                name="token"
                label="Token"
                tooltip="The token is a string, like 110201543:AAHdqTcvCH1vGWJxfSeofSAs0K5PALDsaw, which is required to authorize the bot and send requests to the Bot API. Keep your token secure and store it safely, it can be used by anyone to control your bot"
              />
              <ProFormText name="chatId" label="Chat ID" />
            </ProForm>
          </ProCard>
        </Col>
        <Col span={8}>
          <ProCard title="Test">
            <ProForm onFinish={onTest}>
              <ProFormTextArea
                name="message"
                label="Message"
                rules={[
                  {
                    required: true,
                  },
                ]}
              />
            </ProForm>
          </ProCard>
        </Col>
      </Row>
    </PageContainer>
  );
};

export default Telegram;
