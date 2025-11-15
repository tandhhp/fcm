import { apiUserClaims, apiUserClaimUpdate } from "@/services/user";
import { DrawerForm, DrawerFormProps, ProFormCheckbox } from "@ant-design/pro-components"
import { message } from "antd";
import { useEffect, useState } from "react";

type Props = DrawerFormProps & {
    data?: any;
};

const ClaimForm: React.FC<Props> = (props) => {

    const [data, setData] = useState<any>();

    useEffect(() => {
        if (props.data && props.open) {
            apiUserClaims({ userId: props.data.id }).then(res => {
                setData(res.data);
            });
        }
    }, [props.data, props.open]);

    const onChange = async (changedValues: any) => {
        await apiUserClaimUpdate({
            userId: props.data.id,
            claimType: changedValues.type,
            claimValue: changedValues.value
        });
        message.success('Cập nhật quyền thành công');
        props.onOpenChange?.(false);
    }

    return (
        <DrawerForm {...props} drawerProps={{
            destroyOnHidden: true
        }} submitter={false}>
            {
                data && data.map((item: any, index: number) => (
                    <ProFormCheckbox key={index} name={item.value} label={item.value}
                    fieldProps={{
                        checked: item.hasClaim,
                        onChange: () => onChange(item)
                    }} />
                ))
            }
        </DrawerForm>
    )
}

export default ClaimForm;