import { apiMyTeam, apiUpdateUserInfo, apiUserDetail } from '@/services/user';
import { EditOutlined, ManOutlined, UserOutlined, WomanOutlined } from '@ant-design/icons';
import { ModalForm, PageContainer, ProCard, ProDescriptions, ProFormInstance, ProFormText, ProFormTextArea, ProTable } from '@ant-design/pro-components';
import { history, useModel, useRequest } from '@umijs/max';
import {
  Image,
  Button,
  Col,
  Row,
  Divider,
  Typography,
  message,
  Avatar,
} from 'antd';
import { useRef, useState } from 'react';
import { apiBranchOptions } from '@/services/settings/branch';

const Profile: React.FC = () => {
  const [openInfo, setOpenInfo] = useState<boolean>(false);
  const formInfoRef = useRef<ProFormInstance>();
  const { initialState } = useModel('@@initialState');
  const { data, refresh } = useRequest(() => apiUserDetail(initialState?.currentUser?.id));

  const gender = () => {
    if (data?.gender === true) {
      return 'Nam'
    } else if (data?.gender === false) {
      return 'Nữ';
    }
    return '-';
  }

  return (
    <PageContainer extra={<Button type='primary' icon={<EditOutlined />} onClick={() => history.push(`/user/center/${initialState?.currentUser?.id}`)}>Chỉnh sửa</Button>}>
      <Row gutter={16}>
        <Col md={8} xxl={6}>
          <ProCard
            title="Thông tin cá nhân"
            headerBordered
          >
            <div className="flex items-center justify-center flex-col">
              <div className='mb-4'>
                {
                  data?.avatar && (
                    <Image src={data?.avatar} width={200} height={200} alt='Avatar' className='rounded-full object-cover' />
                  )
                }
                {
                  !data?.avatar && (
                    <Avatar icon={<UserOutlined className='text-6xl' />} className='w-[200px] h-[200px] flex items-center justify-center' />
                  )
                }
              </div>
              <div className='mb-2 text-center'><Typography.Title level={4}>{data?.name}</Typography.Title>
                <div className='text-gray-500'>{data?.userName}</div>
              </div>
            </div>
            <Divider />
            <ProDescriptions title="Thông tin chi tiết" column={1} bordered size='small'>
              <ProDescriptions.Item label="Ngày tạo">
              </ProDescriptions.Item>
              <ProDescriptions.Item label="Email">{data?.email}</ProDescriptions.Item>
              <ProDescriptions.Item label="Giới tính">{gender()}</ProDescriptions.Item>
              <ProDescriptions.Item label="Điện thoại">
                {data?.phoneNumber}
              </ProDescriptions.Item>
              <ProDescriptions.Item label="Ngày sinh" valueType={'date'}>{data?.dateOfBirth}</ProDescriptions.Item>
              <ProDescriptions.Item label="DDCN">
                {data?.identityNumber}
              </ProDescriptions.Item>
              <ProDescriptions.Item label="Địa chỉ">
                {data?.address}
              </ProDescriptions.Item>
              <ProDescriptions.Item label="Chi nhánh" request={apiBranchOptions} dataIndex="branchId">
                {data?.branchId}
              </ProDescriptions.Item>
            </ProDescriptions>
          </ProCard>
        </Col>
        <Col md={16} xxl={18}>
          <ProTable
            columns={[
              {
                title: '#',
                valueType: 'indexBorder',
                width: 30,
              },
              {
                title: <UserOutlined />,
                dataIndex: 'avatar',
                valueType: 'avatar',
                width: 30,
                search: false
              },
              {
                title: 'Tài khoản',
                dataIndex: 'userName',
                search: false
              },
              {
                title: 'Họ và tên',
                dataIndex: 'name',
                valueType: 'text',
                render: (dom, entity) => (
                  <div>
                    {entity.gender === true && (<ManOutlined className='text-blue-500' />)}{entity.gender === false && (<WomanOutlined className='text-red-500' />)} {dom}
                  </div>
                )
              },
              {
                title: 'Email',
                dataIndex: 'email',
                valueType: 'text',
                search: false
              },
              {
                title: 'Số điện thoại',
                dataIndex: 'phoneNumber',
                valueType: 'text',
              },
              {
                title: 'Ngày sinh',
                dataIndex: 'dateOfBirth',
                valueType: 'date',
                width: 100,
                search: false
              },
              {
                title: 'Chi nhánh',
                dataIndex: 'branchId',
                valueType: 'select',
                width: 100,
                search: false,
                request: apiBranchOptions
              },
            ]}
            request={apiMyTeam}
            search={{
              layout: 'vertical'
            }}
            rowKey="id"
          />
        </Col>
      </Row>
      <ModalForm open={openInfo} title="Cập nhật đặc điểm" onOpenChange={setOpenInfo} formRef={formInfoRef} onFinish={async (values: any) => {
        await apiUpdateUserInfo(values);
        message.success('Cập nhật thành công!');
        refresh();
        setOpenInfo(false);
      }}>
        <ProFormText hidden name="userId" />
        <ProFormTextArea name="healthHistory" label="Tiền sử sức khỏe" />
        <ProFormTextArea name="familyCharacteristics" label="Đặc điểm gia đình" />
        <ProFormTextArea name="personality" label="Đặc điểm tính cách" />
        <ProFormTextArea name="concerns" label="Mối quan tâm" />
      </ModalForm>
    </PageContainer>
  );
};

export default Profile;
