import { listTrainerUser } from "@/services/user"
import { ManOutlined, WomanOutlined } from "@ant-design/icons"
import { ProColumns, ProTable } from "@ant-design/pro-components"
import { Tag } from "antd"
import dayjs from "dayjs"

const TrainerComponent: React.FC = () => {
    const columns: ProColumns<any>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
            width: 40
        },
        {
            title: 'Tài khoản',
            dataIndex: 'userName',
            search: false
        },
        {
            title: 'Họ & Tên',
            dataIndex: 'name',
            render: (dom, entity) => (
                <div>{entity.gender === true && (<ManOutlined className='text-blue-500' />)}{entity.gender === false && (<WomanOutlined className='text-red-500' />)} {dom}</div>
            )
        },
        {
            title: 'Email',
            dataIndex: 'email'
        },
        {
            title: 'Số điện thoại',
            dataIndex: 'phoneNumber',
            width: 110
        },
        {
            title: 'Ngày sinh',
            dataIndex: 'dateOfBirth',
            valueType: 'date',
            width: 100,
            search: false,
            render: (_, entity) => entity.dateOfBirth ? dayjs(entity.dateOfBirth).format('DD-MM-YYYY') : '-'
        },
        {
            title: 'Địa chỉ',
            dataIndex: 'address',
            search: false
        },
        {
            title: 'CCCD',
            dataIndex: 'identityNumber',
            search: false
        },
        {
            title: 'Ngày cấp',
            dataIndex: 'identityDate',
            valueType: 'date',
            width: 100,
            search: false
        },
        {
            title: 'Nơi cấp',
            dataIndex: 'identityAddress',
            search: false
        },
        {
            title: 'Trạng thái',
            dataIndex: 'status',
            search: false,
            width: 80,
            valueEnum: {
                0: <Tag color="processing">Đang làm</Tag>,
                1: <Tag color="error">Đã nghỉ</Tag>
            }
        }
    ]

    return (
        <ProTable
            className="mb-4"
            scroll={{
                x: true
            }}
            search={{
                layout: 'vertical'
            }}
            request={listTrainerUser} columns={columns} />
    )
}

export default TrainerComponent;