import { queryCatalogSelect } from '@/services/catalog';
import { ProFormSelect, ProFormSelectProps } from '@ant-design/pro-components';
import { getLocale } from '@umijs/max';

const FormCatalogList: React.FC<ProFormSelectProps> = (props) => {
    return (
        <ProFormSelect
            {...props}
            showSearch
            request={(params) => queryCatalogSelect({
                ...params,
                locale: getLocale()
            })}
        />
    );
};

export default FormCatalogList;
