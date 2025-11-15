import {
  addLocalization,
  deleteLocalization,
  listLocalization,
  saveLocalization,
} from '@/services/localization';
import { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import {
  ActionType,
  ModalForm,
  PageContainer,
  ProColumns,
  ProFormInstance,
  ProFormText,
  ProTable,
} from '@ant-design/pro-components';
import { getLocale } from '@umijs/max';
import { FormattedMessage } from '@umijs/max';
import { Button, message, Popconfirm } from 'antd';
import { useEffect, useRef, useState } from 'react';

const Localization: React.FC = () => {
  const formRef = useRef<ProFormInstance>();
  const actionRef = useRef<ActionType>();
  const [open, setOpen] = useState(false);
  const [selectedItem, setSelectedItem] = useState<any>();

  const handleAdd = () => {
    formRef.current?.resetFields();
    setSelectedItem(null);
    setOpen(true);
  };

  useEffect(() => {
    if (selectedItem) {
      formRef.current?.setFields([
        {
          name: 'id',
          value: selectedItem.id,
        },
        {
          name: 'key',
          value: selectedItem.key,
        },
        {
          name: 'value',
          value: selectedItem.value,
        },
      ]);
    }

  }, [JSON.stringify(selectedItem)]);

  const handleUpdate = (item: any) => {
    setSelectedItem(item);
    setOpen(true);
  };

  const handleRemove = async (id: string) => {
    const response = await deleteLocalization(id);
    if (response.succeeded) {
      message.success('Deleted!');
      actionRef.current?.reload();
    }
  };

  const columns: ProColumns<any>[] = [
    {
      title: '#',
      valueType: 'indexBorder',
    },
    {
      title: 'Key',
      dataIndex: 'key',
    },
    {
      title: 'Value',
      dataIndex: 'value',
    },
    {
      title: '',
      valueType: 'option',
      render: (dom, entity) => [
        <Button
          icon={<EditOutlined />}
          size='small'
          key={1}
          type="primary"
          onClick={() => handleUpdate(entity)}
        ></Button>,
        <Popconfirm
          title="Are you sure?"
          key={2}
          onConfirm={() => handleRemove(entity.id)}
        >
          <Button icon={<DeleteOutlined />} type="primary"
            size='small' danger />
        </Popconfirm>,
      ],
      width: 80
    },
  ];

  const onFinish = async (values: any) => {
    if (!values.id) {
      const response = await addLocalization(values);
      if (response.succeeded) {
        message.success('Saved!');
      }
    } else {
      const response = await saveLocalization(values);
      if (response.succeeded) {
        message.success('Saved!');
      }
    }
    setOpen(false);
    actionRef.current?.reload();
  };

  return (
    <PageContainer
      extra={
        <Button type="primary" className='flex gap-2 items-center' icon={<PlusOutlined />} onClick={handleAdd}>
          <FormattedMessage id="general.new" />
        </Button>
      }
    >
      <ProTable
        scroll={{
          x: true
        }}
        rowSelection={{}}
        actionRef={actionRef}
        columns={columns}
        request={(params) => {
          const locale = getLocale();
          return listLocalization({ locale, ...params });
        }}
        rowKey="id"
        search={{
          layout: "vertical"
        }}
        pagination={{
          defaultPageSize: 10,
          showSizeChanger: true
        }}
      />
      <ModalForm
        title={<FormattedMessage id='menu.settings.localization' />}
        formRef={formRef}
        open={open}
        onOpenChange={setOpen}
        onFinish={onFinish}
      >
        <ProFormText name="id" hidden />
        <ProFormText
          name="key"
          label="Key"
          disabled={selectedItem}
          rules={[
            {
              required: true,
            },
          ]}
        />
        <ProFormText
          name="value"
          label="Value"
          rules={[
            {
              required: true,
            },
          ]}
        />
      </ModalForm>
    </PageContainer>
  );
};

export default Localization;
