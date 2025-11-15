import { PageContainer, ProCard, ProDescriptions } from '@ant-design/pro-components';
import { useState } from 'react';
import SecuriryCenter from './components/security';
import ProfileTab from './components/profile';
import { Button, Col, message, Row } from 'antd';
import { EditOutlined, LeftOutlined } from '@ant-design/icons';
import { apiChangeAvatar, apiUserDetail } from '@/services/user';
import { useParams, useRequest } from '@umijs/max';

const UserCenter: React.FC = () => {
  const [tab, setTab] = useState('tab1');
  const { id } = useParams();
  const { data, refresh, loading } = useRequest(() => apiUserDetail(id));

  return (
    <PageContainer extra={<Button icon={<LeftOutlined />} onClick={() => history.back()}>Quay lại</Button>}>
      <Row gutter={16}>
        <Col md={6} xs={24}>
          <ProCard title="Thông tin cá nhân" headerBordered className='h-full' loading={loading}>
            <div className="relative flex flex-col justify-center items-center mb-4">
              <img src={data?.avatar || 'https://avatar.iran.liara.run/public'} className="w-52 h-52 object-cover" />
              <label className='right-0 cursor-pointer hover:text-blue-500 absolute top-0' htmlFor="file-upload">
                <input type='file' hidden id="file-upload" onChange={async (e) => {
                  if (e.currentTarget.files && e.currentTarget.files.length > 0) {
                    const formData = new FormData();
                    formData.append('file', e.currentTarget.files[0]);
                    await apiChangeAvatar(formData);
                    message.success('Thành công!');
                    refresh();
                  }
                }} /><EditOutlined /> Đổi ảnh
              </label>
            </div>
            <div className='text-center font-semibold text-base 2xl:text-lg'>
              {data?.name || 'Chưa cập nhật'}
            </div>
            <div className='text-center text-gray-500 text-sm 2xl:text-base mb-4'>
              {data?.userName || 'Chưa cập nhật'}
            </div>
            <ProDescriptions size='small' bordered column={1}>
              <ProDescriptions.Item label="Email">
                {data?.email || 'Chưa cập nhật'}
              </ProDescriptions.Item>
              <ProDescriptions.Item label="Số điện thoại">
                {data?.phoneNumber || 'Chưa cập nhật'}
              </ProDescriptions.Item>
            </ProDescriptions>
          </ProCard>
        </Col>
        <Col md={18} xs={24}>
          <ProCard
            tabs={{
              tabPosition: 'top',
              activeKey: tab,
              items: [
                {
                  label: 'Bảo mật',
                  key: 'tab1',
                  children: <SecuriryCenter headerTitle="Bảo mật" />,
                },
                {
                  label: 'Hồ sơ cá nhân',
                  key: 'tab2',
                  children: <ProfileTab />,
                  disabled: true
                },
              ],
              onChange: (key) => {
                setTab(key);
              },
            }}
          ></ProCard>
        </Col>
      </Row>
    </PageContainer>
  );
};

export default UserCenter;
