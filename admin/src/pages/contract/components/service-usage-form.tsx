import { apiContractServiceUsageCreate, apiContractServiceUsageUpdate } from "@/services/finances/contract-service-usage";
import { ModalForm, ProFormDatePicker, ProFormDigit, ProFormText } from "@ant-design/pro-components";
import { message } from "antd";
import { useEffect } from "react";

interface ServiceUsageFormProps {
    open: boolean;
    onOpenChange: (open: boolean) => void;
    data: any;
    record: API.ContractServiceUsage | null;
    reload: () => void;
}

const ServiceUsageForm: React.FC<ServiceUsageFormProps> = ({ open, onOpenChange, data, record, reload }) => {

    const onFinish = async (values: any) => {
        const payload = {
            ...values,
            contractId: data?.id,
            id: record?.id
        };

        if (record?.id) {
            await apiContractServiceUsageUpdate(payload);
            message.success('Cập nhật phiếu sử dụng dịch vụ thành công');
        } else {
            await apiContractServiceUsageCreate(payload);
            message.success('Tạo phiếu sử dụng dịch vụ thành công');
        }
        reload();
        return true;
    }

    return (
        <ModalForm
            title={record?.id ? "Cập nhật phiếu sử dụng dịch vụ" : "Tạo phiếu sử dụng dịch vụ"}
            open={open}
            onOpenChange={onOpenChange}
            onFinish={onFinish}
            modalProps={{
                destroyOnClose: true,
            }}
            initialValues={record || {}}
        >
            <ProFormText
                name="serviceName"
                label="Loại dịch vụ"
                rules={[{ required: true, message: 'Vui lòng nhập loại dịch vụ' }]}
                placeholder="Nhập loại dịch vụ"
            />
            <ProFormDatePicker
                name="usedDate"
                label="Ngày sử dụng"
                rules={[{ required: true, message: 'Vui lòng chọn ngày sử dụng' }]}
                fieldProps={{
                    style: { width: '100%' }
                }}
            />
            <ProFormDigit
                name="amount"
                label="Số tiền"
                rules={[{ required: true, message: 'Vui lòng nhập số tiền' }]}
                placeholder="Nhập số tiền"
                fieldProps={{
                    min: 1
                }}
            />
            <ProFormDigit
                name="peopleCount"
                label="Số người"
                rules={[{ required: true, message: 'Vui lòng nhập số người' }]}
                placeholder="Nhập số người"
                fieldProps={{
                    min: 1
                }}
            />
        </ModalForm>
    );
}

export default ServiceUsageForm;
