import FileExplorer from '@/components/file-explorer';
import {
  getHeader,
  saveSetting,
} from '@/services/setting';
import { FolderAddOutlined, UploadOutlined } from '@ant-design/icons';
import {
  PageContainer,
  ProCard,
  ProForm,
  ProFormInstance,
  ProFormSelect,
  ProFormText,
} from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { message, Button, Space } from 'antd';
import { useEffect, useRef, useState } from 'react';

const Header: React.FC = () => {
  const { id } = useParams();
  const formRef = useRef<ProFormInstance>();
  const [open, setOpen] = useState<boolean>(false);
  const [options, setOptions] = useState<any>([]);

  useEffect(() => {
    getHeader(id).then((response: CPN.Header) => {
      setOptions(response.templates)
      formRef.current?.setFields([
        {
          name: 'viewName',
          value: response.viewName,
        },
        {
          name: 'brand',
          value: response.brand,
        },
        {
          name: 'logo',
          value: response.logo,
        }
      ]);
    });
  }, []);

  const onFinish = async (values: CPN.Header) => {
    const response = await saveSetting(id, values);
    if (response.succeeded) {
      message.success('Saved!');
    }
  };

  return (
    <PageContainer>
      <ProCard>
        <ProForm onFinish={onFinish} formRef={formRef}>
          <ProFormText name="id" initialValue={id} hidden />
          <ProFormText name="brand" label="Brand" />
          <ProFormSelect
            options={options}
            name="viewName"
            label="Template"
            rules={[
              {
                required: true
              }
            ]} />
          <ProFormText name="logo" label="Logo"
            addonAfter={
              <Space>
                <Button type='primary' icon={<FolderAddOutlined />} onClick={() => setOpen(true)}>File Explorer</Button>
                <Button icon={<UploadOutlined />}>Upload</Button>
              </Space>
            }
          />
        </ProForm>
      </ProCard>
      <FileExplorer open={open} onOpenChange={setOpen} />
    </PageContainer>
  );
};

export default Header;
