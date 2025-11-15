import {
  EditOutlined,
  EllipsisOutlined,
  EyeOutlined,
  PlusOutlined,
  SettingOutlined,
} from '@ant-design/icons';
import { ModalForm, ProCard, ProFormDigit, ProFormText } from '@ant-design/pro-components';
import { Fragment, useEffect, useState } from 'react';
import { CatalogType } from '@/constants';
import { useParams } from '@umijs/max';
import { addCatalog } from '@/services/catalog';
import { message, Image, Empty, Divider, Descriptions, Typography, Button, Space, Tooltip } from 'antd';
import { absolutePath, formatDate } from '@/utils/format';
import TagList from './tag';
import { BASE_URL } from '@/utils/setting';

type Props = {
  catalog?: API.Catalog;
}

const CatalogSummary: React.FC<Props> = ({ catalog }) => {
  const [open, setOpen] = useState<boolean>(false);

  const onFinish = async (values: API.Catalog) => {
    values.active = true;
    const response = await addCatalog(values);
    if (response.succeeded) {
      message.success('Added!');
      setOpen(false);
    } else {
      message.error(response.errors[0].description)
    }
  }

  return (
    <ProCard
      title="Thông tin"
      headerBordered
    >
      <div className="flex items-center justify-center">
        {!catalog?.thumbnail ? (
          <Empty />
        ) : (
          <Image
            src={catalog?.thumbnail}
            height={200}
            className="object-cover"
            wrapperClassName='w-full'
          />
        )}
      </div>
      <Divider />
      <Descriptions title="Thống kê" column={1} bordered size='small'>
        <Descriptions.Item label="Lượt xem">
          {catalog?.viewCount.toLocaleString()}
        </Descriptions.Item>
        <Descriptions.Item label="Created date">
          {formatDate(catalog?.createdDate)}
        </Descriptions.Item>
        <Descriptions.Item label="Modified date">
          {formatDate(catalog?.modifiedDate)}
        </Descriptions.Item>
      </Descriptions>
      <ModalForm open={open} onOpenChange={setOpen} onFinish={onFinish} title="Create tag">
        <ProFormText name="name" rules={[
          {
            required: true
          }
        ]} label="Name" />
      </ModalForm>
    </ProCard>
  );
};

export default CatalogSummary;
