import { apiAddTransFeedback } from "@/services/comment";
import { apiGetMyTransactions } from "@/services/user";
import { CommentOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, ProColumns, ProFormInstance, ProFormText, ProFormTextArea, ProTable } from "@ant-design/pro-components";
import { useParams } from "@umijs/max";
import { Button, message } from "antd";
import { useEffect, useRef, useState } from "react";

const TransactionTab: React.FC = () => {

    const { id } = useParams();
    const [open, setOpen] = useState<boolean>(false);
    const formRef = useRef<ProFormInstance>();
    const actionRef = useRef<ActionType>();
    const [trans, setTrans] = useState<any>();

    useEffect(() => {

        formRef.current?.setFields([
            {
                name: 'id',
                value: trans?.id
            },
            {
                name: 'feedback',
                value: trans?.feedback
            }
        ]);
    }, [trans])

    const columns: ProColumns<any>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
            width: 50,
            align: 'center'
        },
        {
            title: 'Ngày',
            dataIndex: 'createdDate',
            valueType: 'dateTime',
            search: false,
            width: 180
        },
        {
            title: 'Điểm',
            dataIndex: 'point',
            valueType: 'digit',
            search: false,
            width: 100
        },
        {
            title: 'Ghi chú',
            dataIndex: 'memo'
        },
        {
            title: 'Feedback của khách',
            dataIndex: 'feedback'
        },
        {
            title: 'Tác vụ',
            valueType: 'option',
            render: (_, entity) => [
                <Button type="primary" size="small" icon={<CommentOutlined />} key="feedback" onClick={() => {
                    setTrans(entity);
                    setOpen(true);
                }}></Button>
            ],
            width: 60
        }
    ]

    return (
        <>
            <ProTable
                actionRef={actionRef}
                ghost
                search={false}
                request={() => apiGetMyTransactions(id)}
                columns={columns}
            />
            <ModalForm open={open} onOpenChange={setOpen} title="Phản ứng của khách hàng" formRef={formRef} onFinish={async (values) => {
                await apiAddTransFeedback(values);
                message.success('Thành công!');
                setOpen(false);
                actionRef.current?.reload();
            }}>
                <ProFormText name="id" hidden />
                <ProFormTextArea name="feedback" label="Nội dung"></ProFormTextArea>
            </ModalForm>
        </>
    )
}

export default TransactionTab;