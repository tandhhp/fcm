import { getCatalog, saveCatalog } from '@/services/catalog';
import {
  ProForm,
  ProFormCheckbox,
  ProFormInstance,
  ProFormSelect,
  ProFormText,
  ProFormTextArea,
} from '@ant-design/pro-components';
import { useParams, getAllLocales } from '@umijs/max';
import { Button, Col, Row, Space, message } from 'antd';
import { useEffect, useRef, useState } from 'react';
import FormCatalogType from '@/components/form/catalog-type';
import FormCatalogList from '@/components/form/catalog-list';
import WfUpload from '@/components/file-explorer/upload';
import { UploadOutlined } from '@ant-design/icons';

type Props = {
  catalog?: API.Catalog;
  reload: any
}

const CatalogSetting: React.FC<Props> = ({ reload }) => {
  const { id } = useParams();

  const formRef = useRef<ProFormInstance>();
  const [upload, setUpload] = useState<boolean>(false);

  useEffect(() => {
    if (id) {
      getCatalog(id).then(catalog => {
        formRef.current?.setFields([
          {
            name: 'id',
            value: catalog.id,
          },
          {
            name: 'name',
            value: catalog.name,
          },
          {
            name: 'normalizedName',
            value: catalog.normalizedName,
          },
          {
            name: 'description',
            value: catalog.description,
          },
          {
            name: 'thumbnail',
            value: catalog.thumbnail,
          },
          {
            name: 'type',
            value: catalog.type.toString(),
          },
          {
            name: 'active',
            value: catalog.active,
          },
          {
            name: 'parentId',
            value: catalog.parentId
          },
          {
            name: 'locale',
            value: catalog.locale
          }
        ]);
      })
    }
  }, [id]);

  const onFinish = async (values: API.Catalog) => {
    values.type = Number(values.type);
    const response = await saveCatalog(values);
    if (response.succeeded) {
      message.success('Saved!');
      reload();
    }
  };

  return (
    <div>
      <ProForm formRef={formRef} onFinish={onFinish}>
        <ProFormText name="id" hidden />
        <ProFormText name="name" label="Tên" rules={[
          {
            required: true
          }
        ]} />
        <ProFormText name="normalizedName" label="Liên kết" rules={[
          {
            required: true
          }
        ]} />
        <ProFormTextArea name="description" label="Mô tả" />
        <Row gutter={16}>
          <Col span={12}>
            <FormCatalogType name="type" label="Loại" />
          </Col>
          <Col span={12}>
            <ProFormSelect
              options={getAllLocales().map(value => ({ label: value, value: value }))}
              name="locale" label="Ngôn ngữ" />
          </Col>
        </Row>
        <FormCatalogList name="parentId" label="Trang cha" />
        <Row gutter={16}>
          <Col span={16}>
            <ProFormText name="thumbnail" label="Ảnh đại diện" fieldProps={{
              suffix: (
                <Space>
                  <Button icon={<UploadOutlined />} onClick={() => setUpload(true)}>Upload</Button>
                </Space>
              )
            }} />
          </Col>
          <Col span={8}>
            <ProFormCheckbox name="active" label="Xuất bản" />
          </Col>
        </Row>
      </ProForm>
      <WfUpload open={upload} onCancel={setUpload} onFinish={(values: any) => {
        formRef.current?.setFieldValue('thumbnail', values.url);
      }} />
    </div>
  );
};

export default CatalogSetting;
