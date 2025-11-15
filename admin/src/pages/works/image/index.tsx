import WfUpload from '@/components/file-explorer/upload';
import ProFormLink from '@/components/link';
import FilePreview from '@/pages/files/center/preview';
import { deleteWork, getArguments, saveArguments } from '@/services/work-content';
import {
  ArrowLeftOutlined,
  BarsOutlined,
  DeleteOutlined,
} from '@ant-design/icons';
import {
  PageContainer,
  ProCard,
  ProForm,
  ProFormInstance,
  ProFormText,
} from '@ant-design/pro-components';
import { history, useIntl, useParams } from '@umijs/max';
import { Button, Col, message, Popconfirm, Row, Space } from 'antd';
import { useEffect, useRef, useState } from 'react';

const Image: React.FC = () => {
  const { id } = useParams();
  const intl = useIntl();

  const [image, setImage] = useState<API.FileContent>();
  const [open, setOpen] = useState<boolean>(false);

  const formRef = useRef<ProFormInstance>();

  const onFinish = async (values: API.Image) => {
    const response = await saveArguments(id, values);
    if (response.succeeded) {
      message.success('Saved!');
    } else {
      message.error(response.errors[0].description);
    }
  };

  useEffect(() => {
    getArguments(id).then((response) => {
      setImage(response.file);
      formRef.current?.setFields([
        {
          name: 'alt',
          value: response.alt,
        },
        {
          name: 'src',
          value: response.src,
        },
        {
          name: 'link',
          value: response.link,
        },
      ]);
    });
  }, []);

  const onConfirm = async () => {
    const response = await deleteWork(id);
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
      <Button icon={<ArrowLeftOutlined />} onClick={() => history.back()}>
        Back
      </Button>
      <Button icon={<BarsOutlined />} />
    </Space>
  );

  const onSelect = (values: API.FileContent) => {
    setImage(values);
    setOpen(false);
  };

  return (
    <PageContainer title="Image" extra={extra}>
      <Row gutter={16}>
        <Col span={6}>
          <FilePreview file={image} onChange={() => setOpen(true)} />
        </Col>
        <Col span={18}>
          <ProCard
            title={intl.formatMessage({
              id: 'menu.settings',
            })}
          >
            <ProForm onFinish={onFinish} formRef={formRef}>
              <ProFormText name="src" label="Src" />
              <ProFormLink name="link" label="Link" />
              <ProFormText name="alt" label="Alt" />
            </ProForm>
          </ProCard>
        </Col>
      </Row>
      <WfUpload open={open} onCancel={setOpen} onFinish={onSelect} />
    </PageContainer>
  );
};

export default Image;
