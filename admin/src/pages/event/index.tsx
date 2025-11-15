import { apiEventList } from "@/services/event";
import { ActionType, ModalForm, PageContainer, ProFormInstance, ProFormText, ProList } from "@ant-design/pro-components"
import { history } from "@umijs/max";
import { useRef, useState } from "react";
import WaitingList from "./components/waitting-list";

const EventPage: React.FC = () => {

    const actionRef = useRef<ActionType>();
    const [open, setOpen] = useState<boolean>(false);
    const formRef = useRef<ProFormInstance>();

    return (
        <PageContainer>
            <ProList
                scroll={{
                    x: true
                }}
                ghost
                actionRef={actionRef}
                grid={{ gutter: 16, column: 3, xs: 1, sm: 1, md: 2, lg: 3, xl: 3, xxl: 3 }}
                request={apiEventList}
                metas={{
                    title: {
                        dataIndex: 'name',
                        render: (dom) => {
                            return <div className="text-base font-semibold">{dom}</div>
                        }
                    }
                }}
                onItem={(record) => {
                    return {
                        onClick: () => {
                            history.push(`/event/time-slot/center/${record.id}`);
                        }
                    }
                }}
                size="small"
            />
            <WaitingList />
            <ModalForm title="Sự kiện" open={open} onOpenChange={setOpen} formRef={formRef} disabled>
                <ProFormText name="id" hidden />
                <ProFormText name="name" label="Tên sự kiện" rules={[
                    {
                        required: true
                    }
                ]} />
            </ModalForm>
        </PageContainer>
    )
}

export default EventPage;