import { ProFormSelect, ProFormSelectProps } from "@ant-design/pro-components";

const ItemPerRowForm: React.FC<ProFormSelectProps> = (prop) => {
    return (
        <ProFormSelect label="Item per row" name="itemPerRow"
            options={[
                {
                    label: 'Desktop 2 - Mobile 2',
                    value: 'col-6'
                },
                {
                    label: 'Desktop 6 - Mobile 2',
                    value: 'col-6 col-md-2'
                },
                {
                    label: 'Desktop 4 - Mobile 2',
                    value: 'col-6 col-md-3'
                }
            ]}
            {...prop} />
    )
}

export default ItemPerRowForm;