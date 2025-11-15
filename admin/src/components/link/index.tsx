import {
  ProForm,
  ProFormInstance,
  ProFormItemProps,
  ProFormSelect,
  ProFormText,
} from '@ant-design/pro-components';
import { Button, message } from 'antd';
import { useEffect, useRef, useState } from 'react';

const ProFormLink: React.FC<ProFormItemProps> = (props) => {
  const formRef = ProForm.useFormInstance();
  const formChildRef = useRef<ProFormInstance>();

  const [hidden, setHidden] = useState<boolean>(true);
  const [link, setLink] = useState<CPN.Link>();

  useEffect(() => {
    const link: CPN.Link = formRef?.getFieldValue('link');
    if (link) {
      formChildRef.current?.setFields([
        {
          name: 'name',
          value: link.name
        },
        {
          name: 'href',
          value: link.href
        },
        {
          name: 'target',
          value: link.target
        }
      ])
    }
  }, []);

  const onFinish = async (values: CPN.Link) => {
    if (!props.name) {
      return message.warning('Name missing');
    }
    setLink(values);
    formRef?.setFields([
      {
        name: props.name,
        value: values,
      },
    ]);
    setHidden(true);
  };

  return (
    <ProForm.Item {...props}>
      <div
        style={{
          display: 'flex',
          gap: 16,
        }}
      >
        <div
          style={{
            height: 32,
            border: '1px dashed #d1d1d1',
            flex: 1,
            padding: '4px 11px',
            borderRadius: 4,
          }}
        >
          {link?.name}
        </div>
        <Button key="primary" type="primary" onClick={() => setHidden(!hidden)}>
          Add link
        </Button>
      </div>
      <div
        style={{
          backgroundColor: '#eee',
          padding: 16,
          borderRadius: 4,
          marginTop: 16,
        }}
        hidden={hidden}
      >
        <ProForm onFinish={onFinish} formRef={formChildRef}>
          <ProFormText name="name" label="Name" />
          <ProFormText
            name="href"
            label="URL"
            rules={[
              {
                required: true,
              },
            ]}
          />
          <ProFormSelect
            name="target"
            label="Target"
            allowClear
            options={[
              {
                value: '_blank',
                label: 'Open in new tab',
              },
            ]}
          />
        </ProForm>
      </div>
    </ProForm.Item>
  );
};

export default ProFormLink;
