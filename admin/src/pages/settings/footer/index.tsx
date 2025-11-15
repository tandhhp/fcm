import { getFooter, saveSetting } from '@/services/setting';
import {
  PageContainer,
  ProCard,
  ProForm,
  ProFormInstance,
  ProFormSelect,
  ProFormText,
} from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { message } from 'antd';
import { useEffect, useRef, useState } from 'react';

const Footer: React.FC = () => {
  const { id } = useParams();
  const formRef = useRef<ProFormInstance>();
  const [options, setOptions] = useState<any>([]);

  useEffect(() => {
    getFooter(id).then((response) => {
      setOptions(response.templates)
      formRef.current?.setFields([
        {
          name: 'companyName',
          value: response.companyName,
        },
        {
          name: 'email',
          value: response.email,
        },
        {
          name: 'phoneNumber',
          value: response.phoneNumber,
        },
        {
          name: 'viewName',
          value: response.viewName,
        }
      ]);
    });
  }, []);

  const onFinish = async (values: any) => {
    const response = await saveSetting(id, values);
    if (response.succeeded) {
      message.success('Saved!');
    }
  };

  return (
    <PageContainer>
      <ProCard>
        <ProForm formRef={formRef} onFinish={onFinish}>
          <ProFormText name="id" initialValue={id} hidden />
          <ProFormText name="companyName" label="Your company" />
          <ProFormText name="email" label="Email" />
          <ProFormText name="phoneNumber" label="Phone number" />
          <ProFormSelect
            options={options}
            name="viewName"
            label="Template"
            rules={[
              {
                required: true
              }
            ]} />
        </ProForm>
      </ProCard>
    </PageContainer>
  );
};

export default Footer;
