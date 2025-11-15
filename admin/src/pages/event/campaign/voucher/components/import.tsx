import { apiCampaignOptions } from "@/services/event/campaign";
import { apiVoucherImport } from "@/services/event/voucher";
import { DownloadOutlined } from "@ant-design/icons";
import { ModalForm, ModalFormProps, ProFormSelect, ProFormUploadDragger } from "@ant-design/pro-components";
import { useParams } from "@umijs/max";
import { message } from "antd";

type Props = ModalFormProps & {
    reload?: () => void;
}

const VoucherImport: React.FC<Props> = (props) => {

    const onFinish = async (values: any) => {
        const formData = new FormData();
        formData.append('file', values.file[0].originFileObj);
        formData.append('campaignId', values.campaignId);
        await apiVoucherImport(formData);
        message.success('Thành công!');
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Import voucher từ file excel" onFinish={onFinish}>
            <div className="mb-3 flex justify-end text-blue-500"><a href="https://docs.google.com/spreadsheets/d/1cqFT01GFNlilluSqZQ5_bGqwAQ2fvqwDYMfa7jLyr5g/edit?usp=sharing" download target="_blank"><DownloadOutlined /> Tải file mẫu</a></div>
            <ProFormSelect name="campaignId" label="Loại" request={apiCampaignOptions} />
            <ProFormUploadDragger
                name={"file"}
                label="Tải file excel lên"
                max={1}
                fieldProps={{
                    beforeUpload: (file) => {
                        const isExcel = file.type === 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
                        if (!isExcel) {
                            message.error('Bạn chỉ có thể tải lên file Excel!');
                        }
                        return isExcel;
                    }
                }}
                description="Chỉ hỗ trợ file định dạng .xlsx"
                rules={[{ required: true, message: 'Vui lòng tải file lên' }]}
                title="Kéo thả file vào khu vực này hoặc nhấp để chọn file"
            />
        </ModalForm>
    )
}

export default VoucherImport;
