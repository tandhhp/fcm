import CatalogSetting from '@/pages/catalog/setting';
import CatalogSummary from '@/pages/catalog/summary';
import { getCatalog, listByTag } from '@/services/catalog';
import { EditOutlined, DeleteOutlined } from '@ant-design/icons';
import {
  PageContainer,
  ProCard,
  ProColumns,
  ProTable,
} from '@ant-design/pro-components';
import { useParams, history, useIntl } from '@umijs/max';
import { Button, Col, Popconfirm, Row } from 'antd';
import { useEffect, useState } from 'react';

const TagCenter: React.FC = () => {
  const { id } = useParams();
  const [catalog, setCatalog] = useState<API.Catalog>();
  const intl = useIntl();

  useEffect(() => {
    getCatalog(id).then((response) => {
      setCatalog(response);
    });
  }, [id]);

  const columns: ProColumns<API.Catalog>[] = [
    {
      title: '#',
      valueType: 'indexBorder',
    },
    {
      title: 'Name',
      dataIndex: 'name',
    },
    {
      title: 'View count',
      dataIndex: 'viewCount',
      search: false,
      valueType: 'digit',
    },
    {
      title: 'Modified date',
      dataIndex: 'modifiedDate',
      valueType: 'fromNow',
      search: false,
    },
    {
      title: 'Active',
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
        <Button
          icon={<EditOutlined />}
          key={1}
          type="primary"
          onClick={() => history.push(`/catalog/${entity.id}`)}
        ></Button>,
        <Popconfirm title="Are you sure?" key={2}>
          <Button icon={<DeleteOutlined />} type="primary" danger />
        </Popconfirm>,
      ],
    },
  ];

  return (
    <PageContainer title={catalog?.name}>
      <Row gutter={16}>
        <Col span={16}>
          <ProCard
            tabs={{
              items: [
                {
                  key: 'content',
                  label: 'Content',
                  children: <ProTable
                  ghost
                  rowSelection={{}}
                    search={{
                      layout: 'vertical'
                    }}
                    rowKey="id"
                    request={(params) => listByTag(id, params)}
                    columns={columns}
                  />
                },
                {
                  label: intl.formatMessage({
                    id: 'menu.settings',
                  }),
                  key: 'setting',
                  children: <CatalogSetting />,
                },
              ]
            }}
          />
        </Col>
        <Col span={8}>
          <CatalogSummary />
        </Col>
      </Row>
    </PageContainer>
  );
};

export default TagCenter;
