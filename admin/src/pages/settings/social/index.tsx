import { getSocial, saveSocial } from '@/services/setting';
import {
  PageContainer,
  ProCard,
  ProForm,
  ProFormInstance,
  ProFormText,
} from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { message } from 'antd';
import { useEffect, useRef } from 'react';

const Social: React.FC = () => {
  const { id } = useParams();
  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    getSocial(id).then((response) => {
      if (response) {
        formRef.current?.setFields([
          {
            name: 'facebookUrl',
            value: response.facebookUrl,
          },
          {
            name: 'youtubeUrl',
            value: response.youtubeUrl,
          },
          {
            name: 'twitterUrl',
            value: response.twitterUrl,
          },
          {
            name: 'instagramUrl',
            value: response.instagramUrl,
          },
        ]);
      }
    });
  }, [id]);

  const onFinish = async (values: any) => {
    const response = await saveSocial(values);
    if (response.succeeded) {
      message.success('Saved');
    }
  };

  return (
    <PageContainer>
      <ProCard>
        <ProForm formRef={formRef} onFinish={onFinish}>
          <ProFormText name="id" initialValue={id} hidden />
          <ProFormText name="facebookUrl" label="Facebook URL" />
          <ProFormText name="youtubeUrl" label="Youtube URL" />
          <ProFormText name="twitterUrl" label="Twitter URL" />
          <ProFormText name="instagramUrl" label="Instagram URL" />
        </ProForm>
      </ProCard>
    </PageContainer>
  );
};

export default Social;
