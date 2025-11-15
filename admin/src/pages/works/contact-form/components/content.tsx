import FormCatalogList from '@/components/form/catalog-list';
import { getArguments, saveArguments } from '@/services/work-content';
import {
  ProFormInstance,
  ProForm,
  ProFormText,
  ProFormSelect,
} from '@ant-design/pro-components';
import { useIntl, useParams } from '@umijs/max';
import { message } from 'antd';
import { useRef, useEffect } from 'react';

const ContactFormContent: React.FC<API.ContactForm> = (props) => {
  const intl = useIntl();

  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    if (props) {
      formRef.current?.setFields([
        {
          name: 'type',
          value:  props.type,
        },
        {
          name: 'name',
          value: props.name,
        },
        {
          name: 'finishPageId',
          value: props.finishPageId,
        }
      ]);
    }
  }, [JSON.stringify(props)]);

  const onFinish = async (values: API.ContactForm) => {
    const body: API.ContactForm = {...props};
    body.type = values.type;
    body.name = values.name;
    body.finishPageId = values.finishPageId;

    const response = await saveArguments(props.id, body);
    if (response.succeeded) {
      message.success(
        intl.formatMessage({
          id: 'general.saved',
        }),
      );
    }
  };

  return (
    <ProForm formRef={formRef} onFinish={onFinish}>
      <ProFormSelect name="type" label="Type"
        options={[
          {
            label: 'Default',
            value: 'Default'
          },
          {
            label: 'Book review',
            value: 'BookReview'
          }
        ]}
      />
      <ProFormText name="name" label="Name" />
      <FormCatalogList name="finishPageId" label="Finish page" rules={[
        {
          required: true
        }
      ]} />
    </ProForm>
  );
};

export default ContactFormContent;
