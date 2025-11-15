import { changePassword } from '@/services/user';
import { EditOutlined } from '@ant-design/icons';
import { ModalForm, ProFormText, ProList } from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { Button, message } from 'antd';
import { useState } from 'react';

type SecuriryCenterProps = {
  headerTitle: string;
};

const SecuriryCenter: React.FC<SecuriryCenterProps> = (props) => {
  const { id } = useParams();
  const [pOpen, setPOpen] = useState<boolean>(false);
  const [eOpen, setEOpen] = useState<boolean>(false);
  const [phoneOpen, setPhoneOpen] = useState<boolean>(false);

  const dataSource = [
    {
      id: 'password',
      name: 'Mật khẩu',
    }
  ];

  const handleEdit = (key: string) => {
    switch (key) {
      case 'password':
        setPOpen(true);
        break;
      case 'email': setEOpen(true); break;
      case 'phone': setPhoneOpen(true); break;
      default:
        break;
    }
  };

  const onFinish = async (values: any) => {
    const response = await changePassword(values);
    if (response.succeeded) {
      message.success('Saved');
      setPOpen(false);
    } else {
      message.error(response.errors[0].description);
    }
  };

  return (
    <div>
      <ProList
        dataSource={dataSource}
        rowKey="id"
        headerTitle={props.headerTitle}
        metas={{
          title: {
            dataIndex: 'name',
          },
          description: {
            render: () => '***********'
          },
          actions: {
            render: (text, row) => [
              <Button
                type="link"
                key={row.id}
                onClick={() => handleEdit(row.id)}
              >
                <EditOutlined />
              </Button>,
            ],
          },
        }}
      />
      <ModalForm open={pOpen} onOpenChange={setPOpen} onFinish={onFinish} title="Đổi mật khẩu" width={600}>
        <ProFormText name="id" initialValue={id} hidden />
        <ProFormText.Password
          name="currentPassword"
          label="Mật khẩu hiện tại"
          rules={[
            {
              required: true,
            },
          ]}
        />
        <ProFormText.Password
          name="newPassword"
          label="Mật khẩu mới"
          rules={[
            {
              required: true,
            },
          ]}
        />
        <ProFormText.Password
          name="confirmPassword"
          label="Nhập lại mật khẩu"
          rules={[
            {
              required: true,
            },
          ]}
        />
      </ModalForm>
      <ModalForm open={eOpen} onOpenChange={setEOpen} title="Email">
          <ProFormText name="email" label="Email" rules={[
            {
              required: true
            }
          ]} />
      </ModalForm>
      <ModalForm open={phoneOpen} onOpenChange={setPhoneOpen} title="Phone number">
          <ProFormText name="phoneNumber" label="PhoneNumber" rules={[
            {
              required: true
            }
          ]} />
      </ModalForm>
    </div>
  );
};

export default SecuriryCenter;
