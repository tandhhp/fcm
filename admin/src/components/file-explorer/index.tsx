import { listFile, uploadFromUrl, apiFileUpload } from '@/services/file-service';
import {
    BarsOutlined,
    DeleteOutlined,
    EyeOutlined,
    FileGifOutlined,
    FileImageTwoTone,
    FileTextTwoTone,
    HomeOutlined,
    UploadOutlined,
} from '@ant-design/icons';
import { ActionType, ProColumns, ProTable } from '@ant-design/pro-components';
import { FormattedMessage, history, useIntl } from '@umijs/max';
import {
    Breadcrumb,
    Button,
    Col,
    Dropdown,
    Input,
    message,
    Modal,
    Popconfirm,
    Row,
    Space,
    Upload,
    UploadProps,
} from 'antd';
import { useRef, useState } from 'react';

type ExplorerProps = {
    open: boolean;
    onOpenChange?: any;
    onFinish?: any;
    onSelect?: any;
    type?: string[];
};

const FileExplorer: React.FC<ExplorerProps> = (props) => {
    const intl = useIntl();
    const actionRef = useRef<ActionType>();
    const [url, setUrl] = useState<string>();

    const uploadProps: UploadProps = {
        name: 'file',
        onChange(info: any) {
            if (info.file.status !== 'uploading') {
                console.log(info.file, info.fileList);
            }
            if (info.file.status === 'done') {
                message.success(`${info.file.name} file uploaded successfully`);
                actionRef.current?.reload();
            } else if (info.file.status === 'error') {
                message.error(`${info.file.name} file upload failed.`);
            }
        },
    };

    const categories = [
        {
            key: 'images',
            name: 'Images',
            icon: <FileImageTwoTone twoToneColor={'#eb2f96'} />
        },
        {
            key: 'videos',
            name: 'Videos',
            icon: <FileGifOutlined />
        },
        {
            key: 'text',
            name: 'Documents',
            icon: <FileTextTwoTone twoToneColor={'#52c41a'} />
        }
    ]

    const columns: ProColumns<API.FileContent>[] = [
        {
            title: '#',
            valueType: 'indexBorder'
        },
        {
            title: 'Name',
            dataIndex: 'name'
        },
        {
            title: 'Date modified',
            dataIndex: 'modifiedDate'
        },
        {
            title: 'Type',
            dataIndex: 'type'
        },
        {
            title: 'Size',
            dataIndex: 'size'
        },
        {
            title: 'Action',
            render: (_, row) => (
                <Dropdown
                    menu={{
                        items: [
                            {
                                key: 1,
                                label: (
                                    <Space
                                        onClick={() =>
                                            history.push(`/files/center/${row.id}`)
                                        }
                                    >
                                        <EyeOutlined />
                                        <FormattedMessage id="general.preview" />
                                    </Space>
                                ),
                            },
                            {
                                key: 2,
                                label: (
                                    <Popconfirm title="Are you sure?">
                                        <Space>
                                            <DeleteOutlined />
                                            Delete
                                        </Space>
                                    </Popconfirm>
                                ),
                                danger: true,
                            },
                        ],
                    }}
                >
                    <Button icon={<BarsOutlined />} type="link" size="small" />
                </Dropdown>
            ),
        }
    ]

    return (
        <Modal
            title="File Explorer"
            open={props.open}
            onCancel={() => props.onOpenChange()}
            centered
            width={1100}
            footer={false}
        >
            <div className="mb-4">
                <Row gutter={16}>
                    <Col span={4}>
                    </Col>
                    <Col span={20}>
                        <div className='flex items-center justify-between'>
                            <Breadcrumb>
                                <Breadcrumb.Item href="">
                                    <HomeOutlined />
                                </Breadcrumb.Item>
                                <Breadcrumb.Item href="">Home</Breadcrumb.Item>
                            </Breadcrumb>
                            <div>
                                <Input.Search placeholder='Search' />
                            </div>
                        </div>
                    </Col>
                </Row>
            </div>
            <Row gutter={16}>
                <Col span={4} style={{
                    borderRight: '1px solid #d1d1d1'
                }}>
                    {
                        categories.map(category => (
                            <div key={category.key} className='hover-light'>
                                <Button type='link' icon={category.icon}>{category.name}</Button>
                            </div>
                        ))
                    }
                </Col>
                <Col span={20}>
                    <ProTable<API.FileContent>
                        rowSelection={{}}
                        toolBarRender={() => {
                            return [
                                <Upload {...uploadProps} key={0}>
                                    <Button icon={<UploadOutlined />} type="primary">
                                        Upload
                                    </Button>
                                </Upload>,
                            ];
                        }}
                        search={false}
                        columns={columns}
                        request={(params) =>
                            listFile(
                                {
                                    ...params,
                                },
                                props.type,
                            )
                        }
                        pagination={{
                          defaultPageSize: 8
                        }}
                        actionRef={actionRef}
                    />
                </Col>
            </Row>
        </Modal>
    );
};

export default FileExplorer;
