import { listRole } from "@/services/role";
import { addToRole } from "@/services/user";
import { PlusOutlined } from "@ant-design/icons";
import { ModalForm, ProFormSelect } from "@ant-design/pro-components";
import { useModel, useParams } from "@umijs/max";
import { Button, Tag, Typography, message } from "antd";
import { useState } from "react";

type ProfileRolesProps = {
    roles: string[];
}

const ProfileRoles: React.FC<ProfileRolesProps> = (props) => {
    const { initialState } = useModel('@@initialState');
    const { currentUser } = initialState || {};
    const { roles } = props;
    const { id } = useParams();
    const [open, setOpen] = useState<boolean>(false);

    const onFinish = async (values: any) => {
        values.id = id;
        const response = await addToRole(values);
        if (response.succeeded) {
            message.success('Added!');
            setOpen(false);
        } else {
            message.error(response.errors[0].description);
        }
    }

    return (
        <>
            <Typography.Title level={5}>Roles</Typography.Title>
            {roles.map((role) => (
                <Tag key={role} color="blue">
                    {role}
                </Tag>
            ))}
            {
                currentUser?.roles.includes('admin') && (
                    <Button type='dashed' size='small' icon={<PlusOutlined />} onClick={() => setOpen(true)}>Add to role</Button>
                )
            }
            <ModalForm title="Add to role" open={open} onOpenChange={setOpen} onFinish={onFinish}>
                <ProFormSelect name="roleName" label="Role" request={async (params) => {
                    const response = await listRole(params);
                    return response.data.map((role: API.Role) => {
                        return {
                            value: role.name,
                            label: role.name
                        }
                    })
                }} />
            </ModalForm>
        </>
    )
}

export default ProfileRoles