import { getWorkSummary, updateWorkSummary } from '@/services/work-content';
import {
  ProCard,
  ProForm,
  ProFormCheckbox,
  ProFormInstance,
  ProFormText,
} from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { message } from 'antd';
import { useEffect, useRef } from 'react';

const WorkSummary: React.FC = () => {
  const { id } = useParams();
  const formRef = useRef<ProFormInstance>();
  useEffect(() => {
    getWorkSummary(id).then((response) => {
      formRef.current?.setFields([
        {
          name: 'name',
          value: response.name,
        },
        {
          name: 'active',
          value: response.active,
        },
      ]);
    });
  }, [id]);

  const onFinish = async (values: API.WorkContent) => {
    const response = await updateWorkSummary(values);
    if (response.succeeded) {
      message.success('Saved!');
    } else {
      message.error(response.errors[0].description);
    }
  };

  return (
    <ProCard title="Info">
      <ProForm formRef={formRef} onFinish={onFinish}>
        <ProFormText hidden name="id" initialValue={id} />
        <ProFormText name="name" label="Name" />
        <ProFormCheckbox name="active" label="Active" />
      </ProForm>
    </ProCard>
  );
};

export default WorkSummary;
