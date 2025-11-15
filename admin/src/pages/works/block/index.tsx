import { useParams } from "@umijs/max";
import BlockEditorForm from "@/components/editorjs/block-editor-form";
import { saveArguments } from "@/services/work-content";
import { ProCard, ProForm } from "@ant-design/pro-components";
import { Col, Row, message } from "antd";
import WorkSummary from "@/components/works/summary";

const Block: React.FC = () => {
  const { id } = useParams();

  const onFinish = async (values: any) => {
    const response = await saveArguments(id, values.blockEditor);
    if (response.succeeded) {
      message.success('Saved');
    }
  };

  return (
    <Row gutter={16}>
      <Col md={16}>
        <ProCard>
          <ProForm onFinish={onFinish}>
            <BlockEditorForm />
          </ProForm>
        </ProCard>
      </Col>
      <Col md={8}>
        <WorkSummary />
      </Col>
    </Row>
  );
};

export default Block;
