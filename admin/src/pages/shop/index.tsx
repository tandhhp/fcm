import { CatalogType } from '@/constants';
import {
  PageContainer,
} from '@ant-design/pro-components';
import CatalogList from '@/components/catalog/list';

const ShopPage: React.FC = () => {
  return (
    <PageContainer>
      <CatalogList type={CatalogType.Product} />
    </PageContainer>
  );
};

export default ShopPage;
