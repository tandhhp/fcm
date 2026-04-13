import { apiSourceOptions, apiSourceTransfer } from "@/services/settings/source";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormSelect, ProFormSwitch } from "@ant-design/pro-components";
import { message } from "antd";
import { useEffect, useRef } from "react";

type Props = ModalFormProps & {
    sourceId?: number;
    sourceName?: string;
    reload?: () => void;
}

const TransferModal: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);

    useEffect(() => {
        if (props.sourceId && props.open) {
            formRef.current?.setFieldsValue({
                fromSourceId: props.sourceId,
                includeAssigned: false
            });
        }
    }, [props.sourceId, props.open]);

    const onFinish = async (values: any) => {
        if (!values.fromSourceId) {
            message.error('Vui lòng chọn nguồn gốc');
            return false;
        }
        if (!values.toSourceId) {
            message.error('Vui lòng chọn nguồn đích');
            return false;
        }
        if (values.fromSourceId === values.toSourceId) {
            message.error('Nguồn gốc và nguồn đích không được trùng nhau');
            return false;
        }

        const response = await apiSourceTransfer({
            fromSourceId: values.fromSourceId,
            toSourceId: values.toSourceId,
            includeAssigned: values.includeAssigned || false,
            contactIds: undefined
        });

        if (response.succeeded) {
            message.success(response.data || 'Chuyển nguồn thành công');
            formRef.current?.resetFields();
            props.reload?.();
            return true;
        } else {
            message.error(response.errors?.[0] || 'Chuyển nguồn thất bại');
            return false;
        }
    }

    return (
        <ModalForm 
            {...props} 
            title={`Chuyển contact từ nguồn: ${props.sourceName || ''}`}
            formRef={formRef} 
            onFinish={onFinish}
            width={500}
        >
            <ProFormSelect 
                name="fromSourceId" 
                label="Nguồn gốc (From)" 
                request={apiSourceOptions}
                rules={[{ required: true, message: 'Vui lòng chọn nguồn gốc' }]}
                disabled={!!props.sourceId}
                placeholder="Chọn nguồn gốc"
            />
            
            <ProFormSelect 
                name="toSourceId" 
                label="Nguồn đích (To)" 
                request={async (params) => {
                    const options = await apiSourceOptions(params);
                    // Loại bỏ nguồn gốc khỏi danh sách nguồn đích
                    const fromSourceId = formRef.current?.getFieldValue('fromSourceId');
                    if (fromSourceId) {
                        return options.filter((opt: any) => opt.value !== fromSourceId);
                    }
                    return options;
                }}
                rules={[{ required: true, message: 'Vui lòng chọn nguồn đích' }]}
                placeholder="Chọn nguồn đích"
                showSearch
            />

            <ProFormSwitch 
                name="includeAssigned" 
                label="Bao gồm contact đã phân bổ"
                tooltip="Nếu bật, sẽ chuyển cả những contact đã được gán cho telesales"
                initialValue={false}
            />
        </ModalForm>
    )
}

export default TransferModal;
