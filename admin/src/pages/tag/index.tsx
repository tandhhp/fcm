import CatalogList from '@/components/catalog/list';
import { CatalogType } from '@/constants';
import { PageContainer } from '@ant-design/pro-components';

const TagPage: React.FC = () => {
  return (
    <PageContainer>
      <CatalogList type={CatalogType.Tag} />
    </PageContainer>
  );
};

export default TagPage;
