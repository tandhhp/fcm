import WorkSummary from '@/components/works/summary';
import { getArguments, saveArguments } from '@/services/work-content';
import { ActionType, DragSortTable, PageContainer, ProCard, ProColumns, ProForm, ProFormInstance, ProFormText, ProList } from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { Button, Col, Divider, Popconfirm, Row, Tooltip, message } from 'antd';
import { useEffect, useRef, useState } from 'react';
import { ArrowLeftOutlined, DeleteOutlined, EditOutlined, PlusOutlined, SaveOutlined } from '@ant-design/icons';
import ModalLink from '@/components/modals/link';
import { FormattedMessage } from '@umijs/max';
import { uuidv4 } from '@/utils/common';

const ListGroup: React.FC = () => {
  const { id } = useParams();
  const [data, setData] = useState<CPN.ListGroup>();
  const [dataTable, setDataTable] = useState<CPN.ListGroupItem[]>();
  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    getArguments(id).then((response) => {
      if (response) {
        setData(response);
        setDataTable(response.items);
        formRef.current?.setFields([
          {
            name: 'name',
            value: response.name
          }
        ]);
      }
    });
  }, [id]);

  const actionRef = useRef<ActionType>();
  const [open, setOpen] = useState<boolean>(false);

  const onConfirm = async (itemId?: string) => {
    let newData = data;
    if (newData) {
      const items = newData.items.filter(x => x.id !== itemId);
      newData.items = items;
      const response = await saveArguments(id, newData);
      if (response.succeeded) {
        message.success('Deleted!');
        setData(newData);
        actionRef.current?.reload();
      }

    }
  };

  const addLink = async (values: any) => {
    let newData = data;
    let item = {
      id: uuidv4(),
      link: {
        href: values.href,
        name: values.name,
        target: values.target,
        id: uuidv4()
      }
    }
    if (newData?.items) {
      newData?.items?.push(item)
    } else {
      newData!.items = [item];
    }
    const response = await saveArguments(id, newData);
    if (response.succeeded) {
      setData(newData);
      message.success('Added!')
      setOpen(false);
    }
  };

  const onFinish = async (values: any) => {
    let newData: CPN.ListGroup = {
      name: '',
      items: []
    };
    if (data) {
      newData = data;
    }
    newData.name = values.name;
    const response = await saveArguments(id, newData);
    if (response.succeeded) {
      setData(newData);
      message.success('Saved!')
    }
  }

  const columns: ProColumns<CPN.ListGroupItem>[] = [
    {
      title: '#',
      dataIndex: 'sort',
      className: 'drag-visible'
    },
    {
      title: 'Name',
      render: (dom, entity) => entity.link?.name
    },
    {
      title: 'Url',
      render: (dom, entity) => entity.link?.href
    },
    {
      title: 'Action',
      valueType: 'option',
      render: (dom, entity) => [
        <Button
          key={1}
          type="primary"
          icon={<EditOutlined />}
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
  ];

  const handleDragSortEnd = (newDataSource: CPN.ListGroupItem[]) => {
    const newData = data;
    if (newData) {
      newData.items = newDataSource;
    }
    setData(newData);
    setDataTable(newDataSource)
    message.success('Saved!');
  };

  return (
    <PageContainer title={data?.name} extra={<Button icon={<ArrowLeftOutlined />} onClick={() => history.back()}><span><FormattedMessage id="general.back" /></span></Button>}>
      <Row gutter={16}>
        <Col span={16}>
          <ProCard>
            <ProForm formRef={formRef} onFinish={onFinish}>
              <ProFormText name="name" label="Name" rules={[
                {
                  required: true
                }
              ]} />

              <DragSortTable<CPN.ListGroupItem>
                toolBarRender={() => {
                  return [
                    <Tooltip key="new" title={<FormattedMessage id="general.new" />}>
                      <Button
                        type="link"
                        icon={<PlusOutlined />}
                        onClick={() => setOpen(true)}
                      >
                      </Button>
                    </Tooltip>
                  ];
                }}
                rowKey="id"
                ghost
                columns={columns}
                dataSource={dataTable}
                pagination={false}
                search={false}
                dragSortKey="sort"
                onDragSortEnd={handleDragSortEnd}
              />
              <Divider dashed />
            </ProForm>
          </ProCard>

          <ModalLink open={open} onOpenChange={setOpen} onFinish={addLink} />
        </Col>
        <Col span={8}>
          <WorkSummary />
        </Col>
      </Row>
    </PageContainer>
  );
};

export default ListGroup;
