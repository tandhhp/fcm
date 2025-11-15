import { addChildWorkContent, addColumn, deleteWork, getListColumn, sortChild } from '@/services/work-content';
import {
  DeleteOutlined,
  EditOutlined,
  MoreOutlined,
  PlusOutlined,
} from '@ant-design/icons';
import {
  ActionType,
  DragSortTable,
  ModalForm,
  ProCard,
  ProColumns,
  ProFormSelect,
  ProFormText,
} from '@ant-design/pro-components';
import { FormattedMessage, useIntl, useParams, history } from '@umijs/max';
import { Button, Col, Empty, message, Popconfirm, Row, Space } from 'antd';
import { useEffect, useRef, useState } from 'react';
import AddComponent from '@/components/add-component';

const RowContent: React.FC = () => {
  const { id } = useParams();
  const intl = useIntl();
  const [data, setData] = useState<API.Column[]>([]);
  const [openAddItem, setOpenAddItem] = useState<boolean>(false);
  const [parentId, setParentId] = useState<string>('');

  const [visible, setVisible] = useState<boolean>(false);

  const fetchData = () => {
    getListColumn(id).then(response => {
      setData(response || []);
    });
  }

  useEffect(() => {
    if (id) {
      fetchData();
    }
  }, [id]);

  const onFinish = async (values: API.Column) => {
    values.rowId = id;
    const response = await addColumn(values);
    if (response.succeeded) {
      message.success('Added!');
      setVisible(false);
      fetchData();
    } else {
      message.error(response.errors[0].description);
    }
  };

  const onAddComponent = async (values: any) => {
    const body: API.WorkContent = {
      active: true,
      parentId: parentId,
      ...values,
    };
    const response = await addChildWorkContent(body);
    if (response.succeeded) {
      message.success('Added!');
      setOpenAddItem(false);
      fetchData();
    }
  };

  const onConfirm = async (id?: string) => {
    const response = await deleteWork(id);
    if (response.succeeded) {
      message.success(
        intl.formatMessage({
          id: 'general.deleted',
        }),
      );
      fetchData();
    } else {
      message.error(response.errors[0].description);
    }
  };

  const getSpan = (className: string) => {
    return Number(className.split('-')[2]) * 2;
  }

  const columns: ProColumns<API.WorkItem>[] = [
    {
      title: '#',
      dataIndex: 'sort',
      className: 'drag-visible'
    },
    {
      title: 'Name',
      dataIndex: 'name',
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
      title: 'Action',
      valueType: 'option',
      render: (dom, entity) => [
        <Button
          key={1}
          type="primary"
          icon={<EditOutlined />}
          onClick={() => {
            history.push(
              `/works/${entity.normalizedName.toLocaleLowerCase()}/${entity.id
              }`,
            );
          }}
        />,
        <Popconfirm
          title="Are you sure?"
          key={4}
          onConfirm={() => onConfirm(entity.id)}
        >
          <Button
            icon={<DeleteOutlined />}
            danger
            type="primary"
          ></Button>
        </Popconfirm>
      ]
    }
  ]

  const handleDragSortEnd = (newDataSource: API.WorkItem[]) => {
    const workIds = newDataSource.map(x => (x.id || ''));
    sortChild(workIds).then(response => {
      if (response.succeeded) {
        fetchData();
        message.success('Saved!');
      }
    })
  };

  return (
    <div>
      <div className='mb-4 flex justify-end'>
        <Button
          icon={<PlusOutlined />}
          type="primary"
          onClick={() => setVisible(true)}
        >
          <span><FormattedMessage id="general.new" /></span>
        </Button>
      </div>
      <Row gutter={16}>
        {
          data.map((col, idx) => (
            <Col key={idx} span={getSpan(col.className)}>
              <ProCard title={col.name} bordered headerBordered extra={
                <>
                  <Button type='link' icon={<PlusOutlined />} onClick={() => {
                    setParentId(col.id || '');
                    setOpenAddItem(true);
                  }} />
                  <Popconfirm
                    title="Are you sure?"
                    onConfirm={() => onConfirm(col.id)}
                  >
                    <Button type="link" danger icon={<DeleteOutlined />} />
                  </Popconfirm>
                </>
              }>
                {
                  col.items && col.items.length > 0 ? (
                    <DragSortTable<API.WorkItem>
                      ghost
                      search={false}
                      columns={columns}
                      dataSource={col.items}
                      rowKey="id"
                      dragSortKey="sort"
                      onDragSortEnd={handleDragSortEnd}
                    />
                  ) : (<Empty />)
                }
              </ProCard>
            </Col>
          ))
        }
      </Row>
      <AddComponent open={openAddItem} onOpenChange={setOpenAddItem} onFinish={onAddComponent} />
      <ModalForm open={visible} onOpenChange={setVisible} onFinish={onFinish}>
        <ProFormText
          name="name"
          label="Name"
          rules={[
            {
              required: true,
            },
          ]}
        />
        <ProFormSelect
          label="Collumn"
          options={[
            {
              label: 'col-md-1',
              value: 'col-md-1',
            },
            {
              label: 'col-md-2',
              value: 'col-md-2',
            },
            {
              label: 'col-md-3',
              value: 'col-md-3',
            },
            {
              label: 'col-md-4',
              value: 'col-md-4',
            },
            {
              label: 'col-md-5',
              value: 'col-md-5',
            },
            {
              label: 'col-md-6',
              value: 'col-md-6',
            },
            {
              label: 'col-md-7',
              value: 'col-md-7',
            },
            {
              label: 'col-md-8',
              value: 'col-md-8',
            },
            {
              label: 'col-md-9',
              value: 'col-md-9',
            },
            {
              label: 'col-md-10',
              value: 'col-md-10',
            },
            {
              label: 'col-md-11',
              value: 'col-md-11',
            },
            {
              label: 'col-md-12',
              value: 'col-md-12',
            },
          ]}
          name="className"
        />
      </ModalForm>
    </div>
  );
};

export default RowContent;
