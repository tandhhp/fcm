import ProFormImage from '@/components/image/form';
import { getArguments } from '@/services/work-content';
import {
  ProForm,
  ProFormInstance,
  ProFormText,
  ProFormTextArea,
} from '@ant-design/pro-components';
import { useParams } from '@umijs/max';
import { Col, Row } from 'antd';
import { useEffect } from 'react';

const Jumbotron: React.FC = () => {
  const { id } = useParams();

  const formRef = ProForm.useFormInstance<ProFormInstance>();

  useEffect(() => {
    getArguments(id).then((response) => {
      formRef?.setFields([
        {
          name: 'backgroundImage',
          value: response.backgroundImage,
        },
      ]);
    });
  }, [id]);

  return (
    <Row gutter={16}>
      <Col md={16}>
        <ProFormImage name="backgroundImage" label="Background image" />
      </Col>
      <Col md={8}>
        <ProFormText name="title" label="Title" />
        <ProFormTextArea name="description" label="Description" />
      </Col>
    </Row>
  );
};

export default Jumbotron;
