import { listUpgrade, singleUpgrade, upgrade } from '@/services/backup';
import { ArrowUpOutlined } from '@ant-design/icons';
import {
  PageContainer,
  ProColumns,
  ProTable,
} from '@ant-design/pro-components';
import { useIntl } from '@umijs/max';
import { Button, message } from 'antd';

const Upgrade: React.FC = () => {
  const intl = useIntl();

  const handleUpgrade = async () => {
    const response = await upgrade();
    if (response.succeeded) {
      message.success('Upgraded!');
    }
  };

  const handleSingleUpgrade = async (url: string) => {
    const response = await singleUpgrade(url);
    if (response.succeeded) {
      message.success(
        intl.formatMessage({
          id: 'general.saved',
        }),
      );
    }
  };

  const columns: ProColumns<API.UpgradeListItem>[] = [
    {
      title: '#',
      valueType: 'indexBorder',
    },
    {
      title: 'Name',
      dataIndex: 'name',
    },
    {
      title: 'Option',
      valueType: 'option',
      render: (dom, entity) => [
        <Button
          key={0}
          icon={<ArrowUpOutlined />}
          type="primary"
          onClick={() => handleSingleUpgrade(entity.url)}
        />,
      ],
    },
  ];

  return (
    <PageContainer
      title={intl.formatMessage({
        id: 'menu.help.upgrade',
      })}
      extra={
        <Button
          icon={<ArrowUpOutlined />}
          type="primary"
          onClick={handleUpgrade}
        >
          Upgrade all
        </Button>
      }
    >
      <ProTable rowKey="name" columns={columns} request={listUpgrade} />
    </PageContainer>
  );
};

export default Upgrade;
