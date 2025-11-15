import {
  ModalForm,
  ProFormText,
  ProFormTextArea,
  ProFormCheckbox,
} from '@ant-design/pro-components';
import { useIntl } from '@umijs/max';

type NewCatalogProps = {
  type: number;
  open: boolean;
  setOpen: any;
  onFinish: any;
};

const NewCatalog: React.FC<NewCatalogProps> = (props) => {
  const intl = useIntl();

  return (
    <ModalForm
      open={props.open}
      onOpenChange={props.setOpen}
      onFinish={props.onFinish}
      title={intl.formatMessage({
        id: 'general.new',
      })}
    >
      <ProFormText
        label="Name"
        name="name"
        rules={[
          {
            required: true,
          },
        ]}
      />
      <ProFormTextArea label="Description" name="description" />
      <ProFormText name="type" initialValue={props.type} label="Type" />
      <ProFormCheckbox name="active" initialValue={false} hidden />
    </ModalForm>
  );
};

export default NewCatalog;
