import { apiGetStatisticsUser, apiGetUserInfo, apiUpdateUserInfo, getUser } from '@/services/user';
import { ArrowRightOutlined, DeleteOutlined, EditOutlined, LeftOutlined, UserOutlined } from '@ant-design/icons';
import { ModalForm, PageContainer, ProCard, ProFormInstance, ProFormText, ProFormTextArea, Statistic } from '@ant-design/pro-components';
import { FormattedDate, history, useParams } from '@umijs/max';
import {
  Image,
  Button,
  Col,
  Empty,
  Row,
  Divider,
  Descriptions,
  Typography,
  Tag,
  message,
  Avatar,
  Popconfirm,
} from 'antd';
import { useEffect, useRef, useState } from 'react';
import { useAccess } from '@umijs/max';
import SubContract from './components/sub-contract';
import { apiContractDelete } from '@/services/contact';
import TransactionTab from './transaction';
import SubUser from './sub-user';
import Achievement from './achievement';

const Profile: React.FC = () => {
  const { id } = useParams();
  const [user, setUser] = useState<any>();
  const [activeKey, setActiveKey] = useState<string>('activity');
  const access = useAccess();
  const [userInfo, setUserInfo] = useState<any>();
  const [openInfo, setOpenInfo] = useState<boolean>(false);
  const formInfoRef = useRef<ProFormInstance>();
  const [statistics, setStatistics] = useState<any>();

  const fetchUserInfo = () => {
    if (id) {
      apiGetUserInfo(id).then(response => {
        setUserInfo(response);
        formInfoRef.current?.setFields([
          {
            name: 'userId',
            value: id
          },
          {
            name: 'healthHistory',
            value: response.healthHistory
          },
          {
            name: 'familyCharacteristics',
            value: response.familyCharacteristics
          },
          {
            name: 'personality',
            value: response.personality
          },
          {
            name: 'concerns',
            value: response.concerns
          }
        ])
      })
    }
  }

  useEffect(() => {
    if (id) {
      getUser(id).then((response) => {
        setUser(response);
      });
      fetchUserInfo();
      apiGetStatisticsUser(id).then(response => {
        setStatistics(response);
      })
    }
  }, [id]);

  const gender = () => {
    if (user?.gender === true) {
      return 'Nam'
    } else if (user?.gender === false) {
      return 'Nữ';
    }
    return '-';
  }

  return (
    <PageContainer extra={<Button type='primary' icon={<LeftOutlined />} onClick={() => history.back()}>Quay lại</Button>}>
      <Row gutter={16}>
        <Col md={8} xxl={6}>
          <ProCard
            title="Thông tin cá nhân"
            headerBordered
          >
            <div className="flex items-center justify-center flex-col">
              <div className='mb-4'>
                {
                  user?.avatar && (
                    <Image src={user?.avatar} width={200} height={200} alt='Avatar' className='rounded-full object-cover' />
                  )
                }
                {
                  !user?.avatar && (
                    <Avatar icon={<UserOutlined className='text-6xl' />} className='w-[200px] h-[200px] flex items-center justify-center' />
                  )
                }
              </div>
              <div className='mb-2 text-center'><Typography.Title level={4}>{user?.name} ({user?.userName})</Typography.Title>
                </div>
            </div>
            <Divider />
            <Descriptions title="Thông tin chi tiết" column={1}>
              <Descriptions.Item label="Số hợp đồng">{user?.contractCode} <SubContract /></Descriptions.Item>
              <Descriptions.Item label="Ngày hợp đồng">
                {user?.contractDate && <FormattedDate value={user?.contractDate} />}
              </Descriptions.Item>
              <Descriptions.Item label={`Ngày hết hạn điểm năm ${user?.year}`}>
                <FormattedDate value={user?.loyaltyExpiredDate} />
              </Descriptions.Item>
              <Descriptions.Item label="Email">{user?.email}</Descriptions.Item>
              <Descriptions.Item label="Giới tính">{gender()}</Descriptions.Item>
              <Descriptions.Item label="Điện thoại">
                {user?.phoneNumber}
              </Descriptions.Item>
              <Descriptions.Item label="Ngày sinh">{user?.dateOfBirth && <FormattedDate value={user?.dateOfBirth} />}</Descriptions.Item>
              <Descriptions.Item label="Hạng thẻ">
                <Tag color={user?.tierColor}>{user?.tierName}</Tag>
              </Descriptions.Item>
              <Descriptions.Item label="Trợ lý cá nhân">
                {user?.seller}
              </Descriptions.Item>
              <Descriptions.Item label="CMT/CCCD/Hộ chiếu">
                {user?.identityNumber}
              </Descriptions.Item>
              <Descriptions.Item label="Ngày cấp">
                {user?.identityDate && <FormattedDate value={user?.identityDate} />}
              </Descriptions.Item>
              <Descriptions.Item label="Nơi cấp">
                {user?.identityAddress}
              </Descriptions.Item>
              <Descriptions.Item label="Địa chỉ">
                {user?.address}
              </Descriptions.Item>
              <Descriptions.Item label="Chi nhánh">
                {user?.branch === 1 ? 'Miền Bắc' : 'Miền Nam'}
              </Descriptions.Item>
            </Descriptions>
            <Divider>Hợp đồng thứ 2</Divider>
            {
              user?.subContracts?.map((subContract: any, index: number) => (
                <div key={index}>
                  <div className='px-2 py-2 border-b border-dashed flex justify-between items-center'>
                    <div><ArrowRightOutlined /> {subContract.code} - {subContract.description}</div>
                    <Popconfirm title="Xác nhận xóa hợp đồng?" onConfirm={async () => {
                      await apiContractDelete(subContract.id);
                      message.success('Xóa hợp đồng thành công!');
                      window.location.reload();
                    }}>
                      <Button type='primary' danger size='small' icon={<DeleteOutlined />} hidden={access.telesale || access.sales}></Button>
                    </Popconfirm>
                  </div>
                </div>
              ))
            }
          </ProCard>
        </Col>
        <Col md={16} xxl={18}>
          <div className='mb-4'>
            <Row gutter={[16, 16]}>
              <Col md={8}>
                <ProCard>
                  <Statistic value={user?.maxLoyalty} layout='vertical'
                    title={`Tổng điểm NP`} />
                </ProCard>
              </Col>
              <Col md={8}>
                <ProCard>
                  <Statistic value={statistics?.currentPoint} layout='vertical' title={`Điểm NP năm thứ ${user?.year}`} />
                </ProCard>
              </Col>
              <Col md={8}>
                <ProCard>
                  <Statistic value={statistics?.totalSpent} layout='vertical' title="Điểm NP đã sử dụng" />
                </ProCard>
              </Col>
              <Col md={8}>
                <ProCard>
                  <Statistic value={user?.token} layout='vertical' title="Điểm NP thưởng" />
                </ProCard>
              </Col>
              <Col md={8}>
                <ProCard>
                  <Statistic value={statistics?.loanPoint || 0} layout='vertical' title="Điểm vay" />
                </ProCard>
              </Col>
              <Col md={8}>
                <ProCard>
                  <Statistic value={user?.amount} layout='vertical' title="Số tiền Top-Up" suffix="₫" />
                </ProCard>
              </Col>
            </Row>
          </div>
          <ProCard tabs={{
            tabPosition: 'top',
            activeKey: activeKey,
            items: [
              {
                label: 'Lịch sử giao dịch',
                key: 'activity',
                children: <TransactionTab />,
              },
              {
                label: 'Chủ thẻ phụ',
                key: 'subUser',
                children: <SubUser />,
              },
              {
                label: 'Đặc điểm',
                key: 'othor',
                children: (
                  <>
                    <Descriptions title="Phòng Trải nghiệm khách hàng" column={1}>
                      <Descriptions.Item label="Tiền sử sức khỏe">{user?.healthHistory}</Descriptions.Item>
                      <Descriptions.Item label="Đặc điểm gia đình">{user?.familyCharacteristics}</Descriptions.Item>
                      <Descriptions.Item label="Đặc điểm tính cách">
                        {user?.personality}
                      </Descriptions.Item>
                      <Descriptions.Item label="Mối quan tâm">
                        {user?.concerns}
                      </Descriptions.Item>
                    </Descriptions>
                    <Divider />

                    <Descriptions title="Chuyên viên chăm sóc khách hàng" column={1} extra={<Button type='primary' icon={<EditOutlined />} onClick={() => {
                      fetchUserInfo();
                      setOpenInfo(true);
                    }} hidden={!access.canSales}>Chỉnh sửa</Button>}>
                      <Descriptions.Item label="Tiền sử sức khỏe">{userInfo?.healthHistory}</Descriptions.Item>
                      <Descriptions.Item label="Đặc điểm gia đình">{userInfo?.familyCharacteristics}</Descriptions.Item>
                      <Descriptions.Item label="Đặc điểm tính cách">
                        {userInfo?.personality}
                      </Descriptions.Item>
                      <Descriptions.Item label="Mối quan tâm">
                        {userInfo?.concerns}
                      </Descriptions.Item>
                    </Descriptions>
                  </>
                )
              },
              {
                label: 'Thành tựu',
                key: 'arch',
                children: <Achievement />
              }
            ],
            onChange: (key) => {
              setActiveKey(key);
            },
          }}>
            <Empty />
          </ProCard>
        </Col>
      </Row>
      <ModalForm open={openInfo} title="Cập nhật đặc điểm" onOpenChange={setOpenInfo} formRef={formInfoRef} onFinish={async (values: any) => {
        await apiUpdateUserInfo(values);
        message.success('Cập nhật thành công!');
        fetchUserInfo();
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
