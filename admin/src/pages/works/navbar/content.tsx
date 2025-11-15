import {
  addNavbarItem,
  deleteWork,
  getChildList,
} from '@/services/work-content';
import { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import {
  ActionType,
  ModalForm,
  ProFormText,
  ProList,
} from '@ant-design/pro-components';
import { FormattedMessage, history, useParams } from '@umijs/max';
import { Button, message, Popconfirm } from 'antd';
import { useRef, useState } from 'react';

const NavbarContent: React.FC = () => {
  const { id } = useParams();
  const actionRef = useRef<ActionType>();
  const [open, setOpen] = useState<boolean>(false);

  const onFinish = async (values: API.NavItem) => {
    const response = await addNavbarItem(values);
    if (response.succeeded) {
      message.success('Added');
      actionRef.current?.reload();
      setOpen(false);
    }
  };

  const onConfirm = async (id: string) => {
    const response = await deleteWork(id);
    if (response.succeeded) {
      message.success('Deleted');
      actionRef.current?.reload();
    }
  };

  return (
    <div>
      <ProList<API.NavItem>
        request={(params) => getChildList(params, id)}
        toolBarRender={() => {
          return [
            <Button
              key={0}
              type="primary"
              icon={<PlusOutlined />}
              onClick={() => setOpen(true)}
            >
              <FormattedMessage id="general.new" />
            </Button>,
          ];
        }}
        metas={{
          title: {
            dataIndex: 'name',
          },
          actions: {
            render: (dom, entity) => [
              <Button
                icon={<EditOutlined />}
                key={1}
                onClick={() => history.push(`/works/nav-item/${entity.id}`)}
              />,
              <Popconfirm
                title="Are you sure?"
                onConfirm={() => onConfirm(entity.id)}
                key={2}
              >
                <Button icon={<DeleteOutlined />} danger type="primary" />
              </Popconfirm>,
            ],
          },
        }}
        actionRef={actionRef}
      />
      <ModalForm open={open} onOpenChange={setOpen} onFinish={onFinish}>
        <ProFormText name="parentId" hidden initialValue={id} />
        <ProFormText name="name" label="Name" />
      </ModalForm>
    </div>
  );
};

export default NavbarContent;
