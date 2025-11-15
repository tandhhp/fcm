import WorkSummary from '@/components/works/summary';
import { ArrowLeftOutlined } from '@ant-design/icons';
import { PageContainer, ProCard } from '@ant-design/pro-components';
import { FormattedMessage, history } from '@umijs/max';
import { Button, Col, Row } from 'antd';
import { useEffect, useState } from 'react';
import ContactFormContent from './components/content';
import ContactFormSetting from './components/setting';
import ContactFormLabels from './components/labels';
import { getArguments } from '@/services/work-content';
import { useParams } from '@umijs/max';
import ContactFormCategory from './components/category';

const ContactForm: React.FC = () => {
  const [tab, setTab] = useState('content');
  const { id } = useParams();
  const [data, setData] = useState<API.ContactForm>();

  useEffect(() => {
    if (id) {
      getArguments(id).then((response) => {
        response.id = id;
        setData(response);
      })
    }
  }, [id, tab]);

  return (
    <PageContainer
      extra={
        <Button icon={<ArrowLeftOutlined />} onClick={() => history.back()}>
          <FormattedMessage id="general.back" />
        </Button>
      }
    >
      <Row gutter={16}>
        <Col span={16}>
          <ProCard
            tabs={{
              activeKey: tab,
              items: [
                {
                  label: 'Content',
                  key: 'content',
                  children: <ContactFormContent {...data} />,
                },
                {
                  label: 'Labels',
                  key: 'labels',
                  children: <ContactFormLabels {...data} />,
                },
                {
                  label: 'Categories',
                  key: 'categories',
                  children: <ContactFormCategory {...data} />,
                },
                {
                  label: 'Setting',
                  key: 'setting',
                  children: <ContactFormSetting />,
                },
              ],
              onChange: (key) => {
                setTab(key);
              },
            }}
          />
        </Col>
        <Col span={8}>
          <WorkSummary />
        </Col>
      </Row>
    </PageContainer>
  );
};

export default ContactForm;
