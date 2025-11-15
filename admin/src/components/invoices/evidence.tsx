import { DrawerForm, DrawerFormProps } from "@ant-design/pro-components";
import { Image } from "antd";

type Props = DrawerFormProps & {
    data?: any;
}

const InvoiceEvidence: React.FC<Props> = (props) => {
    return (
        <DrawerForm {...props} title="Chứng từ hóa đơn" width={400} submitter={false}>
            <Image src={props.data?.evidenceUrl} />
        </DrawerForm>
    )
}

export default InvoiceEvidence;