import ButtonUpload from '@/components/file-explorer/button';
import { listFile } from '@/services/file-service';
import { absolutePath } from '@/utils/format';
import {
  ActionType,
  ModalForm,
  ProList,
} from '@ant-design/pro-components';
import { useIntl } from '@umijs/max';
import { Image } from 'antd';
import { useRef } from 'react';

type GalleryProps = {
  open: boolean;
  onOpenChange: React.Dispatch<React.SetStateAction<boolean>>;
  onSelect?: any;
};

const Gallery: React.FC<GalleryProps> = (props) => {
  const intl = useIntl();
  const actionRef = useRef<ActionType>();

  return (
    <ModalForm open={props.open} onOpenChange={props.onOpenChange}>
      <ProList<API.FileContent>
        ghost
        toolBarRender={() => {
          return [
            <ButtonUpload key="upload" />
          ];
        }}
        headerTitle={intl.formatMessage({
          id: 'menu.fileManager',
        })}
        request={(params: any) =>
          listFile(
            {
              ...params,
            },
            ['.png', '.jpg', '.jpeg', 'image/jpeg'],
          )
        }
        search={{
          layout: 'vertical',
        }}
        pagination={{
          pageSize: 6,
        }}
        grid={{ column: 3 }}
        onItem={(record: any) => {
          return {
            onClick: () => {
              if (!props.onSelect) {
                return;
              }
              props.onSelect(record);
            },
          };
        }}
        metas={{
          content: {
            dataIndex: 'name',
            title: 'Name',
            render: (dom: string, record: any) => (
              <Image
                src={absolutePath(record.url)}
                height={100}
                preview={false}
              />
            ),
          },
        }}
        actionRef={actionRef}
      />
    </ModalForm>
  );
};

export default Gallery;
