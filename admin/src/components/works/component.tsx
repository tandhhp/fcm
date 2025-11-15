import { queryFormSelect } from '@/services/component';
import { ProFormSelect, ProFormSelectProps } from '@ant-design/pro-components';

const ComponentFormSelect: React.FC<ProFormSelectProps> = (props) => {
  return (
    <ProFormSelect
      showSearch
      request={queryFormSelect}
      name={props.name}
      label={props.label}
      rules={[
        {
          required: true
        }
      ]}
    />
  );
};

export default ComponentFormSelect;
