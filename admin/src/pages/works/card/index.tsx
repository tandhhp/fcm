import FileExplorer from '@/components/file-explorer';
import ImagePreview from '@/components/image-preview';
import { getCard, saveCard } from '@/services/work-content';
import {
  PageContainer,
  ProCard,
  ProForm,
  ProFormField,
  ProFormInstance,
  ProFormText,
  ProFormTextArea,
} from '@ant-design/pro-components';
import { useIntl, useParams } from '@umijs/max';
import { message, Empty, Row, Col } from 'antd';
import { useEffect, useRef, useState } from 'react';

const WfCard: React.FC = () => {
  const intl = useIntl();
  const { id } = useParams();
  const formRef = useRef<ProFormInstance>();
  const [visible, setVisible] = useState<boolean>(false);
  const [image, setImage] = useState<API.FileContent>();

  useEffect(() => {
    getCard(id).then((response) => {
      if (response) {
        formRef.current?.setFields([
          {
            name: 'title',
            value: response.title,
          },
          {
            name: 'text',
            value: response.text,
          },
        ]);
        setImage(response.image);
      }
    });
  }, [id]);

  const onFinish = async (values: any) => {
    values.id = id;
    values.image = image;
    const response = await saveCard(values);
    if (response.succeeded) {
      message.success(
        intl.formatMessage({
          id: 'general.saved',
        }),
      );
    }
  };

  const onImageSelect = (values: API.FileContent) => {
    setImage(values);
    setVisible(false);
  };

  const renderImage = () => {
    if (image) {
      return <ImagePreview src={image} onClick={() => setVisible(true)} />;
    }
    return (
      <div className="image-placeholder" onClick={() => setVisible(true)}>
        <Empty />
      </div>
    );
  };

  return (
    <PageContainer
      title={intl.formatMessage({
        id: 'menu.component.card',
      })}
    >
      <Row gutter={16}>
        <Col span={4}>
          <ProCard title="Work contents">
            <Empty />
          </ProCard>
        </Col>
        <Col span={12}>
          <ProCard title="Content">
            <ProForm onFinish={onFinish} formRef={formRef}>
              <ProFormField label="Image">{renderImage()}</ProFormField>
              <ProFormText name="title" label="Title" />
              <ProFormTextArea name="text" label="Text" />
            </ProForm>
          </ProCard>
        </Col>
        <Col span={8}>
          <ProCard title="Preview">
            <Empty />
          </ProCard>
        </Col>
      </Row>
      <FileExplorer
        open={visible}
        onOpenChange={setVisible}
        onSelect={onImageSelect}
      />
    </PageContainer>
  );
};

export default WfCard;
