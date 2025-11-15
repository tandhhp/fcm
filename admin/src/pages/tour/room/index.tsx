import CatalogList from "@/components/catalog/list";
import { CatalogType } from "@/constants";
import { PageContainer } from "@ant-design/pro-components"

const RoomPage : React.FC = () => {
    return (
        <PageContainer>
            <CatalogList type={CatalogType.Room} />
        </PageContainer>
    )
}

export default RoomPage;