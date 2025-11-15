import { listUser } from '@/services/user';
import { EyeOutlined } from '@ant-design/icons';
import {
  PageContainer,
  ProColumns,
  ProTable,
} from '@ant-design/pro-components';
import { FormattedMessage, history } from '@umijs/max';
import { Badge, Button, Space, Tooltip } from 'antd';
import dayjs from 'dayjs';

const EmployeePage: React.FC = () => {

  const columns: ProColumns<any>[] = [
    {
      title: '#',
      valueType: 'indexBorder',
      width: 50
    },
    {
      title: 'Tài khoản',
      dataIndex: 'userName'
    },
    {
      title: 'Họ & tên',
      dataIndex: 'name',
    },
    {
      title: 'Email',
      dataIndex: 'email',
      render: (dom, entity) => (
        <Space>
          <Badge color={entity.emailConfirmed ? 'green' : 'red'} /> {dom}
        </Space>
      )
    },
    {
      title: <FormattedMessage id='general.phoneNumber' />,
      dataIndex: 'phoneNumber',
      render: (dom, entity) => (
        <Space>
          <Badge color={entity.phoneNumberConfirmed ? 'green' : 'red'} /> {dom}
        </Space>
      )
    },
    {
      title: 'Ngày sinh',
      dataIndex: 'dateOfBirth',
      valueType: 'date',
      search: false,
      width: 100,
      render: (_, entity) => entity.dateOfBirth ? dayjs(entity.dateOfBirth).format('DD-MM-YYYY') : '-'
    },
    {
      title: 'Giới tính',
      dataIndex: 'gender',
      render: (dom, entity) => {
        if (entity.gender === true) {
          return 'Nam'
        } else if (entity.gender === false) {
          return 'Nữ';
        }
        return '-';
      },
      search: false,
      width: 80
    },
    {
      title: 'Tùy chọn',
      valueType: 'option',
      render: (dom, entity) => [
        <Tooltip key="detail" title="Xem chi tiết">
          <Button
            type="text"
            icon={<EyeOutlined />}
            size='small'
            onClick={() => {
              history.push(`/user/member/${entity.id}`);
            }}
          />
        </Tooltip>,
      ],
      width: 100
    },
  ];

  return (
    <PageContainer>
      <ProTable<API.User>
        rowKey="id"
        request={(params) => listUser({
          ...params,
          role: 'admin'
        })}
        columns={columns}
        search={{
          layout: 'vertical',
        }}
      />
    </PageContainer>
  );
};

export default EmployeePage;
