import Gallery from '@/pages/files/gallery';
import { EditOutlined } from '@ant-design/icons';
import { ProForm } from '@ant-design/pro-components';
import { Button, Empty, Image } from 'antd';
import { useEffect, useState } from 'react';

type ProFormImageProps = {
  name?: string;
  label?: string;
};

const ProFormImage: React.FC<ProFormImageProps> = (props) => {
  const formRef = ProForm.useFormInstance();
  const [open, setOpen] = useState<boolean>(false);
  const [src, setSrc] = useState<string>();

  const onFinish = (values: API.FileContent) => {
    formRef?.setFieldValue('backgroundImage', values.url);
    setSrc(values.url);
    setOpen(false);
  };

  useEffect(() => {
    new Promise((resolve) => {
      setTimeout(() => {
        setSrc(formRef?.getFieldValue(props.name || ''));
        resolve(true);
      }, 500);
    });
  }, []);

  const EmptyImage = () => (
    <div className='border p-4'>
      <Empty />
    </div>
  )

  return (
    <ProForm.Item name={props.name} label={props.label}>
      <div className='relative'>
        {
          src ? (

            <Image src={src} height={150} className='w-full border' />
          ) : <EmptyImage />
        }
        <Button icon={<EditOutlined />} onClick={() => setOpen(true)} type='text' className='absolute top-0 right-0' />
      </div>
      <Gallery open={open} onOpenChange={setOpen} onSelect={onFinish} />
    </ProForm.Item>
  );
};

export default ProFormImage;