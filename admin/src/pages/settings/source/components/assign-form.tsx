import { apiTelesalesManagerOptions, apiTelesalesOptions } from "@/services/role";
import { apiSourceOptions, apiSourceAssign, apiAvailableSource } from "@/services/settings/source";
import { apiContactAssignSource } from "@/services/contact";
import { UserSwitchOutlined } from "@ant-design/icons";
import { ModalForm, ModalFormProps, ProFormDigit, ProFormSelect, ProFormCheckbox, ProFormInstance } from "@ant-design/pro-components"
import { Button, message } from "antd";
import { useEffect, useRef, useState } from "react";
import { useRequest } from "@umijs/max";

type Props = ModalFormProps & {
    reload?: () => void;
}

const AssignForm: React.FC<Props> = (props) => {

    const { data: sourceData } = useRequest(apiAvailableSource);
    const [teamData, setTeamData] = useState<any[]>([]);
    const [tmId, setTmId] = useState<string | null>(null);
    const [teleData, setTeleData] = useState<any[]>([]);
    const formRef = useRef<ProFormInstance>();

    useEffect(() => {
        apiTelesalesManagerOptions().then(res => {
            setTeamData(res);
        });
    }, []);

    useEffect(() => {
        if (tmId) {
            apiTelesalesOptions({ telesalesManagerId: tmId }).then(res => {
                setTeleData(res);
            });
        } else {
            setTeleData([]);
        }
    }, [tmId]);

    const onFinish = async (values: any) => {
        const { tmId: formTmId, sources = [], ...rest } = values || {};
        if (!sources.length) {
            message.warning('Vui lòng chọn nguồn cần phân bổ');
            return false;
        }
        // Build assignments from dynamic digit fields assignCount_<teleId>
        const assigns = Object.keys(rest)
            .filter(k => k.startsWith('assignCount_'))
            .map(k => ({ telesalesId: k.replace('assignCount_', ''), numberOfContact: rest[k] }))
            .filter(item => !!item.numberOfContact);

        if (!assigns.length) {
            message.warning('Vui lòng nhập số lượng phân bổ cho ít nhất một telesales');
            return false;
        }

        // Payload for bulk source assignment
        const payload = {
            tmId: formTmId,
            sources,
            assigns
        };

        try {
            await apiSourceAssign(payload);
            console.log(payload);
            message.success('Phân bổ nguồn danh bạ thành công');
            formRef.current?.resetFields();
            props.reload?.();
            return true;
        } catch (e) {
            message.error('Phân bổ thất bại');
            return false;
        }
    };

    return (
        <ModalForm {...props} title="Phân bổ nguồn danh bạ"
            formRef={formRef}
            onFinish={onFinish}
            trigger={<Button type="primary" icon={<UserSwitchOutlined />}>Phân bổ</Button>}
        >
            <ProFormCheckbox.Group name="sources" label="Nguồn" options={sourceData?.map((item: any) => (
                {
                    label: `${item.label} - (${item.contactCount})`
                    , value: item.value
                }))}
                rules={[{ required: true, message: 'Vui lòng chọn ít nhất một nguồn' }]} />
            <ProFormSelect name="tmId" label="Chọn team telesales" placeholder="Chọn team telesales"
                fieldProps={{
                    options: teamData,
                    onChange: (value: string) => setTmId(value)
                }}
            />
            {
                teleData && (
                    <div>
                        {teleData.map(tele => (
                            <div className="flex gap-4" key={tele.id}>
                                <div className="flex-1">{tele.label}</div>
                                <ProFormDigit name={`assignCount_${tele.value}`} label="Số lượng phân bổ" placeholder="Nhập số lượng phân bổ" />
                            </div>
                        ))}
                    </div>
                )
            }
        </ModalForm>
    )
}

export default AssignForm;