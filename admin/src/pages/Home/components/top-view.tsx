import { CatalogType } from "@/constants";
import { apiTopView, queryViewCount } from "@/services/catalog";
import { EyeOutlined } from "@ant-design/icons";
import { ProCard, ProList } from "@ant-design/pro-components"
import { FormattedMessage } from "@umijs/max";
import moment from "moment";
import { useEffect, useState } from "react"

const TopView: React.FC = () => {

    const [dataSource, setDataSource] = useState<API.Catalog[]>([]);
    const [activeKey, setActiveKey] = useState<string>(CatalogType.Tour.toString());
    const [viewCount, setViewCount] = useState<number>(0);
  
    useEffect(() => {
      queryViewCount().then((response) => {
        setViewCount(response);
      });
    }, []);

    useEffect(() => {
        apiTopView(activeKey).then(response => {
            setDataSource(response);
        })
    }, [activeKey]);

    const Children = () => <ProList
        ghost
        dataSource={dataSource}
        metas={{
            title: {
                dataIndex: 'name',
                render: (dom) => <div className="line-clamp-1">{dom}</div>
            },
            description: {
                render: (dom, entity) => (
                    <>{moment(entity.modifiedDate).format('DD/MM/YYYY hh:mm')} | <EyeOutlined /> {entity.viewCount.toLocaleString()}</>
                )
            }
        }}
    />

    return (
        <>
            <ProCard
                headerBordered
                extra={`Tổng: ${viewCount}`}
                title="Lượt xem nhiều" tabs={{
                    tabPosition: 'top',
                    activeKey: activeKey,
                    onChange: (actKey) => setActiveKey(actKey),
                    items: [
                        {
                            label: <FormattedMessage id='menu.ecommerce.product' />,
                            key: CatalogType.Product.toString(),
                            children: <Children />,
                        },
                        {
                            label: 'Cơ sở khám',
                            key: CatalogType.Healthcare.toString(),
                            children: <Children />,
                        },
                        {
                            label: 'Dưỡng sinh độc bản',
                            key: CatalogType.Tour.toString(),
                            children: <Children />,
                        }
                    ],
                }}>

            </ProCard>
        </>
    )
}

export default TopView