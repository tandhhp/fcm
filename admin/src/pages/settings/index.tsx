import { EditOutlined } from '@ant-design/icons';
import {
  DrawerForm,
  PageContainer,
  ProCard
} from '@ant-design/pro-components';
import { Button, Empty, message } from 'antd';
import { useState } from 'react';
import ChatGPTSetting from './components/chatgpt';
import { SETTING_NAME } from '@/utils/constants';
import { getSetting, saveSetting } from '@/services/setting';
import { useEffect } from 'react';

const SettingPage: React.FC = () => {

  const [open, setOpen] = useState<boolean>(false);
  const [key, setKey] = useState<string>();
  const [data, setData] = useState<any>();

  useEffect(() => {
    if (key && open) {
      // Fetch data for the selected setting
      const fetchData = async () => {
        const result = await getSetting(key);
        setData(result.data);
      };
      fetchData();
    }
  }, [key, open]);

  const SettingRender = () => {
    if (key === SETTING_NAME.CHATGPT) {
      return <ChatGPTSetting data={data} />
    }
    return <Empty />
  }

  const onFinish = async (values: any) => {
    await saveSetting({
      name: key,
      value: values
    });
    message.success('Cài đặt đã được lưu thành công!');
    return true;
  }

  return (
    <PageContainer>
      <div className='grid grid-cols-1 md:grid-cols-4 gap-4 mb-4'>
        <ProCard title="ChatGPT" headerBordered
          actions={[
            <Button key="edit" type='text' icon={<EditOutlined />} onClick={() => {
              setOpen(true);
              setKey(SETTING_NAME.CHATGPT);
            }}>
              Cài đặt
            </Button>
          ]}
        >
          <div className='flex gap-4 items-center'>
            <div className='w-16  h-16 flex-shrink-0'>
              <img src='https://upload.wikimedia.org/wikipedia/commons/thumb/e/ef/ChatGPT-Logo.svg/1024px-ChatGPT-Logo.svg.png' alt='LOGO' className='object-cover' />
            </div>
            <div className='text-gray-500 flex-1'>
              ChatGPT là một công cụ mạnh mẽ giúp bạn tạo ra các ứng dụng AI thông minh. Bạn có thể sử dụng nó để xây dựng các chatbot, trợ lý ảo và nhiều ứng dụng khác.
            </div>
          </div>
        </ProCard>
      </div>
      <DrawerForm open={open} onOpenChange={setOpen} title="Cài đặt ChatGPT" onFinish={onFinish}>
        <SettingRender />
      </DrawerForm>
    </PageContainer>
  );
};

export default SettingPage;
