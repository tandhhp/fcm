import { FormTag } from '@/components/form';
import { listTagById } from '@/services/catalog';
import { addItem, deleteItem } from '@/services/work-content';
import { PlusOutlined } from '@ant-design/icons';
import {
  ModalForm,
  ProFormDigit,
} from '@ant-design/pro-components';
import { FormattedMessage, useParams } from '@umijs/max';
import { Tag, Button, message, Space } from 'antd';
import { useEffect, useState } from 'react';

const TagList: React.FC = () => {
  const { id } = useParams();
  const [tags, setTags] = useState<API.Catalog[]>();
  const [open, setOpen] = useState<boolean>(false);

  const fetchTag = () => {
    listTagById(id).then((response) => {
      setTags(response);
    });
  };

  useEffect(() => {
    fetchTag();
  }, []);

  const onFinish = async (values: API.WorkItem) => {
    const response = await addItem({
      workId: id,
      catalogId: values.id,
      sortOrder: values.sortOrder
    });
    if (response.succeeded) {
      message.success('Saved!');
      setOpen(false);
      fetchTag();
    } else {
      message.error(response.errors[0].description);
    }
  };

  const onClose = async (tagId?: string) => {
    const response = await deleteItem({
      catalogId: tagId,
      workId: id,
      sortOrder: 0,
    });
    if (response.succeeded) {
      const newTags = tags?.filter((tag) => tag.id !== tagId);
      setTags(newTags);
      message.success('Deleted');
    } else {
      message.error(response.errors[0].description);
    }
  };

  return (
    <div>
      {tags?.map((tag) => (
        <Tag key={tag.id} closable onClose={() => onClose(tag.id)} className='mb-2'>
          {tag.name}
        </Tag>
      ))}
      <Button
        size="small"
        type="dashed"
        onClick={() => setOpen(true)}
      >
        <Space>
          <PlusOutlined />
          <FormattedMessage id="general.new" />
        </Space>
      </Button>
      <ModalForm open={open} onOpenChange={setOpen} onFinish={onFinish} title="Thêm mới">
        <FormTag label='Tag' name='id' />
        <ProFormDigit label="Sort order" name="sortOrder" />
      </ModalForm>
    </div>
  );
};

export default TagList;
