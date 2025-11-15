import { getChildList } from '@/services/work-content';
import { DeleteOutlined, FolderOutlined } from '@ant-design/icons';
import { ProList } from '@ant-design/pro-components';
import { history, useParams } from '@umijs/max';
import { Button, Popconfirm } from 'antd';

type ChildWorkContentProps = {
  actionRef?: any;
};

const ChildWorkContent: React.FC<ChildWorkContentProps> = (props) => {
  const { id } = useParams();

  const onConfirm = () => {};

  return (
    <ProList<API.WorkItem>
      search={{}}
      rowSelection={{}}
      request={(params) => getChildList(params, id)}
      actionRef={props.actionRef}
      metas={{
        title: {
          dataIndex: 'name',
          title: 'Name',
        },
        actions: {
          render: (text, row) => [
            <Button
              type="primary"
              icon={<FolderOutlined />}
              key={0}
              onClick={() =>
                history.push(
                  `/works/${row.normalizedName.toLocaleLowerCase()}/${row.id}`,
                )
              }
            ></Button>,
            <Popconfirm
              key={1}
              title="Are you sure?"
              onConfirm={() => onConfirm()}
            >
              <Button type="primary" danger icon={<DeleteOutlined />} />
            </Popconfirm>,
          ],
        },
      }}
    />
  );
};

export default ChildWorkContent;
