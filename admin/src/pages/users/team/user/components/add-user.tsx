import { apiUserOptions } from "@/services/user"
import { apiTeamAddUser } from "@/services/users/team"
import { UserAddOutlined } from "@ant-design/icons"
import { ModalForm, ProFormSelect } from "@ant-design/pro-components"
import { useParams } from "@umijs/max"
import { Button, message } from "antd"

type Props = {
    reload?: () => void;
}

const TeamAddUser: React.FC<Props> = ({ reload }) => {

    const { id } = useParams<{ id: string }>();

    return (
        <ModalForm title="Thêm thành viên" trigger={(
            <Button type="primary" icon={<UserAddOutlined />}>Thêm thành viên</Button>
        )} onFinish={async (values) => {
            await apiTeamAddUser({
                ...values,
                teamId: id
            });
            message.success('Thêm thành viên thành công');
            reload?.();
            return true;
        }}>
            <ProFormSelect name={"userId"} label="Người dùng" request={apiUserOptions} rules={[{ required: true }]} showSearch />
        </ModalForm>
    )
}

export default TeamAddUser;