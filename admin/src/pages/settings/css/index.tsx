import { getCss, saveCss } from '@/services/work-content';
import {
  PageContainer,
  ProCard,
  ProForm,
  ProFormInstance,
  ProFormSelect,
  ProFormTextArea,
} from '@ant-design/pro-components';
import { Space, message } from 'antd';
import { useEffect, useRef } from 'react';
import ChangeTheme from './change-theme';

const CssSetting: React.FC = () => {
  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    getCss(undefined).then((response) => {
      formRef.current?.setFields([
        {
          name: 'arguments',
          value: response,
        },
      ]);
    });
  }, []);

  const onFinish = async (values: API.WorkItem) => {
    const response = await saveCss(values);
    if (response.succeeded) {
      message.success('Saved!');
    }
  };

  return (
    <PageContainer>
      <ProCard>
        <div className='flex mb-4 justify-end'>
        <ChangeTheme />
        </div>
        <ProForm onFinish={onFinish} formRef={formRef}>
          <ProFormTextArea name="arguments" label="Content" />
        </ProForm>
      </ProCard>
    </PageContainer>
  );
};

export default CssSetting;
