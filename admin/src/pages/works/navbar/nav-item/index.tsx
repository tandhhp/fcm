import ModalLink from '@/components/modals/link';
import WorkSummary from '@/components/works/summary';
import {
  deleteNavItem,
  listNavItem,
  saveNavItem,
} from '@/services/work-content';
import { DeleteOutlined, EyeOutlined, PlusOutlined } from '@ant-design/icons';
import {
  ActionType,
  PageContainer,
  ProCard,
  ProList,
} from '@ant-design/pro-components';
import { FormattedMessage, useParams } from '@umijs/max';
import { Button, Col, message, Row } from 'antd';
import { useRef, useState } from 'react';

const NavItemComponent: React.FC = () => {
  const { id } = useParams();
  const [open, setOpen] = useState<boolean>(false);
  const actionRef = useRef<ActionType>();

  const onFinish = async (values: CPN.Link) => {
    const response = await saveNavItem(id, values);
    if (response.succeeded) {
      message.success('Saved!');
      actionRef.current?.reload();
      setOpen(false);
    } else {
      message.error(response.errors[0].description);
    }
  };

  const onRemove = async (linkId: string) => {
    const response = await deleteNavItem(linkId, id);
    if (response.succeeded) {
      message.success('Deleted');
      actionRef.current?.reload();
    } else {
      message.error(response.errors[0].description);
    }
  };

  return (
    <PageContainer>
      <Row gutter={16}>
        <Col span={16}>
          <ProCard>
            <ProList<CPN.Link>
              actionRef={actionRef}
              request={() => listNavItem(id)}
              toolBarRender={() => {
                return [
                  <Button
                    key={0}
                    type="primary"
                    icon={<PlusOutlined />}
                    onClick={() => setOpen(true)}
                  >
                    <FormattedMessage id="general.new" />
                  </Button>,
                ];
              }}
              metas={{
                title: {
                  dataIndex: 'name',
                },
                actions: {
                  render: (dom, entity) => [
                    <Button
                      icon={<EyeOutlined />}
                      key={1}
                      onClick={() => (window.location.href = entity.href)}
                    />,
                    <Button
                      icon={<DeleteOutlined />}
                      key={2}
                      danger
                      type="primary"
                      onClick={() => onRemove(entity.id)}
                    />,
                  ],
                },
              }}
            />
          </ProCard>
        </Col>
        <Col span={8}>
          <WorkSummary />
        </Col>
      </Row>
      <ModalLink open={open} onOpenChange={setOpen} onFinish={onFinish} />
    </PageContainer>
  );
};

export default NavItemComponent;
