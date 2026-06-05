import { apiEventList } from "@/services/event";
import { ActionType, ModalForm, PageContainer, ProFormInstance, ProFormText, ProList } from "@ant-design/pro-components"
import { history, useAccess } from "@umijs/max";
import { useRef, useState } from "react";
import WaitingList from "./components/waitting-list";
import { CalendarOutlined } from "@ant-design/icons";

const EventPage: React.FC = () => {

    const access = useAccess();
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
                renderItem={(item: any) => (
                    <div className="p-4 rounded-lg cursor-pointer hover:shadow bg-white m-2 border overflow-hidden relative" key={item.id} style={{
                        borderColor: `${item.color}`
                    }}
                        onClick={() => {
                            if (access.dot || access.telesaleManager) return;
                            history.push(`/event/time-slot/center/${item.id}`);
                        }}
                    >
                        <div className="text-base font-semibold pb-2 border-b border-dashed mb-1">{item.name}</div>
                        <div className="text-gray-500">Showup hôm nay: <span className="font-bold text-green-500">{item.todayCount}</span></div>
                        <CalendarOutlined className="absolute top-2 right-2 text-gray-300 rotate-45 text-4xl" />
                    </div>
                )}
                onItem={(record) => {
                    return {
                        onClick: () => {
                            if (access.dot || access.telesaleManager) return;
                            history.push(`/event/time-slot/center/${record.id}`);
                        }
                    }
                }}
                size="small"
                rowKey="id"
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