import CatalogList from "@/components/catalog/list"
import { CatalogType } from "@/constants"
import { PageContainer } from "@ant-design/pro-components";

const ProductPage : React.FC = () => {
    return (
        <PageContainer>
            <CatalogList type={CatalogType.Product} />
        </PageContainer>
    )
}

export default ProductPage;