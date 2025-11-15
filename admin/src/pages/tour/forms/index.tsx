import { CatalogType } from "@/constants";
import FormDetail from "@/pages/form/detail";
import { queryCatalogSelect } from "@/services/catalog";
import { apiCxCreateForm, apiDeleteForm, apiFormList, apiResendEmail, apiTourUpdateStatus } from "@/services/form";
import { apiBranchOptions } from "@/services/settings/branch";
import { apiCardHolderOptions } from "@/services/user";
import { TourFormStatus } from "@/utils/status";
import { ArrowRightOutlined, CheckOutlined, CloseOutlined, DeleteOutlined, DoubleRightOutlined, MailOutlined, PhoneOutlined, PlusOutlined } from "@ant-design/icons";
import { ActionType, ModalForm, PageContainer, ProColumnType, ProFormDigit, ProFormInstance, ProFormSelect, ProFormText, ProTable } from "@ant-design/pro-components";
import { useAccess } from "@umijs/max";
import { Button, Popconfirm, Tag, Tooltip, message } from "antd";
import { useEffect, useRef, useState } from "react";

const TourFormPage: React.FC = () => {

    const actionRef = useRef<ActionType>();
    const [openDetail, setOpenDetail] = useState<boolean>(false);
    const [form, setForm] = useState<any>();
    const access = useAccess();
    const [open, setOpen] = useState<boolean>(false);
    const [catalogOptions, setCatalogOptions] = useState<any>();
    const [catalogType, setCatalogType] = useState<CatalogType>();
    const [parentId, setParentId] = useState<string>();
    const [parentOptions, setParentOptions] = useState<any>();
    const formRef = useRef<ProFormInstance>();

    const updateStatus = async (row: any) => {
        const response = await apiTourUpdateStatus(row);
        if (response.succeeded) {
            message.success('Cập nhật trạng thái thành công!');
            actionRef.current?.reload();
        }
    }

    useEffect(() => {
        if (catalogType) {
            queryCatalogSelect({
                type: parentId ? null : catalogType,
                parentId
            }).then(response => setCatalogOptions(response));
        }
    }, [catalogType, parentId]);

    useEffect(() => {
        if (catalogType) {
            queryCatalogSelect({
                type: catalogType
            }).then(response => setParentOptions(response));
        }
    }, [catalogType]);

    const columns: ProColumnType<any>[] = [
        {
            title: '#',
            width: 30,
            valueType: 'indexBorder'
        },
        {
            title: 'Đơn đăng ký',
            dataIndex: 'tourName',
            render: (dom, entity) => {
                return (
                    <div className="cursor-pointer hover:text-blue-500" onClick={() => {
                        setForm(entity);
                        setOpenDetail(true);
                    }}>{dom}</div>
                )
            }

        },
        {
            title: 'Mã HĐ',
            dataIndex: 'contractCode',
            width: 80
        },
        {
            title: 'Người đăng ký',
            dataIndex: 'fullName',
            width: 180

        },
        {
            title: 'Liên hệ',
            dataIndex: 'email',
            render: (dom, entity) => {
                return (
                    <div>
                        <div className="flex gap-1"><MailOutlined /> <Tooltip title={entity.email}><div className="w-32 truncate">{entity.email}</div></Tooltip></div>
                        <div><PhoneOutlined /> {entity.phoneNumber}</div>
                    </div>
                )
            }
        },
        {
            title: 'Điện thoại',
            dataIndex: 'phoneNumber',
            hideInTable: true
        },
        {
            title: 'Ngày đăng ký',
            dataIndex: 'createdDate',
            valueType: 'dateTime',
            search: false,
            width: 170
        },
        {
            title: 'Chi nhánh',
            dataIndex: 'branchId',
            search: false,
            valueType: 'select',
            width: 80,
            request: apiBranchOptions
        },
        {
            title: 'Loại',
            dataIndex: 'type',
            render: (dom, entity) => {
                if (entity.type === CatalogType.Healthcare) {
                    return <Tag className="w-full text-center" color="blue">Gói khám</Tag>
                }
                if (entity.type === CatalogType.Tour) {
                    return <Tag className="w-full text-center" color="green">Nghỉ dưỡng</Tag>
                }
                if (entity.type === CatalogType.Product) {
                    return <Tag className="w-full text-center" color="red">Sản phẩm</Tag>
                }
                if (entity.type === CatalogType.Room) {
                    return <Tag className="w-full text-center" color="orange">Khách sạn</Tag>
                }
                if (entity.type === CatalogType.Special) {
                    return <Tag className="w-full text-center" color="black">Đặc biệt</Tag>
                }
                return '-';
            },
            width: 100,
            valueEnum: {
                15: {
                    text: 'Gói khám'
                },
                14: {
                    text: 'Nghỉ dưỡng'
                },
                2: {
                    text: 'Sản phẩm'
                },
                17: {
                    text: 'Danh sách khách sạn'
                },
            }
        },
        {
            title: 'Điểm',
            dataIndex: 'point',
            valueType: 'digit',
            search: false
        },
        {
            title: 'Trạng Thái',
            dataIndex: 'status',
            valueEnum: {
                0: {
                    text: 'Mới',
                    status: 'Default'
                },
                1: {
                    text: 'Đang xử lý',
                    status: 'Processing'
                },
                2: {
                    text: 'Hoàn thành',
                    status: 'Success'
                },
                3: {
                    text: 'Đã hủy',
                    status: 'Error'
                },
                4: {
                    text: 'KT xác nhận',
                    status: 'Processing'
                }
            },
            width: 120
        },
        {
            title: 'Tác vụ',
            valueType: 'option',
            render: (dom, row) => [
                <Tooltip key="processing" title={`Chuyển trạng thái sang đang xử lý`}>
                    <Popconfirm title="Xác nhận chuyển trạng thái" onConfirm={() => {
                        row.status = TourFormStatus.InProgress;
                        updateStatus(row);
                    }}>
                        <Button type="primary" size="small" icon={<ArrowRightOutlined />}
                            hidden={row.status === TourFormStatus.InProgress || row.status === TourFormStatus.Completed
                                || row.status === TourFormStatus.AccountantApproved
                                || row.status === TourFormStatus.Canceled
                            } />
                    </Popconfirm>
                </Tooltip>,
                <Tooltip key="processing" title={`Chuyển trạng thái sang hoàn thành`}>
                    <Popconfirm title="Xác nhận chuyển trạng thái" onConfirm={() => {
                        row.status = TourFormStatus.Completed;
                        updateStatus(row);
                    }}>
                        <Button type="primary" size="small" icon={<DoubleRightOutlined />}
                            hidden={row.status !== TourFormStatus.AccountantApproved || access.canAccountant} />
                    </Popconfirm>
                </Tooltip>,
                <Tooltip key="accountant" title={`Kế toán xác nhận`}>
                    <Popconfirm title="Xác nhận chuyển trạng thái" onConfirm={() => {
                        if (row.status === TourFormStatus.New) {
                            row.status = TourFormStatus.InProgress;
                        } else if (row.status === TourFormStatus.InProgress) {
                        }
                        row.status = TourFormStatus.AccountantApproved;
                        updateStatus(row);
                    }}>
                        <Button type="primary" size="small" icon={<CheckOutlined />}
                            hidden={row.status !== TourFormStatus.InProgress || !access.canAccountant} />
                    </Popconfirm>
                </Tooltip>,
                <Tooltip key="cancel" title="Hủy đơn đăng ký">
                    <Popconfirm title="Bạn có chắc chắn muốn hủy đơn đăng ký này?" onConfirm={() => {
                        row.status = TourFormStatus.Canceled;
                        updateStatus(row);
                    }}>
                        <Button size="small" danger icon={<CloseOutlined />}
                            hidden={row.status === TourFormStatus.Completed || row.status === TourFormStatus.Canceled || access.canAccountant || row.status === TourFormStatus.AccountantApproved} />
                    </Popconfirm>
                </Tooltip>,
                <Popconfirm key="email" title="Xác nhận gửi lại email" onConfirm={async () => {
                    await apiResendEmail(row.id);
                    message.success('Gửi thành công!');
                }}>
                    <Button size="small" type="primary" icon={<MailOutlined />} hidden={(!access.canCX && !access.canCXM) || row.status === TourFormStatus.Canceled} />
                </Popconfirm>,
                <Tooltip key="delete" title="Xóa đơn đăng ký">
                    <Popconfirm title="Bạn có chắc chắn muốn xóa đơn đăng ký này?" onConfirm={() => {
                        apiDeleteForm(row.id).then(() => {
                            message.success('Xóa thành công!');
                            actionRef.current?.reload();
                        })
                    }}>
                        <Button type="primary" size="small" danger icon={<DeleteOutlined />} hidden={!access.canAdmin} />
                    </Popconfirm>
                </Tooltip>
            ],
            width: 60,
            align: 'center'
        }
    ];

    return (
        <PageContainer extra={<Button type="primary" icon={<PlusOutlined />} hidden={!access.canCX} onClick={() => setOpen(true)}>Tạo đơn đăng ký</Button>}>

            <ProTable
                scroll={{
                    x: true
                }}
                actionRef={actionRef}
                search={{
                    layout: 'vertical'
                }}
                columns={columns}
                request={apiFormList}
            >

            </ProTable>
            <FormDetail open={openDetail} setOpen={setOpenDetail} formId={form?.id} />
            <ModalForm open={open} onOpenChange={setOpen}
                formRef={formRef}
                title="Tạo đơn đăng ký thủ công" onFinish={async (values) => {
                    await apiCxCreateForm(values);
                    message.success('Tạo thành công!');
                    actionRef.current?.reload();
                    setOpen(false);
                }}>
                <ProFormSelect
                    showSearch
                    name="type"
                    fieldProps={{
                        onChange: (value: any) => {
                            setCatalogType(value);
                            formRef.current?.setFieldValue('catalogId', '');
                            formRef.current?.setFieldValue('parentId', '');
                            setParentId('');
                        },
                    }}
                    label="Loại sản phẩm / dịch vụ" options={[
                        {
                            label: 'Chăm sóc sức khỏe',
                            value: CatalogType.Product
                        },
                        {
                            label: 'Dưỡng sinh độc bản',
                            value: CatalogType.Tour
                        },
                        {
                            label: 'Cơ sở khám',
                            value: CatalogType.Hospital
                        },
                        {
                            label: 'Khách sạn',
                            value: CatalogType.Room
                        },
                        {
                            label: 'Đặc biệt',
                            value: CatalogType.Special
                        }
                    ]}
                    rules={[
                        {
                            required: true
                        }
                    ]} />
                {
                    catalogType !== CatalogType.Special && (
                        <>
                            <ProFormSelect
                                showSearch
                                hidden={(catalogType === CatalogType.Product || catalogType === CatalogType.Room)}
                                label="Chọn sản phẩm / dịch vụ" name="parentId" options={parentOptions}
                                fieldProps={{
                                    onChange: (value: any) => {
                                        setParentId(value);
                                    },
                                }} />
                            <ProFormSelect
                                showSearch
                                label={(catalogType === CatalogType.Product || catalogType === CatalogType.Room) ? 'Chọn sản phẩm / dịch vụ' : 'Chọn gói'}
                                name="catalogId" options={catalogOptions} rules={[
                                    {
                                        required: true
                                    }
                                ]} />
                        </>
                    )
                }
                {
                    catalogType === CatalogType.Special && (
                        <>
                            <ProFormText label="Tên sản phẩm - dịch vụ" name="name" />
                            <ProFormDigit label="Điểm" name="point"
                                fieldProps={{
                                    formatter: (value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ','),
                                    parser: (value) => value?.replace(/(,*)/g, '') as unknown as number
                                }} />
                        </>
                    )
                }
                <ProFormSelect label="Chọn chủ thẻ"
                    showSearch
                    name="cardHolderId" request={apiCardHolderOptions} rules={[
                        {
                            required: true
                        }
                    ]} />
            </ModalForm>
        </PageContainer>
    )
}

export default TourFormPage;