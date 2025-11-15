import { apiNotificationCount } from "@/services/notification"
import { BellOutlined } from "@ant-design/icons"
import { history, Link, useRequest } from "@umijs/max"
import { Badge, Empty, Popover } from "antd"

const NotificationBadge: React.FC = () => {

    const { data } = useRequest(apiNotificationCount);

    const Content = () => {
        return (
            <>
                <div className="flex pb-1 border-b border-gray-200 border-dashed mb-2">
                    <div className="font-semibold">Thông báo</div>
                    <div className="ml-auto text-sm text-gray-500">{data} chưa đọc</div>
                </div>
                <Empty description="Đang phát triển" className="mb-2" />
                <div className="pt-1 text-center border-t border-gray-200 border-dashed">
                    <Link to="/notification" className="text-yellow-500 hover:underline">Xem tất cả</Link>
                </div>
            </>
        )
    }

    return (
        <Popover content={<Content />} placement="bottomRight">
            <Badge count={data}>
                <div className="w-6 h-6 flex items-center justify-center cursor-pointer hover:text-blue-500 transition-colors" onClick={() => history.push('/notification')}>
                    <BellOutlined className="text-lg" />
                </div>
            </Badge>
        </Popover>
    )
}

export default NotificationBadge;