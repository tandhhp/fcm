import WorkContentComponent from '@/components/works';
import { addCatalog, getCatalog } from '@/services/catalog';
import {
  ModalForm,
  PageContainer,
  ProCard,
  ProFormText,
} from '@ant-design/pro-components';
import { FormattedMessage, history, useParams } from '@umijs/max';
import { Button, Col, message, Row } from 'antd';
import React, { Fragment, useEffect, useState } from 'react';
import CatalogSetting from './setting';
import CatalogSummary from './summary';
import { CatalogType } from '@/constants';
import ProductDetail from './products/detail';
import { LeftOutlined } from '@ant-design/icons';
import CatalogContent from './content';
import Itinerary from './components/itinerary';
import HospitalContent from './components/hospital';
import ChildTour from './child-tour';

const CatalogPage: React.FC = () => {
  const { id } = useParams();

  const [open, setOpen] = useState<boolean>(false);
  const [catalog, setCatalog] = useState<API.Catalog>();
  const [tab, setTab] = useState('content');

  const reload = () => {
    getCatalog(id).then((response) => setCatalog(response));
  }

  useEffect(() => {
    reload();
  }, [id]);

  const onFinish = async (values: API.Catalog) => {
    addCatalog(values).then((response) => {
      if (response.succeeded) {
        message.success('Saved!');
        setOpen(false);
      }
    });
  };

  const onTabChange = (key: string) => {
    setTab(key);
  }

  const renderContent = () => {
    if (!catalog) {
      return <Fragment />;
    }
    if (catalog.type === CatalogType.Tour && catalog.parentId) {
      return <Itinerary />;
    }
    if (catalog.type === CatalogType.Tour || catalog.type === CatalogType.Room) {
      return <CatalogContent />;
    }
    if (catalog.type === CatalogType.Hospital) {
      return <HospitalContent />
    }
    if (catalog.type === CatalogType.Product) {
      return <ProductDetail />;
    }
    return <WorkContentComponent />;
  }

  return (
    <PageContainer
      title={catalog?.name}
      extra={<Button icon={<LeftOutlined />} onClick={() => history.back()}><span><FormattedMessage id='general.back' /></span></Button>}
    >
      <Row gutter={16}>
        <Col md={18}>
          <ProCard
            tabs={{
              tabPosition: 'top',
              activeKey: tab,
              items: [
                {
                  label: 'Nội dung',
                  key: 'content',
                  children: renderContent(),
                },
                {
                  label: <FormattedMessage id='menu.settings' />,
                  key: 'setting',
                  children: <CatalogSetting catalog={catalog} reload={reload} />,
                },
                {
                  label: 'Gói nghỉ dưỡng',
                  key: 'simple',
                  children: <ChildTour resetKey={() => {
                    setTab('content')
                  }} />,
                  disabled: catalog?.type !== CatalogType.Tour || catalog?.parentId !== null
                }
              ],
              onChange: onTabChange,
            }}
            className='mb-4'
          />
          <ModalForm onFinish={onFinish} open={open} onOpenChange={setOpen}>
            <ProFormText name="name" label="Tên" />
            <ProFormText name="normalizedName" label="Normalized Name" />
          </ModalForm>
        </Col>
        <Col md={6}>
          <CatalogSummary catalog={catalog} />
        </Col>
      </Row>
    </PageContainer>
  );
};

export default CatalogPage;
