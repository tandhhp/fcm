import { apiCardOptions, apiSalesOptions, apiSmOptions, apiUpdateContractCode, apiUpdateLoyalty, deleteUser } from '@/services/user';
import { ArrowRightOutlined, AuditOutlined, DeleteOutlined, EditOutlined, EyeOutlined, FundOutlined, ManOutlined, MoneyCollectOutlined, MoreOutlined, SendOutlined, UserAddOutlined, WomanOutlined } from '@ant-design/icons';
import {
    ActionType,
    ModalForm,
    PageContainer,
    ProColumns,
    ProFormInstance,
    ProFormText,
    ProTable,
} from '@ant-design/pro-components';
import { FormattedMessage, history } from '@umijs/max';
import { Button, Dropdown, message, Popconfirm, Space, Tag, Tooltip } from 'antd';
import { useEffect, useRef, useState } from 'react';
import OpenLoyalty from '../components/loyalty';
import { useAccess } from '@umijs/max';
import SendEmailComponent from '../components/send-email';
import TopUpModal from '../components/top-up';
import CardHolderForm from './components/form';
import dayjs from 'dayjs';
import PointDrawer from './components/point';
import LoanPointModal from './components/loan-point';
import { apiBranchOptions } from '@/services/settings/branch';

const CardHolderPage: React.FC = () => {
    const actionRef = useRef<ActionType>();
    const [open, setOpen] = useState<boolean>(false);
    const [user, setUser] = useState<any>();
    const [openLoyalty, setOpenLoyalty] = useState<boolean>(false);
    const access = useAccess();
    const salesForm = useRef<ProFormInstance>();
    const [openEmail, setOpenEmail] = useState<boolean>(false);
    const [openTopUp, setOpenTopUp] = useState<boolean>(false);
    const [saleOptions, setSaleOptions] = useState<any[]>([]);
    const [cardOptions, setCardOptions] = useState<any[]>([]);
    const [smOptions, setSmOptions] = useState<any[]>([]);
    const [openContract, setOpenContract] = useState<boolean>(false);
    const contractFormRef = useRef<ProFormInstance>();
    const [openPoint, setOpenPoint] = useState<boolean>(false);
    const [openLoan, setOpenLoan] = useState<boolean>(false);

    useEffect(() => {
        contractFormRef.current?.setFields([
            {
                name: 'contractCode',
                value: user?.contractCode
            }
        ])
    }, [user]);

    const onConfirm = async (id?: string) => {
        const response = await deleteUser(id);
        if (response.succeeded) {
            message.success('Deleted');
            actionRef.current?.reload();
        }
    }

    useEffect(() => {
        salesForm.current?.setFieldValue('sellerId', user?.sellerId);

        apiSalesOptions().then(response => {
            setSaleOptions(response);
        });
        apiCardOptions().then(response => {
            setCardOptions(response);
        });
        apiSmOptions().then(response => {
            setSmOptions(response);
        });

    }, [user]);

    const columns: ProColumns<any>[] = [
        {
            title: '#',
            valueType: 'indexBorder',
            width: 30,
            align: 'center'
        },
        {
            title: 'Hạng',
            dataIndex: 'cardId',
            render: (dom, entity) => <Tag color={entity.tierColor} className='w-full text-center'>{entity.tierName}</Tag>,
            width: 90,
            valueType: 'select',
            fieldProps: {
                options: cardOptions
            }
        },
        {
            title: 'Chi nhánh',
            dataIndex: 'branchId',
            search: false,
            valueType: 'select',
            minWidth: 100,
            request: apiBranchOptions
        },
        {
            title: 'Tài khoản',
            dataIndex: 'userName'
        },
        {
            title: 'Mã HĐ',
            dataIndex: 'contractCode',
            width: 100,
            minWidth: 100,
            render: (dom, entity) => (
                <div>{entity.contractCode}{entity.hasSubContract ? '⭐' : ''}</div>
            )
        },
        {
            title: 'Họ & tên',
            dataIndex: 'name',
            render: (dom, entity) => (
                <div>{entity.gender === true && (<ManOutlined className='text-blue-500' />)}{entity.gender === false && (<WomanOutlined className='text-red-500' />)} {dom}</div>
            )
        },
        {
            title: 'Email',
            dataIndex: 'email',
            hideInTable: true
        },
        {
            title: <FormattedMessage id='general.phoneNumber' />,
            dataIndex: 'phoneNumber',
            render: (dom, entity) => entity.phoneNumberHide,
            width: 90
        },
        {
            title: 'Ngày sinh',
            dataIndex: 'dateOfBirth',
            valueType: 'date',
            search: false,
            width: 100,
            render: (_, entity) => entity.dateOfBirth ? dayjs(entity.dateOfBirth).format('DD-MM-YYYY') : '-'
        },
        {
            title: 'Điểm NP',
            dataIndex: 'loyalty',
            search: false,
            valueType: 'digit',
            width: 80,
            render: (dom, entity) => (
                <Button type='link' size='small' onClick={() => {
                    setUser(entity);
                    setOpenPoint(true);
                }}>{dom}</Button>
            )
        },
        {
            title: 'Số tiền',
            dataIndex: 'amount',
            search: false,
            valueType: 'digit',
            width: 70
        },
        {
            title: 'Trợ lý',
            dataIndex: 'salesId',
            hideInTable: !access.canDos && !access.canAdmin && !access.canSm,
            valueType: 'select',
            fieldProps: {
                options: saleOptions
            },
            search: access.canSm as any
        },
        {
            title: 'Quản lý',
            dataIndex: 'smId',
            hideInTable: !access.canDos && !access.canAdmin && !access.canSm,
            render: (dom, entity) => (
                <Tooltip title={entity.smUserName}>
                    {entity.smName}
                </Tooltip>
            ),
            valueType: 'select',
            fieldProps: {
                options: smOptions
            },
            search: access.canDos
        },
        {
            title: 'Tác vụ',
            valueType: 'option',
            render: (dom, entity) => [
                <Dropdown key="more" menu={{
                    items: [
                        {
                            label: 'Xem chi tiết',
                            key: 'detail',
                            disabled: !access.canCardHolder,
                            icon: <EyeOutlined />
                        },
                        {
                            label: 'Sửa thông tin',
                            key: 'edit',
                            disabled: !access.canCRUDCardHolder,
                            icon: <EditOutlined />
                        },
                        {
                            label: 'Cập nhật hợp đồng',
                            key: 'updateContract',
                            icon: <AuditOutlined />,
                            disabled: !access.cx
                        },
                        {
                            label: 'Nạp điểm',
                            key: 'loyalty',
                            disabled: !access.canDeposit,
                            icon: <MoneyCollectOutlined />
                        },
                        {
                            key: 'topup',
                            label: 'Nạp tiền',
                            icon: <FundOutlined />,
                            disabled: !access.sales && !access.sm
                        },
                        {
                            key: 'loan',
                            label: 'Vay điểm',
                            icon: <ArrowRightOutlined />,
                            disabled: !access.cx && !access.sales && !access.canAdmin
                        }
                    ],
                    onClick: (info) => {
                        setUser(entity);
                        if (info.key === 'updateContract') {
                            setOpenContract(true);
                        }
                        if (info.key === 'detail') {
                            history.push(`/user/member/${entity.id}`);
                        }
                        if (info.key === 'loyalty') {
                            setOpenLoyalty(true);
                        }
                        if (info.key === 'edit') {
                            setOpen(true);
                        }
                        if (info.key === 'topup') {
                            setOpenTopUp(true);
                        }
                        if (info.key === 'loan') {
                            setOpenLoan(true);
                        }
                    }
                }}>
                    <Button type='dashed' size='small' icon={<MoreOutlined />}></Button>
                </Dropdown>,
                <Popconfirm title="Xác nhận xóa?" key={2} onConfirm={() => onConfirm(entity.id)}>
                    <Button type="primary" icon={<DeleteOutlined />} size='small' danger hidden={!access.canAdmin} />
                </Popconfirm>
            ],
            width: 60
        },
    ];

    return (
        <PageContainer extra={
            <Space>
                <Button type='primary' icon={<UserAddOutlined />} onClick={() => setOpen(true)} hidden={!access.canCRUDCardHolder}>Tạo mới</Button>
                <Button type='primary' icon={<SendOutlined />} onClick={() => setOpenEmail(true)} hidden={!access.canCX}>Gửi Email hàng loạt</Button>
            </Space>
        }>
            <ProTable<API.User>
                scroll={{
                    x: true
                }}
                rowKey="id"
                columns={columns}
                actionRef={actionRef}
                search={{
                    layout: 'vertical',
                }}
            />
            <CardHolderForm user={user} open={open} setOpen={setOpen} actionRef={actionRef} />
            <OpenLoyalty
                title={`Nạp điểm cho: ${user?.name} - Điểm hiện tại: ${user?.loyalty}`}
                open={openLoyalty} onOpenChange={setOpenLoyalty} onFinish={async (values) => {
                    values.userId = user?.id;
                    await apiUpdateLoyalty(values);
                    message.success('Nạp điểm thành công!');
                    actionRef.current?.reload();
                    setOpenLoyalty(false);
                }} />
            <SendEmailComponent open={openEmail} onOpenChange={setOpenEmail} />
            <TopUpModal open={openTopUp} onOpenChange={setOpenTopUp} id={user?.id} />
            <ModalForm title="Cập nhật hợp đồng" open={openContract} onOpenChange={setOpenContract} formRef={contractFormRef} onFinish={async (values) => {
                values.userId = user?.id;
                await apiUpdateContractCode(values);
                message.success('Cập nhật hợp đồng thành công!');
                actionRef.current?.reload();
                setOpenContract(false);

            }}>
                <ProFormText label="Số hợp đồng" name="contractCode" rules={
                    [
                        {
                            required: true
                        }
                    ]
                } />
            </ModalForm>
            <PointDrawer cardHolder={user} open={openPoint} onClose={() => setOpenPoint(false)} />
            <LoanPointModal open={openLoan} onOpenChange={setOpenLoan} cardHolder={user} />
        </PageContainer>
    );
};

export default CardHolderPage;
