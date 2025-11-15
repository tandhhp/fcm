import { useParams } from "@umijs/max";
import BlockEditorForm from "@/components/editorjs/block-editor-form";
import { saveArguments } from "@/services/work-content";
import { PageContainer, ProCard, ProForm } from "@ant-design/pro-components";
import { Button, Col, Row, message } from "antd";
import WorkSummary from "@/components/works/summary";
import { history } from "@umijs/max";
import { ArrowLeftOutlined } from "@ant-design/icons";

const EditorPage: React.FC = () => {
  const { id } = useParams();

  const onFinish = async (values: any) => {
    const response = await saveArguments(id, values.blockEditor);
    if (response.succeeded) {
      message.success('Saved');
    }
  };

  return (
    <PageContainer extra={<Button onClick={() => history.back()} icon={<ArrowLeftOutlined />}></Button>}>
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
    </PageContainer>
  );
};

export default EditorPage;
