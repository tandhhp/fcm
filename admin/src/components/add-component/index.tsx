import { ModalForm, ModalFormProps, ProFormText } from '@ant-design/pro-components';
import ComponentFormSelect from '../works/component';

const AddComponent: React.FC<ModalFormProps> = (props) => {
  return (
    <ModalForm {...props} title='Add component'>
      <ProFormText name="name" label="Name" />
      <ComponentFormSelect name="componentId" label="Component" />
    </ModalForm>
  );
};

export default AddComponent;
