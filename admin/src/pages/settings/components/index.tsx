import { deleteComponent, listComponent } from '@/services/component';
import { DeleteOutlined, FolderOutlined } from '@ant-design/icons';
import {
  ActionType,
  PageContainer,
  ProColumns,
  ProTable,
} from '@ant-design/pro-components';
import { history } from '@umijs/max';
import { Button, message, Popconfirm } from 'antd';
import { useRef } from 'react';

const Block: React.FC = () => {
  const actionRef = useRef<ActionType>();

  const onConfirm = async (id?: string) => {
    const response = await deleteComponent(id);
    if (response.succeeded) {
      message.success('Deleted!');
      actionRef.current?.reload();
    } else {
      message.error(response.errors[0].description);
    }
  };

  const columns: ProColumns<API.Component>[] = [
    {
      title: '#',
      valueType: 'indexBorder',
    },
    {
      title: 'Name',
      dataIndex: 'name',
      render: (dom, entity) => (
        <a
          onClick={() =>
            history.push(`/settings/component/center/${entity.id}`)
          }
        >
          {entity.name}
        </a>
      ),
    },
    {
      title: 'Normalized name',
      dataIndex: 'normalizedName',
    },
    {
      title: 'Status',
      dataIndex: 'active',
      valueEnum: {
        false: {
          text: 'Draft',
          status: 'Default',
        },
        true: {
          text: 'Active',
          status: 'Processing',
        },
      },
    },
    {
      title: '',
      valueType: 'option',
      render: (dom, entity) => [
        <Button key="view" icon={<FolderOutlined />} size='small' onClick={() => history.push(`/settings/component/center/${entity.id}`)} />,
        <Popconfirm
          title="Are you sure?"
          key={2}
          onConfirm={() => onConfirm(entity.id)}
        >
          <Button icon={<DeleteOutlined />} type="primary" danger size='small' />
        </Popconfirm>,
      ],
    },
  ];

  return (
    <PageContainer>
      <ProTable
        rowSelection={{}}
        actionRef={actionRef}
        rowKey="id"
        request={listComponent}
        columns={columns}
        search={{
          layout: 'vertical',
        }}
        pagination={{
          defaultPageSize: 10,
          showSizeChanger: true
        }}
      />
    </PageContainer>
  );
};

export default Block;
