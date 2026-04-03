import { apiContactImport } from "@/services/contact";
import { apiGetTypeOfDataBySource, apiSourceOptions } from "@/services/settings/source";
import { apiUserOptions } from "@/services/user";
import { apiTeamOptions } from "@/services/users/team";
import { ModalForm, ModalFormProps, ProFormInstance, ProFormSelect, ProFormText, ProFormUploadDragger } from "@ant-design/pro-components";
import { message } from "antd";
import { useEffect, useRef, useState } from "react";

type Props = ModalFormProps & {
    reload?: () => void;
}

const ContactImport: React.FC<Props> = (props) => {

    const formRef = useRef<ProFormInstance>(null);
    const [sourceOptions, setSourceOptions] = useState<any[]>([]);
    const [teamId, setTeamId] = useState<number>();
    const [sourceId, setSourceId] = useState<number>();
    const [typeOpData, setTypeOpData] = useState<any[]>([]);

    useEffect(() => {
        const fetchSourceOptions = async () => {
            if (teamId) {
                const options = await apiSourceOptions({ teamId });
                setSourceOptions(options);
            }
        }
        fetchSourceOptions();
    }, [teamId]);

    useEffect(() => {
        const fetchTypeOfData = async () => {
            if (sourceId) {
                const typeOfData = await apiGetTypeOfDataBySource(sourceId);
                setTypeOpData(typeOfData.data);
            }
        }
        fetchTypeOfData();
    }, [sourceId]);

    const onFinish = async (values: any) => {
        const formData = new FormData();
        formData.append('file', values.file[0].originFileObj);
        formData.append('sourceId', values.sourceId);
        await apiContactImport(formData);
        message.success('Nhập dữ liệu thành công');
        formRef.current?.resetFields();
        props.reload?.();
        return true;
    }

    return (
        <ModalForm {...props} title="Nhập dữ liệu danh bạ" formRef={formRef} onFinish={onFinish}>
            <div className="mb-2 flex justify-end">
                <a href="https://docs.google.com/spreadsheets/d/1goezVrEivPWb7czSx_-BuDC4qa166X3eGuShz_l9b9U/edit?gid=0#gid=0" target="_blank" rel="noreferrer" className="text-blue-600 underline">Tải mẫu file Excel tại đây</a>
            </div>
            <div className="grid md:grid-cols-2 gap-4">
                <ProFormSelect name="teamId" label="Nhóm" request={apiTeamOptions}
                    onChange={(value: number) => setTeamId(value)}
                    showSearch />
                <ProFormSelect name={"sourceId"} label="Nguồn danh bạ" rules={[{ required: true }]}
                    onChange={(value: number) => setSourceId(value)}
                    disabled={!teamId} options={sourceOptions} showSearch />
                    <ProFormText name="sourceType" label="Source" disabled />
                    <ProFormText name="typeOfData" label="Type Of Data" disabled />
            </div>
            <ProFormSelect name="teleId" label="Belong of Staff" request={apiUserOptions} showSearch />
            <ProFormUploadDragger name={"file"} label="File Excel" rules={[{ required: true }]} max={1} fieldProps={{
                accept: ".xlsx",
                beforeUpload: () => false
            }}
                title={"Kéo thả file vào đây hoặc click để chọn file"}
                description={"Chỉ hỗ trợ file định dạng .xlsx"}
            />
        </ModalForm>
    )
}

export default ContactImport;