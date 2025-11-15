import { apiFormDetail } from "@/services/form";
import { Descriptions, Modal } from "antd"
import { useEffect, useState } from "react";

type Props = {
    open: boolean;
    setOpen: any;
    formId: string;
}

const FormDetail: React.FC<Props> = ({ open, setOpen, formId }) => {

    const [data, setData] = useState<any>();

    useEffect(() => {
        if (formId) {
            apiFormDetail(formId).then(response => {
                setData(response);
            })
        }
    }, [formId]);

    return (
        <Modal
            width={1000}
            open={open} onCancel={() => setOpen(false)} title="Chi tiết đơn đăng ký" footer={false} centered>
            <Descriptions bordered column={2}>
                <Descriptions.Item label="Số người lớn">{data?.form?.adult}</Descriptions.Item>
                <Descriptions.Item label="Số trẻ em">{data?.form?.children}</Descriptions.Item>
                <Descriptions.Item label="Ghi chú">{data?.form?.note}</Descriptions.Item>
            </Descriptions>
        </Modal>
    )
}

export default FormDetail;