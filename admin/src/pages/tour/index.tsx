import CatalogList from "@/components/catalog/list";
import { CatalogType } from "@/constants";
import { PageContainer } from "@ant-design/pro-components"

const TourPage: React.FC = () => {
    return (
        <PageContainer>
            <CatalogList type={CatalogType.Tour} />
        </PageContainer>
    )
}

export default TourPage;