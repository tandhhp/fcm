import { apiContractLeaveVoucherUsageCreate, apiContractLeaveVoucherUsageUpdate } from "@/services/finances/contract-leave-voucher-usage";
import { ModalForm, ProFormDatePicker, ProFormDigit, ProFormText } from "@ant-design/pro-components";
import { message } from "antd";

interface LeaveVoucherUsageFormProps {
    open: boolean;
    onOpenChange: (open: boolean) => void;
    data: any;
    record: API.ContractLeaveVoucherUsage | null;
    reload: () => void;
}

const LeaveVoucherUsageForm: React.FC<LeaveVoucherUsageFormProps> = ({ open, onOpenChange, data, record, reload }) => {

    const onFinish = async (values: any) => {
        const payload = {
            ...values,
            contractId: data?.id,
            id: record?.id
        };

        if (record?.id) {
            await apiContractLeaveVoucherUsageUpdate(payload);
            message.success('Cập nhật phiếu sử dụng quyền nghỉ thành công');
        } else {
            await apiContractLeaveVoucherUsageCreate(payload);
            message.success('Tạo phiếu sử dụng quyền nghỉ thành công');
        }
        reload();
        return true;
    }

    return (
        <ModalForm
            title={record?.id ? "Cập nhật phiếu sử dụng quyền nghỉ" : "Tạo phiếu sử dụng quyền nghỉ"}
            open={open}
            onOpenChange={onOpenChange}
            onFinish={onFinish}
            modalProps={{
                destroyOnClose: true,
            }}
            initialValues={record || {}}
        >
            <ProFormText
                name="voucherName"
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

export default LeaveVoucherUsageForm;
