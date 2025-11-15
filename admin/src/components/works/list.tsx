import {
  addChildWorkContent,
  deleteWork,
  getChildList,
} from '@/services/work-content';
import {
  PlusOutlined,
  ArrowUpOutlined,
  ArrowDownOutlined,
  DeleteOutlined,
} from '@ant-design/icons';
import { ActionType, ProList } from '@ant-design/pro-components';
import { FormattedMessage, useParams, history } from '@umijs/max';
import { Button, Popconfirm, message } from 'antd';
import { useRef, useState } from 'react';
import AddComponent from '../add-component';

const WorkList: React.FC = () => {
  const { id } = useParams();
  const actionRef = useRef<ActionType>();
  const [open, setOpen] = useState<boolean>(false);

  const onFinish = async (values: any) => {
    const body: API.WorkContent = {
      parentId: id,
      ...values,
    };
    const response = await addChildWorkContent(body);
    if (response.succeeded) {
      message.success('Added!');
      actionRef.current?.reload();
      setOpen(false);
    }
  };

  const onConfirm = async (workId: string) => {
    const response = await deleteWork(workId);
    if (response.succeeded) {
      message.success('Deleted');
      actionRef.current?.reload();
    } else {
      message.error(response.errors[0].description);
    }
  };

  return (
    <div>
      <ProList<API.WorkItem>
        toolBarRender={() => {
          return [
            <Button
              key={0}
              icon={<PlusOutlined />}
              type="primary"
              onClick={() => setOpen(true)}
            >
              <FormattedMessage id="general.new" />
            </Button>,
          ];
        }}
        headerTitle="Content"
        request={(params) => getChildList(params, id)}
        actionRef={actionRef}
        metas={{
          title: {
            dataIndex: 'name',
            render: (dom, row) => (
              <a
                onClick={() =>
                  history.push(
                    `/works/${row.normalizedName.toLocaleLowerCase()}/${
                      row.id
                    }`,
                  )
                }
              >
                {dom}
              </a>
            ),
          },
          actions: {
            render: (dom, entity) => [
              <Button key={2} icon={<ArrowUpOutlined />}></Button>,
              <Button key={3} icon={<ArrowDownOutlined />}></Button>,
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
              </Popconfirm>,
            ],
          },
        }}
      />

      <AddComponent open={open} onOpenChange={setOpen} onFinish={onFinish} />
    </div>
  );
};

export default WorkList;
