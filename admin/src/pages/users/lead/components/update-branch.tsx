import { apiUpdateKeyInBranch } from "@/services/key-in";
import { apiBranchOptions } from "@/services/settings/branch";
import { ModalForm, ModalFormProps, ProFormSelect } from "@ant-design/pro-components"
import { message } from "antd";

type Props = ModalFormProps & {
    keyIn?: any;
    reload: () => void;
}

const UpdateBranchModal: React.FC<Props> = (props) => {

    const onFinish = async (values: any) => {
        values.keyIn = props.keyIn?.id;
        await apiUpdateKeyInBranch(values);
        message.success("Cập nhật chi nhánh thành công");
        props.reload();
        props.onOpenChange?.(false);
    }

    return (
        <ModalForm {...props} title={`Cập nhật chi nhánh Key-In: ${props.keyIn?.name}`} onFinish={onFinish}>
            <ProFormSelect name="branchId" label="Chi nhánh" rules={[{ required: true }]} request={apiBranchOptions} />
        </ModalForm>
    )
}

export default UpdateBranchModal