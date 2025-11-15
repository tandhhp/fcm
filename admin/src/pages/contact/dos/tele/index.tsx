import { apiTelesalesOptions } from "@/services/role";
import { apiUserListTele, apiUserSetDos } from "@/services/user";
import { LeftOutlined, UserAddOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProFormSelect, ProTable } from "@ant-design/pro-components"
import { history, useParams } from "@umijs/max";
import { Button, message } from "antd";
import { useRef, useState } from "react";

const Index: React.FC = () => {

    const { id } = useParams();
    const [open, setOpen] = useState(false);
    const actionRef = useRef<ActionType>();

    return (
        <PageContainer extra={<Button icon={<LeftOutlined />} onClick={() => history.back()}>Quay lại</Button>}>

            <ProTable
                actionRef={actionRef}
                headerTitle={<Button icon={<UserAddOutlined />} type="primary" onClick={() => setOpen(true)}>Thêm mới</Button>}
                request={apiUserListTele}
                params={{
                    dosId: id
                }}
                rowKey={"id"}
                columns={[
                    {
                        title: '#',
                        valueType: 'indexBorder',
                        width: 30
                    },
                    {
                        title: 'Tên',
                        dataIndex: 'name'
                    },
                    {
                        title: 'Email',
                        dataIndex: 'email'
                    },
                    {
                        title: 'SĐT',
                        dataIndex: 'phoneNumber'
                    }
                ]}
                search={false}
            />
            <ModalForm title="Thêm mới Tele" open={open} onOpenChange={setOpen} modalProps={{ destroyOnClose: true }} onFinish={async (values) => {
                await apiUserSetDos({
                    dosId: id,
                    teleId: values.teleId
                });
                message.success("Thêm mới thành công");
                setOpen(false);
                actionRef.current?.reload();
            }}>
                <ProFormSelect name="teleId" label="Chọn Tele" request={apiTelesalesOptions} rules={[
                    {
                        required: true
                    }
                ]} />
            </ModalForm>
        </PageContainer>
    )
}

export default Index;