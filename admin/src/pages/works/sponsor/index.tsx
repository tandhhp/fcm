import WorkSummary from "@/components/works/summary";
import { getArguments, saveArguments } from "@/services/work-content";
import { uuidv4 } from "@/utils/common";
import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";
import { DragSortTable, ModalForm, PageContainer, ProColumns, ProFormText } from "@ant-design/pro-components"
import { FormattedMessage, useParams } from "@umijs/max";
import { Avatar, Button, Col, Popconfirm, Row, Space, message } from "antd";
import { useEffect, useState } from "react"

const Sponsor: React.FC = () => {

    const { id } = useParams();
    const [brands, setBrands] = useState<CPN.Brand[]>([]);
    const [open, setOpen] = useState<boolean>(false);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        if (id) {
            getArguments(id).then(response => {
                setBrands(response.brands || []);
                setLoading(false);
            })
        }
    }, [id]);

    const onFinish = async (values: CPN.Brand) => {
        let newBrands: CPN.Brand[] = brands;
        if (values.id) {
            const objIndex = newBrands.findIndex((obj => obj.id === values.id));
            newBrands[objIndex].name = values.name;
            newBrands[objIndex].url = values.url;
            newBrands[objIndex].logo = values.logo;
        } else {
            values.id = uuidv4();
            if (!newBrands) {
                newBrands = [];
            }
            newBrands.push(values);
        }
        const body = {
            brands: newBrands
        }
        const response = await saveArguments(id, body);
        if (response.succeeded) {
            setBrands(newBrands);
            message.success('Saved!');
            setOpen(false);
        }
    }

    const remove = async (brand: CPN.Brand) => {
        const newBrands = brands.filter(x => x.id !== brand.id);
        const body = {
            brands: newBrands
        }
        const response = await saveArguments(id, body);
        if (response.succeeded) {
            setBrands(newBrands);
            message.success('Deleted!');
        }
    }

    const columns: ProColumns<CPN.Brand>[] = [
        {
            title: '#',
            dataIndex: 'sort',
            className: 'drag-visible',
            search: false
        },
        {
            title: 'Name',
            dataIndex: 'name',
            render: (dom, entity) => (
                <Space>
                    <Avatar src={entity.logo} />
                    {dom}
                </Space>
            )
        },
        {
            title: 'Url',
            dataIndex: 'url'
        },
        {
            title: 'Action',
            valueType: 'option',
            render: (dom, entity) => [
                <Popconfirm
                    title="Are you sure?"
                    key={4}
                    onConfirm={() => remove(entity)}
                >
                    <Button
                        icon={<DeleteOutlined />}
                        danger
                        type="primary"
                    ></Button>
                </Popconfirm>
            ]
        }
    ];

    const handleDragSortEnd = async (newDataSource: CPN.Brand[]) => {
        const body = {
            brands: newDataSource
        }
        const response = await saveArguments(id, body);
        if (response.succeeded) {
            setBrands(newDataSource);
            message.success('Saved!');
        }
    };

    return (
        <PageContainer>
            <Row gutter={16}>
                <Col span={16}>
                    <DragSortTable
                        toolBarRender={() => [
                            <Button icon={<PlusOutlined />} key="add" type="link" onClick={() => setOpen(true)} />
                        ]}
                        dataSource={brands}
                        columns={columns}
                        search={{
                            layout: "vertical"
                        }}
                        rowKey="id"
                        dragSortKey="sort"
                        loading={loading}
                        onDragSortEnd={handleDragSortEnd}
                    />
                </Col>
                <Col span={8}>
                    <WorkSummary />
                </Col>
            </Row>
            <ModalForm onFinish={onFinish} open={open} onOpenChange={setOpen} title={<FormattedMessage id="menu.component.sponsor" />}>
                <ProFormText name="id" label="Id" disabled />
                <ProFormText name="name" label="Name" />
                <ProFormText name="logo" label="Logo" />
                <ProFormText name="url" label="Url" />
            </ModalForm>

        </PageContainer>
    )
}

export default Sponsor