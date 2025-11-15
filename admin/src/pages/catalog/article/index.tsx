import CatalogList from '@/components/catalog/list';
import { CatalogType } from '@/constants';
import {
    PageContainer,
} from '@ant-design/pro-components';

const ArticleList: React.FC = () => {

    return (
        <PageContainer>
            <CatalogList type={CatalogType.Article} />
        </PageContainer>
    );
};

export default ArticleList;
