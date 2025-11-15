import {
  deleteFileContent,
  getFileDetail,
  listWorkItemFiles,
} from '@/services/file-service';
import {
  ArrowLeftOutlined,
  DeleteOutlined,
  FolderOutlined,
} from '@ant-design/icons';
import {
  PageContainer,
  ProColumns,
  ProTable,
} from '@ant-design/pro-components';
import { FormattedMessage, history, useIntl } from '@umijs/max';
import { useParams } from '@umijs/max';
import { Col, Row, Button, Popconfirm, message, Space } from 'antd';
import { useEffect, useState } from 'react';
import FilePreview from './preview';

const FileCenter: React.FC = () => {
  const { id } = useParams();
  const intl = useIntl();
  const [fileContent, setFileContent] = useState<API.FileContent>();

  useEffect(() => {
    getFileDetail(id).then((response) => {
      setFileContent(response);
    });
  }, []);

  const columns: ProColumns<API.WorkItem>[] = [
    {
      title: '#',
      valueType: 'indexBorder',
    },
    {
      title: 'Name',
      dataIndex: 'name',
    },
    {
      title: intl.formatMessage({
        id: 'general.status',
      }),
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
      width: 120,
      render: (dom, entity) => [
        <Button
          icon={<FolderOutlined />}
          key={1}
          type="primary"
          onClick={() => {
            history.push(
              `/works/${entity.normalizedName.toLocaleLowerCase()}/${
                entity.id
              }`,
            );
          }}
        ></Button>,
        <Popconfirm title="Are you sure?" key={2}>
          <Button icon={<DeleteOutlined />} type="primary" danger />
        </Popconfirm>,
      ],
    },
  ];

  const onConfirm = async () => {
    const response = await deleteFileContent(id);
    if (response.succeeded) {
      message.success(
        intl.formatMessage({
          id: 'general.deleted',
        }),
      );
      history.back();
    } else {
      message.error(response.errors[0].description);
    }
  };

  const extra = (
    <Space>
      <Popconfirm title="Are you sure?" onConfirm={onConfirm}>
        <Button type="primary" danger icon={<DeleteOutlined />}>
          Delete
        </Button>
      </Popconfirm>
      <Button onClick={() => history.back()} icon={<ArrowLeftOutlined />}>
        <FormattedMessage id="general.back" />
      </Button>
    </Space>
  );

  return (
    <PageContainer title="Center" extra={extra}>
      <Row gutter={16}>
        <Col span={8}>
          <FilePreview file={fileContent} />
        </Col>
        <Col span={16}>
          <ProTable
            columns={columns}
            request={(params) => listWorkItemFiles(params, { id })}
            rowKey="id"
          />
        </Col>
      </Row>
    </PageContainer>
  );
};

export default FileCenter;
