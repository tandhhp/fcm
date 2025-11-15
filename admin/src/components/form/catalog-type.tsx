import { listTypes } from '@/services/catalog';
import { ProFormSelect, ProFormSelectProps } from '@ant-design/pro-components';


const FormCatalogType: React.FC<ProFormSelectProps> = (props) => {
  return (
    <ProFormSelect
      {...props}
      request={listTypes}
      label={props.label}
      name={props.name}
      initialValue={props.initialValue}
      rules={[
        {
          required: true
        }
      ]}
    />
  );
};

export default FormCatalogType;
