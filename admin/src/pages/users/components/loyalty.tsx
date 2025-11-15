import { ModalForm, ModalFormProps, ProFormDigit, ProFormSelect, ProFormTextArea } from "@ant-design/pro-components"
import { Alert, Col, Row } from "antd"

const OpenLoyalty: React.FC<ModalFormProps> = (props) => {
    return (
        <ModalForm {...props}>
            <Alert
                showIcon
                closable
                message={(
                    <div>
                        - Sau khi nạp điểm cần phê duyệt của kế toán<br />
                        - Nhập số âm nếu cần trừ điểm của khách
                    </div>
                )}
                className="mb-4"
            />
            <Row gutter={16}>
                <Col span={18}>
                    <ProFormDigit fieldProps={{
                        min: -1000000,
                        formatter: (value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ','),
                        parser: (value) => value?.replace(/(,*)/g, '') as unknown as number
                    }} className="w-full" label="Số điểm" name="point" rules={[
                        {
                            required: true,
                            message: 'Vui lòng nhập điểm muốn nạp'
                        }
                    ]} />
                </Col>
                <Col span={6}>
                    <ProFormSelect name="type" label="Loại" fieldProps={{
                        defaultValue: 0
                    }} options={[
                        {
                            label: 'Điểm NP',
                            value: 0
                        },
                        {
                            label: 'Điểm thưởng NP',
                            value: 1
                        }
                    ]} />
                </Col>
            </Row>
            <ProFormTextArea label="Ghi chú" name="memo" rules={[
                {
                    required: true
                }
            ]} />
        </ModalForm>
    )
}

export default OpenLoyalty