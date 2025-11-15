import { getSetting, querySaveSetting, queryThemeOptions } from "@/services/setting";
import { ProForm, ProFormInstance, ProFormSelect } from "@ant-design/pro-components";
import { message } from "antd";
import { useEffect, useRef } from "react";

const ChangeTheme: React.FC = () => {

    const formRef = useRef<ProFormInstance>();

    useEffect(() => {
        getSetting('theme').then(response => {
            if (!response) {
                formRef.current?.setFieldValue('name', 'default');
                return;
            }
            formRef.current?.setFieldValue('name', response.name);
        })
    }, []);

    const onChange = async (value: string) => {
        const response = await querySaveSetting('theme', {
            name: value
        });
        if (response.succeeded) {
            message.success('Saved!');
        } else {
            message.error(response.errors[0].description);
        }
    }

    return (
        <>
            <ProForm layout="inline" submitter={false} formRef={formRef}>
                <ProFormSelect
                    showSearch
                    request={queryThemeOptions}
                    fieldProps={{
                        onChange: onChange
                    }}
                    label="Change Theme" name='name' />
            </ProForm>
        </>
    )
}

export default ChangeTheme;