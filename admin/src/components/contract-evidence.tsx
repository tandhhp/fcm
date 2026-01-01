import { apiContractEvidenceDelete, apiContractEvidenceList, apiContractEvidenceTypeOptions, apiContractEvidenceUpload, apiContractInvoiceOptions } from "@/services/finances/contract";
import { DeleteOutlined, DownloadOutlined } from "@ant-design/icons";
import { DrawerForm, DrawerFormProps, ProFormInstance, ProFormSelect, ProFormUploadButton } from "@ant-design/pro-components"
import { Button, Empty, Image, message, Popconfirm } from "antd";
import { RcFile } from "antd/lib/upload";
import { useEffect, useRef, useState } from "react";

type Props = DrawerFormProps & {
    contractId?: string;
}

const ContractEvidence: React.FC<Props> = ({ contractId, ...rest }) => {

    const [data, setData] = useState<{
        key: string;
        evidences: {
            id: string;
            url: string;
            name: string;
            uploadedAt: string;
        }[]
    }[]>([]);

    const [evidenceTypeId, setEvidenceTypeId] = useState<number>();
    const [invoiceId, setInvoiceId] = useState<string>();
    const formRef = useRef<ProFormInstance>(null);

    const fetchData = () => {
        if (!contractId) {
            return;
        }
        apiContractEvidenceList(contractId).then((response) => {
            setData(response.data || []);
        })
    };

    useEffect(() => {
        if (contractId) {
            fetchData();
        }
    }, [contractId]);

    const onUpload = async (file: RcFile, fileList: RcFile[]) => {
        if (!evidenceTypeId) {
            message.error('Vui lòng chọn loại chứng từ trước khi tải lên');
            return false;
        }
        if (!contractId) {
            message.error('Không tìm thấy hợp đồng');
            return false;
        }
        if (fileList.length === 0) {
            message.error('Vui lòng chọn file để tải lên');
            return false;
        }
        const formData = new FormData();
        formData.append('contractId', contractId);
        formData.append('evidenceTypeId', evidenceTypeId.toString());
        if (invoiceId) {
            formData.append('invoiceId', invoiceId);
        }
        fileList.forEach((file) => {
            formData.append('files', file);
        });
        await apiContractEvidenceUpload(formData);
        message.success('Tải lên thành công');
        setEvidenceTypeId(undefined);
        setInvoiceId(undefined);
        fetchData();
        return false;
    }

    return (
        <DrawerForm formRef={formRef}
            {...rest}
            title="Thư viện ảnh hợp đồng"
            drawerProps={{
                destroyOnHidden: true
            }}
            submitter={false}
        >
            <div className="mb-4 flex gap-4">
                <div className="flex-1">
                    <ProFormSelect
                        onChange={(value: number) => {
                            setEvidenceTypeId(value);
                            setInvoiceId(undefined);
                            formRef.current?.setFieldValue('invoiceId', undefined);
                        }}
                        name="evidenceTypeId" label="Loại chứng từ" request={apiContractEvidenceTypeOptions} allowClear={false} rules={[
                            {
                                required: true
                            }
                        ]} />
                </div>
                <div className="flex-1">
                    <ProFormSelect
                        disabled={evidenceTypeId !== 3}
                        onChange={(value: string) => setInvoiceId(value)}
                        name="invoiceId" label="Chứng từ liên quan" request={apiContractInvoiceOptions}
                        params={{
                            contractId
                        }}
                        showSearch />
                </div>
                <ProFormUploadButton
                    disabled={!evidenceTypeId}
                    name={"file"}
                    fieldProps={{
                        beforeUpload: onUpload
                    }}
                    accept=".png,.jpg,.jpeg"
                    className="w-full" title="Tải lên ảnh chứng từ" label="Tải lên ảnh chứng từ" />
            </div>
            {
                data.length === 0 ? (
                    <div>
                        <Empty description="Chưa có ảnh chứng từ" />
                    </div>
                ) : (
                    data.map((group) => (
                        <div key={group.key} style={{ marginBottom: 24 }}>
                            <div className="border">
                                <h3 className="px-2 py-1 bg-slate-100 font-bold">{group.key}</h3>
                                <div style={{ display: 'flex', flexWrap: 'wrap', gap: 16 }} className="p-1">
                                    {
                                        group.evidences.map((evidence) => (
                                            <div key={evidence.id} className="flex flex-col gap-2">
                                                <div className="border p-1 w-32 h-32">
                                                    <Image src={evidence.url} alt={evidence.name} className="object-cover" style={{
                                                        width: 120,
                                                        height: 120
                                                    }} />
                                                    <div style={{ marginTop: 8 }}>{evidence.name}</div>
                                                </div>
                                                <div className="flex gap-2 justify-center">
                                                    <Button icon={<DownloadOutlined />} size="small" href={evidence.url} target="_blank" rel="noreferrer" className="text-blue-600 underline text-xs text-center">
                                                    </Button>
                                                    <Popconfirm title="Bạn có chắc chắn muốn xóa ảnh này?" onConfirm={async () => {
                                                        await apiContractEvidenceDelete(evidence.id);
                                                        message.success('Xóa ảnh thành công');
                                                        fetchData();
                                                    }} okText="Xóa" cancelText="Hủy">
                                                        <Button danger size="small" icon={<DeleteOutlined />}>
                                                        </Button>
                                                    </Popconfirm>                                                    
                                                </div>
                                            </div>
                                        ))
                                    }
                                </div>
                            </div>
                        </div>
                    ))
                )
            }
        </DrawerForm>
    )
}

export default ContractEvidence;