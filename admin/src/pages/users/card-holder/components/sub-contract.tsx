import { apiContractAdd } from "@/services/contact";
import { PlusSquareOutlined } from "@ant-design/icons";
import { ModalForm, ProFormText, ProFormTextArea } from "@ant-design/pro-components";
import { useAccess, useParams } from "@umijs/max";
import { Button, message } from "antd";
import { useState } from "react";

const SubContract: React.FC = () => {

    const access = useAccess();
    const { id } = useParams();
    const [open, setOpen] = useState<boolean>(false);

    const onFinish = async (values: any) => {
        values.cardHolderId = id;
        await apiContractAdd(values);
        setOpen(false);
        message.success("Thêm hợp đồng thành công");
        window.location.reload();
    }

    return (
        <>
            <Button size="small" type="text" icon={<PlusSquareOutlined />} className="ml-1" onClick={() => setOpen(true)} hidden={access.sales || access.telesale} />
            <ModalForm open={open} onOpenChange={setOpen} title="Hợp đồng thứ 2" onFinish={onFinish}>
                <ProFormText name="code" label="Số hợp đồng" rules={[
                    {
                        required: true
                    }
                ]} />
                <ProFormTextArea name="description" label="Mô tả" />
            </ModalForm>
        </>
    )
}

export default SubContract;