import { getNavbar, saveNavbar } from '@/services/work-content';
import {
  ProForm,
  ProFormInstance,
  ProFormSelect,
  ProFormText,
} from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { message } from 'antd';
import { useEffect, useRef } from 'react';

const NavbarSetting: React.FC = () => {
  const { id } = useParams();
  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    getNavbar(id).then((response) => {
      if (response) {
        formRef.current?.setFields([
          {
            name: 'layout',
            value: response.layout,
          },
        ]);
      }
    });
  }, [id]);

  const onFinish = async (values: API.Navbar) => {
    const response = await saveNavbar(values);
    if (response.succeeded) {
      message.success('Saved!');
    }
  };

  return (
    <ProForm formRef={formRef} onFinish={onFinish}>
      <ProFormText name="id" hidden initialValue={id} />
      <ProFormSelect
        label="Layout"
        name="layout"
        options={[
          {
            label: 'Default',
            value: 0,
          },
          {
            label: 'Vertical',
            value: 1,
          },
        ]}
      />
    </ProForm>
  );
};

export default NavbarSetting;
