import { deleteWork, getCss, saveCss } from '@/services/work-content';
import { DeleteOutlined } from '@ant-design/icons';
import {
  PageContainer,
  ProCard,
  ProForm,
  ProFormInstance,
  ProFormText,
  ProFormTextArea,
} from '@ant-design/pro-components';
import { useIntl, useParams } from '@umijs/max';
import { Button, message, Popconfirm } from 'antd';
import { useEffect, useRef } from 'react';

const CssSetting: React.FC = () => {
  const { id } = useParams();
  const intl = useIntl();
  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    getCss(id).then((response) => {
      formRef.current?.setFields([
        {
          name: 'id',
          value: id,
        },
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

  const onConfirm = async () => {
    const response = await deleteWork(id);
    if (response.succeeded) {
      message.success(
        intl.formatMessage({
          id: 'general.deleted',
        }),
      );
      history.back();
    } else {
      message.error(response.errors[0].description);
    }
  };

  const extra = (
    <Popconfirm title="Are you sure?" onConfirm={onConfirm}>
      <Button type="primary" danger icon={<DeleteOutlined />}>
        {' '}
        Delete
      </Button>
    </Popconfirm>
  );

  return (
    <PageContainer extra={extra}>
      <ProCard>
        <ProForm onFinish={onFinish} formRef={formRef}>
          <ProFormText name="id" hidden />
          <ProFormTextArea name="arguments" label="Content" />
        </ProForm>
      </ProCard>
    </PageContainer>
  );
};

export default CssSetting;
