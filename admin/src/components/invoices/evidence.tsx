import { apiInvoiceEvidences } from "@/services/finances/invoice";
import { DrawerForm, DrawerFormProps } from "@ant-design/pro-components";
import { Image } from "antd";
import { useEffect, useState } from "react";

type Props = DrawerFormProps & {
    data?: any;
}

const InvoiceEvidence: React.FC<Props> = (props) => {

    const [data, setData] = useState<any[]>([]);

    useEffect(() => {
        if (props.open) {
            apiInvoiceEvidences(props.data?.id).then((res) => {
                setData(res.data || []);
            });
        }
    }, [props.open]);

    return (
        <DrawerForm {...props} title="Chứng từ hóa đơn" width={400} submitter={false}
        drawerProps={{
            destroyOnHidden: true
        }}
        >
            {data?.map((item: any) => (
                <Image key={item.id} src={item.url} className="mb-2" />
            ))}
        </DrawerForm>
    )
}

export default InvoiceEvidence;